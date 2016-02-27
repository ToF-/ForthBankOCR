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


: TEST_ENCODING_OCR 
    ASSERT( 0 S"  _ " ENCODE
              S" | |" ENCODE
              S" |_|" ENCODE
            [ 2 BASE ! ]
            10101111 =?
            [ DECIMAL ] ) ;
                
: TEST_ENCODING_OCR_LINE 
    PAD 2 ERASE
    PAD S"  _    " ENCODE-LINE
    PAD S" | |  |" ENCODE-LINE
    PAD S" |_|  |" ENCODE-LINE
    [ 2 BASE ! ]
    ASSERT( PAD 0 + C@  10101111 =? )
    ASSERT( PAD 1 + C@  00001001 =? )
    [ DECIMAL ] ;

: TESTS 
    TEST_ENCODING_OCR
    TEST_ENCODING_OCR_LINE
;
PAGE
TESTS
." SUCCESS"
BYE
