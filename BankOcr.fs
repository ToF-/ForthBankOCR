\ BankOcr

: CTABLE CREATE DOES> + C@ ;

3      CONSTANT WIDTH
3      CONSTANT HEIGHT
CHAR ? CONSTANT NOT-FOUND

VARIABLE ACCOUNT-SIZE 9 ACCOUNT-SIZE !

: SIZE ACCOUNT-SIZE @ ;

CREATE ACCOUNT SIZE ALLOT

: .ACCOUNT ACCOUNT SIZE TYPE ;

: OCR-WIDTH SIZE WIDTH * ;


2 BASE !
CTABLE DIGITS  
1101111 C, 0001001 C, 1011110 C, 1011011 C, 0111001 C,  
1110011 C, 1110111 C, 1001001 C, 1111111 C, 1111011 C,
DECIMAL 


: BAR? 32 <> 1 AND ;

: POS ( i -- pos of char i in ocr digit )
    ?DUP 0=  IF  1  
    ELSE
        1- HEIGHT /MOD 1+ 
        OCR-WIDTH * 
        SWAP + 
    THEN ;
       
: ADDRESS ( n i -- addr  char i or nth ocr digit ) 
    POS SWAP WIDTH * + PAD + ;

: OCR-BIT ( n i -- 0|1  bar at char i of nth ocr digit )
    ADDRESS C@ BAR? ;

: OCR>PATTERN ( n -- b )
    0000000
    7 0 DO  1 LSHIFT OVER I OCR-BIT OR  LOOP
    NIP ;

2 BASE !
: >CHAR 110000 OR ;
: >INT  001111 AND ;
DECIMAL

: PATTERN>DIGIT ( b -- c )
    NOT-FOUND 10 0 DO OVER I DIGITS = IF DROP I LEAVE THEN LOOP
    NIP >CHAR ;

: OCR>DIGIT ( i -- c )
    OCR>PATTERN PATTERN>DIGIT ;

: OCR>ACCOUNT ( -- )
    SIZE 0 DO
        I OCR>DIGIT
        ACCOUNT I + C! 
    LOOP ;

: ILLEGIBLE? ( -- f|t )
    FALSE 
    SIZE 0 DO 
        I ACCOUNT + C@ NOT-FOUND = OR 
    LOOP ;

: ERROR? ( -- f|t )
    0
    SIZE 0 DO  ACCOUNT I + C@  >INT SIZE I - * + LOOP
    11 MOD ;

: PROCESS-OCR
    OCR>ACCOUNT 
    .ACCOUNT SPACE
    ILLEGIBLE? IF ."  ILL"
    ELSE ERROR? IF ."  ERR" THEN
    THEN CR ;


VARIABLE LINE-OFFSET 

: PROCESS-FILE ( addr,u -- )
    R/O OPEN-FILE THROW 
    0 LINE-OFFSET !
    BEGIN
        PAD LINE-OFFSET @ + OVER 100 SWAP READ-LINE THROW 
    WHILE
        LINE-OFFSET +!
        LINE-OFFSET @ 81 >= IF 
            PROCESS-OCR
            0 LINE-OFFSET !
        THEN
    REPEAT ;
