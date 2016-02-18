\ BankOcr

: CTABLE CREATE DOES> + C@ ;

3  CONSTANT WIDTH
3  CONSTANT HEIGHT

VARIABLE SIZE 9 SIZE !

CREATE ACCOUNT SIZE @ ALLOT

: .ACCOUNT ACCOUNT SIZE @ TYPE ;

: OCR-WIDTH SIZE @ WIDTH * ;

CREATE OCR OCR-WIDTH HEIGHT * ALLOT

: BAR? 32 <> 1 AND ;

: POS ( i -- pos of char i in ocr digit )
    ?DUP 0=  IF  1  
    ELSE
        1- HEIGHT /MOD 1+ 
        OCR-WIDTH * 
        SWAP + 
    THEN ;
       
: ADDRESS ( n i -- addr  char i or nth ocr digit ) 
    POS SWAP WIDTH * + OCR + ;

: EMPTY-OCR OCR OCR-WIDTH HEIGHT * ERASE ;

: OCR>PATTERN ( n -- b )
    DUP 0 ADDRESS C@ BAR? ( n b )
    7 1 DO 1 LSHIFT OVER I ADDRESS C@ BAR?  OR LOOP
    NIP ;

2 BASE !
CTABLE DIGITS  
1101111 C, 0001001 C, 1011110 C, 1011011 C, 0111001 C,  
1110011 C, 1110111 C, 1001001 C, 1111111 C, 1111011 C,
DECIMAL 
2 BASE !

: >CHAR ( d -- c ) 
    110000 OR ;
DECIMAL

CHAR ? CONSTANT NOT-FOUND

: PATTERN>DIGIT ( b -- c )
    NOT-FOUND 10 0 DO OVER I DIGITS = IF DROP I LEAVE THEN LOOP
    NIP >CHAR ;

: OCR>DIGIT ( i -- c )
    OCR>PATTERN PATTERN>DIGIT ;

: OCR>ACCOUNT ( -- )
    SIZE @ 0 DO
        I OCR>DIGIT
        ACCOUNT I + C! 
    LOOP ;

: ILLEGIBLE? ( -- f|t )
    FALSE SIZE @ 0 DO I ACCOUNT + C@ NOT-FOUND = OR LOOP ;
