\ BankOcr

: BAR? ( c -- 0|1 )
    BL <> 1 AND  ;

: #BAR? ( adr,i -- 0|1 )
    + C@ BAR? ;

: <<BIT! ( b,adr -- )
    DUP C@ 
    1 LSHIFT ROT OR 
    SWAP C! ;

: BYTE# ( n -- n/3 )
    3 / ;

: OCR-BIT! ( n,b,adr -- )
    ROT BYTE# + <<BIT! ;

: OCR-BITS! ( src,max,dst -- )
    SWAP 0 DO 
        OVER I #BAR? 
        OVER I -ROT OCR-BIT! 
    LOOP  2DROP ;
    
