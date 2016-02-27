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
