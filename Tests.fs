\ gforth Tests.fs

S" BankOcr.fs" REQUIRED

: =? ( n,n' -- f|t with message if false )
    2DUP <> IF ." EXPECTED " . ." BUT WAS " . CR FALSE ELSE = THEN ;

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

: TEST-ENCODE 
    >R >R >R >R 
    PAD ENCODE
    R> R> 
    PAD ENCODE
    R> R> 
    PAD ENCODE ;

: TESTS
    ASSERT( BL BAR? 0 =? )
    ASSERT( PIPE BAR? 1 =? )
    ASSERT( UNDERSCORE BAR? 1 =? )

    S"  _     _  _     _  _  _  _  _ " ( addr,size -- )
    
    DROP \ we don't need the size

    ASSERT( DUP 0 #BAR? 0 =? )
    ASSERT( DUP 1 #BAR? 1 =? )
    ASSERT( DUP 2 #BAR? 0 =? )
    \ visual test of bars 
    \ 30 0 DO DUP I #BAR? . LOOP CR
    DROP
    [ 2 BASE ! ] 
    ASSERT( 1 0  <<BIT   1 =? )
    ASSERT( 1 1  <<BIT  11 =? )
    ASSERT( 0 11 <<BIT 110 =? )
    ASSERT( 0 PAD C!
            1 PAD <<BIT!
            0 PAD <<BIT!
            1 PAD <<BIT!
            1 PAD <<BIT!
            PAD C@ 1011 =? ) 
    ASSERT( S"  _ " DROP PAD <<BITS
            S" | |" DROP PAD <<BITS
            S" |_|" DROP PAD <<BITS
            PAD C@ 10101111 =? ) 
    ASSERT( S"  _    " PAD ENCODE
            S" | |  |" PAD ENCODE
            S" |_|  |" PAD ENCODE
            PAD 1+ C@ 00001001 =? ) 
    ASSERT( 01010101 FIND-DIGIT NOT-FOUND =? )
    ASSERT( 10101111 FIND-DIGIT 0 =? )
    ASSERT( 10111111 FIND-DIGIT 1000 =? )

    [ DECIMAL ] 
    PAD 9 ERASE
    S"     _  _     _  _  _  _  _ " 
    S"   | _| _||_||_ |_   ||_||_|" 
    S"   ||_  _|  | _||_|  ||_| _|" 
    TEST-ENCODE
    PAD PAD 9 + OCR>ACCOUNT
    PAD 9 + 9 TYPE CR
    ASSERT( PAD 9 + C@ [CHAR] 1 =? )

    PAD 9 ERASE
    S"     _  _     _  _  _  _  _ " 
    S"   | _| _||_||_ |_   ||_||_|"
    S"    |_  _|  | _||_|  ||_| _|"
    TEST-ENCODE
    PAD PAD 9 + OCR>ACCOUNT
    PAD 9 + 9 TYPE CR
    ASSERT( PAD 9 + C@ [CHAR] ? =? )

    ASSERT( S" 457508000" DROP CHECKSUM TRUE =? )
    ASSERT( S" 457508001" DROP CHECKSUM FALSE =? )
    ASSERT( S" 45750?001" DROP ILLEGIBLE TRUE =? )
    ASSERT( S" 457504001" DROP ILLEGIBLE FALSE =? )

    S" 457508000" DROP .ACCOUNT CR
    S" 111111111" DROP .ACCOUNT CR
    S" ?34433333" DROP .ACCOUNT CR


;

TESTS
." SUCCESS"
BYE
