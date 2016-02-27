\ BankOcr

: BAR?  ( c -- 0|1 )
    BL <> 1 AND ;    

: #BAR? ( addr,i -- 0|1 )
    + C@ BAR? ;

: <<BIT ( b,byte -- byte' )
    1 LSHIFT OR ;

: <<BIT! ( b,addr -- )
    SWAP OVER C@ <<BIT SWAP C! ;

: <<BITS ( srce,dest -- )
    3 0 DO
        OVER I #BAR? ( srce,dest,b -- )
        OVER <<BIT! 
    LOOP 2DROP ;

: SIZE 3 / ;
: OCR# 3 *  + ;

: ENCODE ( srce,n,dest -- )
    SWAP SIZE 0 DO
        OVER I OCR#  
        OVER I + <<BITS
    LOOP 2DROP ;

CREATE OCR-TABLE 10 ALLOT
OCR-TABLE 10 ERASE
S"  _     _  _     _  _  _  _  _ " OCR-TABLE ENCODE
S" | |  | _| _||_||_ |_   ||_||_|" OCR-TABLE ENCODE
S" |_|  ||_  _|  | _||_|  ||_| _|" OCR-TABLE ENCODE

15 CONSTANT NOT-FOUND 
: FIND-DIGIT ( byte -- n )
    NOT-FOUND 10 0 DO       ( byte,NF -- )
        OVER OCR-TABLE I + C@ ( byte,NF,byte,p -- )
        = IF DROP I LEAVE THEN ( byte,i -- )
    LOOP NIP ;

: >CHAR ( n -- c )
    48 + ;

: >INT ( c -- n )
    15 AND ;

: OCR>ACCOUNT ( srce,dest -- )
    9 0 DO
        OVER I + C@ FIND-DIGIT >CHAR
        OVER I + C!
    LOOP 2DROP ;
        
: CHECKSUM ( srce -- f )
    0 9 0 DO
        OVER I + C@ >INT 
        9 I - * +
    LOOP 11 MOD 0= NIP ;

: ILLEGIBLE ( srce -- f )
    FALSE 9 0 DO 
        OVER I + C@ 
        >INT NOT-FOUNd = OR
    LOOP NIP ;

: .ACCOUNT ( srce -- )
    DUP 9 TYPE SPACE
    DUP ILLEGIBLE IF ." ILL" ELSE
    CHECKSUM 0=   IF ." ERR" THEN THEN ;

CREATE OCR 9 ALLOT
CREATE ACCOUNT 9 ALLOT

: PROCESS-ACCOUNT ( -- )
    PAD ACCOUNT OCR>ACCOUNT
    ACCOUNT .ACCOUNT CR ;

: PROCESS-FILE ( addr,n -- )
    R/O OPEN-FILE THROW >R
    OCR 9 ERASE
    BEGIN
        PAD 30 R@ READ-LINE THROW
    WHILE
        ?DUP IF PAD SWAP OCR ENCODE 
    ELSE 
        PROCESS-ACCOUNT 
        OCR 9 ERASE
    THEN
    REPEAT R> 2DROP ;
    
