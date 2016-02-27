\ gforth Tests.fs
S" Equals.fs"  REQUIRED
S" BankOcr.fs" REQUIRED

\ bit patterns
\
\  10
\ 101
\ 111
\  _     _  _     _  _  _  _  _ 
\ | |  | _| _||_||_ |_   ||_||_|
\ |_|  ||_  _|  | _||_|  ||_| _|


: TEST-ENCODING-OCR 
    ASSERT( 0 S"  _ " ENCODE-OCR
              S" | |" ENCODE-OCR
              S" |_|" ENCODE-OCR
            [ 2 BASE ! ]
            10101111 =?
            [ DECIMAL ] ) ;
                
: TEST-ENCODING-OCR-LINE 
    PAD 2 ERASE
    PAD S"  _    " ENCODE-LINE
    PAD S" | |  |" ENCODE-LINE
    PAD S" |_|  |" ENCODE-LINE
    [ 2 BASE ! ]
    ASSERT( PAD 0 + C@  10101111 =? )
    ASSERT( PAD 1 + C@  00001001 =? )
    [ DECIMAL ] ;

: TEST-FIND-DIGIT   
    [ 2 BASE ! ]
    ASSERT( 10101111 FIND-DIGIT 0000 =? )
    ASSERT( 10111111 FIND-DIGIT 1000 =? )
    [ DECIMAL ] ;

: TESTS 
    TEST-ENCODING-OCR
    TEST-ENCODING-OCR-LINE
    TEST-FIND-DIGIT
;
PAGE
TESTS
." SUCCESS"
BYE
