
CREATE OCRBITS 9 3 * 3 * ALLOT 

: BAR? ( c -- 0|1 ) 32 <> 1 AND ;
: >BIN ( b1,b2,b2 -- binary pattern ) SWAP 2* OR SWAP 2* 2* OR ;
: >BYTE ( b1,b2,b3 -- byte ) ROT 2* 2* 2* ROT OR 2* 2* 2* OR  ; 

: 2*OR ( n b -- n*2 or b ) SWAP 2* OR ;
: OCRBIT ( r c -- b ) SWAP 27 * + OCRBITS C@ BAR? ;  


2 BASE !
CREATE DIGITS 010101111 C, 00001001 C, 010011110 C, 10011011 C, 00111001 C, 10110011 C, 10110111 C, 10001001 C, 10111111 C, 10111011 C, 
DECIMAL

: >DIGIT ( byte -- n) -1 10 0 DO OVER DIGITS I + C@ = IF DROP I THEN LOOP SWAP DROP ; 

CREATE DIGITS 9 ALLOT 
\  0         1         2         3         4         5         6         7         8
\  01234567890123456789012345678901234567890123456789012345678901234567890123456789012
S"  _     _  _     _  _  _  _ | |  | _| _||_||_ |_   ||_||_|  ||_  _|  | _||_|  ||_|"
OCR SWAP CMOVE







