\ BankOcr

9 CONSTANT SIZE

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

: BYTE>DIGIT ( byte -- digit|NF )
    NOT-FOUND 10 0 DO
        OVER PATTERNS I + C@ = IF DROP I LEAVE THEN
    LOOP NIP ;

CREATE BITS SIZE ALLOT
CREATE BITS' SIZE ALLOT
CREATE ACCOUNTS SIZE SIZE * ALLOT
VARIABLE ALT-MAX ALT-MAX OFF

: ACCOUNT ( -- addr )
    ACCOUNTS ALT-MAX @ SIZE * + ;


: TO-CHAR ( c -- n )
    48 OR ;

: TO-INT ( c -- n )
    15 AND ;

: BITS>ACCOUNT ( srce,dest,n -- )
    0 DO
        >R DUP C@ BYTE>DIGIT TO-CHAR ( srce,c -- )
        R@ C! 
        1+ R> 1+ 
    LOOP 2DROP ;

: ILLEGIBLE? ( str,n -- t|f )
    FALSE -ROT 0 DO
        DUP C@ NOT-FOUND = ROT OR SWAP 1+
    LOOP DROP ; 

: CHECKSUM? ( str,n -- t|f )
    0 -ROT 0 DO    
        DUP C@ TO-INT SIZE I - * ROT + SWAP 1+ 
    LOOP DROP 11 MOD 0= ;

: ADD-MISSING-BIT ( n,byte -- byte' )
    1 ROT LSHIFT OR ;

: .ACCOUNT ( str,n )
    2DUP TYPE
    2DUP ILLEGIBLE? IF ."  ILL" 2DROP 
    ELSE CHECKSUM? 0= IF ."  ERR" 
    THEN THEN CR ;

: ALT-ACCOUNT ( -- addr )
    ACCOUNTS ALT-MAX @ SIZE * + ;

: DUPLICATE-BITS ( -- )
    BITS BITS' SIZE CMOVE ;

: CHANGE-MISSING-BIT ( addr,n -- )
    OVER C@ ADD-MISSING-BIT SWAP C! ;

: VALID? ( addr,n -- f|t )
    2DUP ILLEGIBLE? 0= -ROT CHECKSUM? AND ;

: ADD-ALT-ACCOUNT ( addr,n -- )
    ALT-MAX ?
    ALT-ACCOUNT SWAP CMOVE 1 ALT-MAX +! ;

: ADD-ALTERNATIVE ( addr,n -- )
    2DUP VALID? IF ADD-ALT-ACCOUNT ELSE 2DROP THEN ;

CREATE TMP-ACCOUNT SIZE ALLOT

: BIT-PATTERN-CHANGED? ( -- f|t )
    BITS' SIZE BITS SIZE COMPARE ;

: FIND-MISSING-IN-OCR ( addr -- ) 
    8 0 DO 
        DUPLICATE-BITS
        DUP I CHANGE-MISSING-BIT BIT-PATTERN-CHANGED? IF
            BITS' TMP-ACCOUNT SIZE BITS>ACCOUNT
            TMP-ACCOUNT SIZE ADD-ALTERNATIVE  
        THEN
    LOOP DROP ;
        
: FIND-MISSING-BAR ( -- )
    ALT-MAX OFF 
    SIZE 0 DO BITS' I + FIND-MISSING-IN-OCR LOOP ;

    
: PROCESS-LINE ( str,n -- )
    ?DUP IF BITS -ROT ENCODE-LINE 
    ELSE DROP 
        BITS ACCOUNT SIZE BITS>ACCOUNT
        FIND-MISSING-BAR
        ACCOUNT SIZE .ACCOUNT
    THEN ;

: PROCESS-FILE ( str,n -- )
    R/O OPEN-FILE THROW >R PAD 30 ERASE
    BEGIN
        PAD 40 R@ READ-LINE THROW
    WHILE
        PAD SWAP  PROCESS-LINE
    REPEAT R> DROP ;
    
