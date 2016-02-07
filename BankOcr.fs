
3 CONSTANT OCR-WIDTH 
3 CONSTANT OCR-HEIGHT
9 CONSTANT ACCOUNT-SIZE
CHAR ? CONSTANT NOT-FOUND
OCR-WIDTH ACCOUNT-SIZE * CONSTANT LINE-SIZE

CREATE OCR-BITS 81 ALLOT 


2 BASE !
CREATE DIGITS 010101111 C, 00001001 C, 010011110 C, 10011011 C, 00111001 C,
              010110011 C, 10110111 C, 010001001 C, 10111111 C, 10111011 C, 
DECIMAL

CREATE ACCOUNT ACCOUNT-SIZE ALLOT 
ACCOUNT ACCOUNT-SIZE ERASE

: BAR?   ( c -- f|t  leaves True is the char is a bar, False is a space ) 
    32 <> 1 AND ;

: 2*OR   ( byte,bit -- byte'  shift-left the byte and store a new bit into the right position ) 
    SWAP 2* OR ;

: OCR-BIT-OFFSET ( n,p -- [p/3]*27+[p%3]+n*3  ) 
    OCR-HEIGHT /MOD LINE-SIZE * + 
    SWAP OCR-WIDTH * + ;

: PATTERN ( n -- byte  converts one OCR digit into its binary pattern ) 
    0  9 0 DO  
        OVER I OCR-BIT-OFFSET OCR-BITS + C@ BAR? 2*OR 
    LOOP NIP ;

: DIGIT-PATTERN ( i -- n )
    DIGITS + C@ ;

: DIGIT>CHAR ( n -- c  convert digit [0..9] to 'O'..'9', and 15 to '?' )
    [ 2 BASE ! ] 110000 OR [ DECIMAL ] ;

: CHAR>DIGIT ( n -- c  convert char ['0'..'9'] to 0..9 and '?' to 15 )
    [ 2 BASE ! ] 001111 AND [ DECIMAL ] ;

: DIGIT  ( byte -- c|?  search the digits table for a digit or ? if not found) 
    NOT-FOUND   10 0 DO
        OVER I DIGIT-PATTERN = IF  DROP I DIGIT>CHAR THEN  
    LOOP NIP ; 

: OCR>ACCOUNT ( --   converts OCR data to account numbert )
    9 0 DO   
        I PATTERN DIGIT 
        ACCOUNT I + C! 
    LOOP ;

: SUMCHECK-ERROR? ( -- f|t  checks that [d0*9+d1*8+..+d8] is a multiple of 11 )
    0   9 0 DO 
        ACCOUNT I + C@ 
        CHAR>DIGIT 9 I - * + 
    LOOP   11 MOD ;

: ILLEGIBLE? ( -- f|t    checks the account for illegible digit )
    FALSE 9 0 DO ACCOUNT I + C@ NOT-FOUND = IF DROP TRUE THEN LOOP ;

: PRINT-ACCOUNT \ prints the account with suffix if illegal or error
    ACCOUNT 9 TYPE 
    ILLEGIBLE?      IF ."  ILL" ELSE 
    SUMCHECK-ERROR? IF ."  ERR" THEN THEN ;


