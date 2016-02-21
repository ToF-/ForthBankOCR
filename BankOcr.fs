\ BankOcr 
3 CONSTANT WIDTH
9 CONSTANT SIZE
2 BASE !
CHAR ? CONSTANT NOT-FOUND
CREATE DIGITS  ( OCR patterns in digit order 0,1,2..9 )
010101111 C, 000001001 C, 010011110 C, 010011011 C, 000111001 C,  
010110011 C, 010110111 C, 010001001 C, 010111111 C, 010111011 C,
: >CHAR 110000 OR ;
: >INT  001111 AND ;
DECIMAL 
: BAR?   32 <> 1 AND ;
: BIT<<  SWAP 1 LSHIFT OR ;  

: OCR>BITS ( src,u,dst -- converts an OCR line into bits )
    SWAP 0 DO 2DUP        
        I WIDTH / + DUP C@     
        ROT I + C@ BAR? 
        BIT<< SWAP C! 
    LOOP 2DROP ;

: BIT-OCR>DIGIT ( byte -- c  find the digit corresponding to the ocr bit pattern or '?' )
    NOT-FOUND 10 0 DO 
        OVER DIGITS I + C@ = IF DROP I LEAVE THEN 
    LOOP NIP >CHAR ;

: >DIGITS ( src,dst,n -- converts OCR bit patterns into digits )
    0 DO 
        OVER I + C@ BIT-OCR>DIGIT 
        OVER I + C!
    LOOP 2DROP ;

: LEGIBLE ( src,n -- f|t true is the string doesn't contain ? )
    FALSE SWAP 0 DO 
        OVER I + C@ NOT-FOUND = OR 
    LOOP 0= NIP ;

: CHECKSUM ( src,n -- f|t true if the account checksums )
    0 SWAP 0 DO
        OVER I + C@ >INT SIZE I - * + 
    LOOP 11 MOD 0= NIP ;

: ADD-OCR-BIT ( adr n b -- adds a bit in position b on adr+n )
    1 SWAP LSHIFT -ROT + DUP C@ ROT OR SWAP C! ; 
        


