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
CREATE ACCOUNTS SIZE SIZE * ALLOT
VARIABLE ACCOUNT# ACCOUNT# OFF

: ACCOUNT ( -- addr )
    ACCOUNTS ACCOUNT# @ SIZE * + ;


: >CHAR ( c -- n )
    48 OR ;

: >INT ( c -- n )
    15 AND ;

: BITS>ACCOUNT ( srce,dest,n -- )
    0 DO
        >R DUP C@ BYTE>DIGIT >CHAR ( srce,c -- )
        R@ C! 
        1+ R> 1+ 
    LOOP 2DROP ;

: ILLEGIBLE? ( str,n -- t|f )
    FALSE -ROT 0 DO DUP C@ NOT-FOUND = ROT OR SWAP 1+ LOOP DROP ; 

: CHECKSUM? ( str,n -- t|f )
    0 -ROT 0 DO DUP C@ >INT SIZE I - * ROT + SWAP 1+ LOOP
    DROP 11 MOD 0= ;

: SET-BIT ( n,byte -- byte' )
    1 ROT LSHIFT OR ;

: .ACCOUNT ( str,n )
    2DUP TYPE
    2DUP ILLEGIBLE? IF ."  ILL" 2DROP 
    ELSE CHECKSUM? 0= IF ."  ERR" 
    THEN THEN CR ;

: ALT-ACCOUNT ( -- addr )
    ACCOUNTS ACCOUNT# @ SIZE * + ;

: SET-BIT! ( addr,n -- )
    OVER C@ SET-BIT SWAP C! ;

: VALID? ( addr,n -- f|t )
    2DUP ILLEGIBLE? 0= -ROT CHECKSUM? AND ;

: ADD-ACCOUNT-IF-VALID ( -- )
    ACCOUNT SIZE VALID? IF 1 ACCOUNT# +! THEN ;

: FIND-MISSING-IN-OCR ( addr -- ) 
    8 0 DO 
        DUP C@ SWAP 
        DUP I SET-BIT! OVER OVER C@ <> IF
            BITS ACCOUNT SIZE BITS>ACCOUNT
            ADD-ACCOUNT-IF-VALID
        THEN SWAP OVER C!
    LOOP DROP ;
        
: FIND-MISSING-BAR ( -- )
    SIZE 0 DO 
BITS I + FIND-MISSING-IN-OCR LOOP ;

    
: PROCESS-LINE ( str,n -- )
    ?DUP IF BITS -ROT ENCODE-LINE 
    ELSE DROP 
        ACCOUNTS SIZE SIZE * ERASE ACCOUNT# OFF 
        BITS ACCOUNT SIZE BITS>ACCOUNT
        ADD-ACCOUNT-IF-VALID
        ACCOUNT# @ 0= IF ACCOUNT SIZE .ACCOUNT ELSE
            FIND-MISSING-BAR 
            ACCOUNT# @ 0 ?DO
            ACCOUNTS I SIZE * + SIZE TYPE SPACE 
        LOOP CR
        THEN

    THEN ;

: PROCESS-FILE ( str,n -- )
    ACCOUNTS SIZE SIZE * ERASE ACCOUNT# OFF 
    R/O OPEN-FILE THROW >R PAD 30 ERASE
    BEGIN
        PAD 40 R@ READ-LINE THROW
    WHILE
        PAD SWAP  PROCESS-LINE
    REPEAT R> DROP ;
    
