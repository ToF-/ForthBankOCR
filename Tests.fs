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
                
: TESTS 
    TEST_ENCODING_OCR
;

TESTS
." SUCCESS"
BYE
