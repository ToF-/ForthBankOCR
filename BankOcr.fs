: BYTE-POS ( addr,n -- addr ) 3 / + ;
: BAR?     ( addr -- t|f )    C@ 32 <> 1 AND ;
: BIT!     ( addr b -- )      OVER C@ 1 LSHIFT OR SWAP C! ;

: OCR>BITS ( src dst -- )
    SWAP 0 DO 
        2DUP I BYTE-POS  ( src,dst,src,di)
        SWAP I + BAR?    ( src,dst,di,b )
        BIT!  
    LOOP 2DROP ;


2 BASE !
CREATE DIGITS  
010101111 C, 000001001 C, 010011110 C, 010011011 C, 000111001 C,  
010110011 C, 010110111 C, 010001001 C, 010111111 C, 010111011 C,
: >CHAR 110000 OR ;
DECIMAL 

CHAR ? CONSTANT NOT-FOUND
: >DIGIT NOT-FOUND 10 0 DO OVER DIGITS I + C@ = IF DROP I LEAVE THEN LOOP NIP >CHAR ;

: >DIGITS 0 DO DUP I + C@ >DIGIT OVER I + C! LOOP DROP ;



