\ BankOcr

: BAR? ( c -- bit )
    BL <> 1 AND ;

: BIT! ( bit,byte -- byte' )
    1 LSHIFT OR ;

: ENCODE ( byte,str,n -- byte' )
    0 DO 
        DUP I + C@ BAR?
        ROT BIT! SWAP
    LOOP DROP ;

: ENCODE-LINE ( dst,str,n -- )
    3 / 0 DO            
        OVER C@ OVER 3 ENCODE   
        >R OVER R> SWAP C!
        SWAP 1+ SWAP 3 + 
    LOOP 2DROP ;        
