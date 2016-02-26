\ gforth Tests.fs

S" BankOcr.fs" REQUIRED

\ bit patterns
\
\  10
\ 101
\ 111
\  _     _  _     _  _  _  _  _ 
\ | |  | _| _||_||_ |_   ||_||_|
\ |_|  ||_  _|  | _||_|  ||_| _|

CHAR | CONSTANT PIPE
CHAR _ CONSTANT UNDERSCORE

: TESTS
    ASSERT( BL BAR? 0 = )
    ASSERT( PIPE BAR? 1 = )
    ASSERT( UNDERSCORE BAR? 1 = )

    S"  _     _  _     _  _  _  _  _ " ( addr,size -- )
    
    DROP \ we don't need the size

    ASSERT( DUP 0 #BAR? 0 = )
    ASSERT( DUP 1 #BAR? 1 = )
    ASSERT( DUP 2 #BAR? 0 = )
    \ visual test of bars 
    \ 30 0 DO DUP I #BAR? . LOOP CR
    DROP
    [ 2 BASE ! ] 
    ASSERT( 1 0  <<BIT   1 = )
    ASSERT( 1 1  <<BIT  11 = )
    ASSERT( 0 11 <<BIT 110 = )
    ASSERT( 0 PAD C!
            1 PAD <<BIT!
            0 PAD <<BIT!
            1 PAD <<BIT!
            1 PAD <<BIT!
            PAD C@ 1011 = ) 
    ASSERT( S"  _ " DROP PAD OCR>BYTE!
            S" | |" DROP PAD OCR>BYTE!
            S" |_|" DROP PAD OCR>BYTE!
            PAD C@ 10101111 = ) 
    ASSERT( S"  _    " PAD OCR>BYTES!
            S" | |  |" PAD OCR>BYTES!
            S" |_|  |" PAD OCR>BYTES!
            PAD 1+ C@ 00001001 = ) 
    [ DECIMAL ] 
    OCRBYTES 10 DUMP


    



;

TESTS
." SUCCESS"
BYE
