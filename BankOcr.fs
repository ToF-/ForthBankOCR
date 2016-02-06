
3 CONSTANT OCR-WIDTH 
3 CONSTANT OCR-HEIGHT
9 CONSTANT ACCOUNT-SIZE
255 CONSTANT NOT-FOUND
OCR-WIDTH ACCOUNT-SIZE * CONSTANT LINE-SIZE

CREATE OCR-BITS 81 ALLOT 

( TEST OCR PATTERNS )
\  0         1         2         3         4         5         6         7         8
\  01234567890123456789012345678901234567890123456789012345678901234567890123456789012
S"  _     _  _     _  _  _  _ | |  | _| _||_||_ |_   ||_||_|  ||_  _|  | _||_|  ||_|"
OCR-BITS SWAP CMOVE

2 BASE !
CREATE DIGITS 010101111 C, 00001001 C, 010011110 C, 10011011 C, 00111001 C,
              010110011 C, 10110111 C, 010001001 C, 10111111 C, 10111011 C, 
DECIMAL

CREATE ACCOUNT ACCOUNT-SIZE ALLOT 
ACCOUNT ACCOUNT-SIZE ERASE

: BAR?   ( c -- f|t ) 
    32 <> 1 AND ;

: 2*OR   ( c,b -- c*2 or b ) 
    SWAP 2* OR ;

: OCR-BIT-OFFSET ( n,p -- p ) 
    OCR-HEIGHT /MOD LINE-SIZE * + 
    SWAP OCR-WIDTH * + ;

: PATTERN ( n -- byte) 
    0  ACCOUNT-SIZE 0 DO  
        OVER I OCR-BIT-OFFSET OCR-BITS + C@ BAR? 2*OR 
    LOOP NIP ;

: @DIGIT ( i -- n )
    DIGITS + C@ ;

: DIGIT  ( byte -- n|-1) 
    NOT-FOUND  10 0 DO
        OVER I @DIGIT = IF  DROP I  THEN  
    LOOP NIP ; 

: ACCOUNT-DIGIT ( i -- n ) DUP PATTERN DIGIT SWAP ACCOUNT + C! ;

: OCR>ACCOUNT ( -- ) 
    9 0 DO I ACCOUNT-DIGIT LOOP ;









