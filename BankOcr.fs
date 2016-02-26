\ BankOcr

: BAR?  ( c -- 0|1 )
    BL <> 1 AND ;    

: #BAR? ( addr,i -- 0|1 )
    + C@ BAR? ;

: <<BIT ( b,byte -- byte' )
    1 LSHIFT OR ;

: <<BIT! ( b,addr -- )
    SWAP OVER C@ <<BIT SWAP C! ;

: OCR>BYTE! ( srce,dest -- )
    3 0 DO
        OVER I #BAR? ( srce,dest,b -- )
        OVER <<BIT! 
    LOOP 2DROP ;

: SIZE 3 / ;
: OCR# 3 *  + ;

: OCR>BYTES! ( srce,n,dest -- )
    SWAP SIZE 0 DO
        OVER I OCR#  
        OVER I + OCR>BYTE!
    LOOP 2DROP ;

CREATE OCRBYTES 10 ALLOT
OCRBYTES 10 ERASE
S"  _     _  _     _  _  _  _  _ " OCRBYTES OCR>BYTES!
S" | |  | _| _||_||_ |_   ||_||_|" OCRBYTES OCR>BYTES!
S" |_|  ||_  _|  | _||_|  ||_| _|" OCRBYTES OCR>BYTES!
        
