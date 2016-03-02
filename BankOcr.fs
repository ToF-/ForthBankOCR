\ BankOcr

: BAR? ( c -- bit )
    BL <> 1 AND ;

: BIT! ( bit,byte -- byte' )
    1 LSHIFT OR ;

: ENCODE-OCR ( byte,str,n -- byte' )
    0 DO 
        DUP I + C@ BAR?
        ROT BIT! SWAP
    LOOP DROP ;

: ENCODE-LINE ( dst,str,n -- )
    3 / 0 DO            
        OVER C@ OVER 3 ENCODE-OCR   
        >R OVER R> SWAP C!
        SWAP 1+ SWAP 3 + 
    LOOP 2DROP ;        


CREATE PATTERNS 10 ALLOT
PATTERNS 10 ERASE
PATTERNS S"  _     _  _     _  _  _  _  _ " ENCODE-LINE
PATTERNS S" | |  | _| _||_||_ |_   ||_||_|" ENCODE-LINE
PATTERNS S" |_|  ||_  _|  | _||_|  ||_| _|" ENCODE-LINE

CHAR ? CONSTANT NOT-FOUND
: FIND-DIGIT ( byte -- digit|NF )
    NOT-FOUND 10 0 DO
        OVER PATTERNS I + C@ = IF DROP I LEAVE THEN
    LOOP NIP ;

CREATE OCR-BITS 9 ALLOT
CREATE ACCOUNT 9 ALLOT

: TO-CHAR ( c -- n )
    48 OR ;

: TO-INT ( c -- n )
    15 AND ;

: FIND-DIGITS ( srce,dest,n -- )
    0 DO
        >R DUP C@ FIND-DIGIT TO-CHAR ( srce,c -- )
        R@ C! 
        1+ R> 1+ 
    LOOP 2DROP ;

: ILLEGIBLE? ( str,n -- t|f )
    FALSE -ROT 0 DO
        DUP C@ NOT-FOUND = ROT OR SWAP 1+
    LOOP DROP ; 

: CHECKSUM? ( str,n -- t|f )
    0 -ROT 0 DO    
        DUP C@ TO-INT 9 I - * ROT + SWAP 1+ 
    LOOP DROP 11 MOD 0= ;

: ADD-MISSING-BIT ( n,byte -- byte' )
    1 ROT LSHIFT OR ;

: .ACCOUNT ( str,n )
    2DUP TYPE
    2DUP ILLEGIBLE? IF ."  ILL" 2DROP 
    ELSE CHECKSUM? 0= IF ."  ERR" 
    THEN THEN CR ;

CREATE ALT-OCR-BITS 9 ALLOT
CREATE ALT-ACCOUNTS 9 9 * ALLOT
VARIABLE ALT-ACCOUNTS-MAX

: ALT-ACCOUNT ( -- addr )
    ALT-ACCOUNTS ALT-ACCOUNTS-MAX @ 9 * + ;

: DUPLICATE-OCR-BITS ( -- )
    OCR-BITS ALT-OCR-BITS 9 CMOVE ;

: CHANGE-MISSING-BIT ( addr,n -- )
    OVER C@ ADD-MISSING-BIT SWAP C! ;

: VALID? ( addr,n -- f|t )
    2DUP ILLEGIBLE? 0= -ROT CHECKSUM? AND ;

: ADD-ALT-ACCOUNT ( addr,n -- )
    ALT-ACCOUNT SWAP CMOVE 1 ALT-ACCOUNTS-MAX +! ;

: ADD-ALTERNATIVE ( addr,n -- )
    2DUP VALID? IF ADD-ALT-ACCOUNT ELSE 2DROP THEN ;

CREATE TMP-ACCOUNT 9 ALLOT

: FIND-MISSING-BAR ( -- )
    0 ALT-ACCOUNTS-MAX ! 
    8 0 DO DUPLICATE-OCR-BITS
           ALT-OCR-BITS I CHANGE-MISSING-BIT
           ALT-OCR-BITS TMP-ACCOUNT 9 FIND-DIGITS
           TMP-ACCOUNT 9 ADD-ALTERNATIVE
    LOOP ;

    
: PROCESS-LINE ( str,n -- )
    ?DUP IF OCR-BITS -ROT ENCODE-LINE 
    ELSE DROP 
        OCR-BITS ACCOUNT 9 FIND-DIGITS
        ACCOUNT 9 ILLEGIBLE? IF FIND-MISSING-BAR THEN
        ACCOUNT 9 .ACCOUNT
    THEN ;

: PROCESS-FILE ( str,n -- )
    R/O OPEN-FILE THROW >R PAD 30 ERASE
    BEGIN
        PAD 40 R@ READ-LINE THROW
    WHILE
        PAD SWAP  PROCESS-LINE
    REPEAT R> DROP ;
    
