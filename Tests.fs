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

: TEST-PROCESS-LINE
    ACCOUNT 9 ERASE
    S"     _  _     _  _  _  _  _ " PROCESS-LINE
    S"   | _| _||_||_ |_   ||_||_|" PROCESS-LINE
    S"   ||_  _|  | _||_|  ||_| _|" PROCESS-LINE
    S" " PROCESS-LINE 
    ASSERT( ACCOUNT 9 S" 123456789" COMPARE 0 =? ) ;

: TEST-ILLEGIBLE 
    ASSERT( S" 123456?89" ILLEGIBLE? TRUE  =? ) 
    ASSERT( S" 123456789" ILLEGIBLE? FALSE =? ) ;

: TEST-CHECKSUM
    ASSERT( S" 000000051" CHECKSUM? TRUE  =? ) 
    ASSERT( S" 000000061" CHECKSUM? FALSE =? ) ;

: TEST-ADD-MISSING-BIT
    [ 2 BASE ! ]
    ASSERT( 00110010 0   ADD-MISSING-BIT 00110011 =? ) 
    ASSERT( 00110010 10  ADD-MISSING-BIT 00110110 =? ) 
    ASSERT( 00110010 111 ADD-MISSING-BIT 10110010 =? )
    [ DECIMAL ] ;

: TEST-FIND-ALTERNATIVE 
    S"  _  _  _  _  _  _  _  _    " PROCESS-LINE
    S" | || || || || || || ||_   |" PROCESS-LINE
    S" |_ |_||_||_||_||_||_| _|  |" PROCESS-LINE
    S" " PROCESS-LINE
    ASSERT( S" 000000051" ALT-ACCOUNT 9 COMPARE 0 =? )
;


: VISUAL-TESTS
    S" 0100?0000" .ACCOUNT
    S" 010040500" .ACCOUNT
    S" input.txt" PROCESS-FILE ;

: TESTS 
    TEST-ENCODING-OCR
    TEST-ENCODING-OCR-LINE
    TEST-FIND-DIGIT
    TEST-PROCESS-LINE
    TEST-ILLEGIBLE
    TEST-CHECKSUM
    TEST-ADD-MISSING-BIT
    TEST-FIND-ALTERNATIVE
    \ VISUAL-TESTS 
;
PAGE
TESTS
." SUCCESS"
BYE
