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

: TEST-BYTE>DIGIT   
    [ 2 BASE ! ]
    ASSERT( 10101111 BYTE>DIGIT 0000 =? )
    ASSERT( 10111111 BYTE>DIGIT 1000 =? )
    [ DECIMAL ] ;

: TEST-PROCESS-LINE
    S"     _  _     _  _  _  _  _ " PROCESS-LINE 
    S"   | _| _||_||_ |_   ||_||_|" PROCESS-LINE
    S"   ||_  _|  | _||_|  ||_| _|" PROCESS-LINE
    S" " PROCESS-LINE 
    ASSERT( ACCOUNTS 9 S" 123456789" COMPARE 0 =? ) ;

: TEST-ILLEGIBLE 
    ASSERT( S" 123456?89" ILLEGIBLE? TRUE  =? ) 
    ASSERT( S" 123456789" ILLEGIBLE? FALSE =? ) ;

: TEST-CHECKSUM
    ASSERT( S" 000000051" CHECKSUM? TRUE  =? ) 
    ASSERT( S" 000000061" CHECKSUM? FALSE =? ) ;

: TEST-SET-BIT
    [ 2 BASE ! ]
    ASSERT( 00  00110010 SET-BIT 00110011 =? ) 
    ASSERT( 10  00110010 SET-BIT 00110110 =? ) 
    ASSERT( 111 00110010 SET-BIT 10110010 =? )
    [ DECIMAL ] ;


: TEST-VALID
    ASSERT( S" 0000000?3" VALID? 0 =? ) 
    ASSERT( S" 000000033" VALID? 0 =? ) ;

: TEST-FIND-ALTERNATIVE 
    S"     _  _  _  _  _  _  _    " PROCESS-LINE
    S" | || || || || || || ||_   |" PROCESS-LINE
    S" |_||_||_||_||_||_||_| _|  |" PROCESS-LINE
    S" " PROCESS-LINE
    ASSERT( ACCOUNT# @ 1 =? )
    ASSERT( S" 000000051" ACCOUNTS 9 COMPARE 0 =? )
    S"  _     _  _  _  _  _  _    " PROCESS-LINE
    S" | || || || || || || ||_   |" PROCESS-LINE
    S" |_||_||_||_||_||_||_| _|  |" PROCESS-LINE
    S" " PROCESS-LINE
    ASSERT( ACCOUNT# @ 1 =? )
    ASSERT( S" 000000051" ACCOUNTS 9 COMPARE 0 =? )
;

: TEST-SEVERAL-ALTERNATIVES 
    S"     _  _  _  _  _  _     _ " PROCESS-LINE
    S" |_||_|| || ||_   |  |  ||_ " PROCESS-LINE
    S"   | _||_||_||_|  |  |  | _|" PROCESS-LINE
    S" " PROCESS-LINE
    ASSERT( ACCOUNT# @ 2 =? )
    ACCOUNT 9 .ACCOUNT
;
: TEST-SEVERAL-ACCOUNTS
S"  _     _  _  _  _  _  _  _ " PROCESS-LINE
S"  _||_||_ |_||_| _||_||_ |_ " PROCESS-LINE
S"  _|  | _||_||_||_ |_||_| _|" PROCESS-LINE
S" " PROCESS-LINE
S"     _     _  _  _  _  _  _ " PROCESS-LINE
S"   ||_|  ||_  _||_| _||_ | |" PROCESS-LINE
S"   | _|  | _| _| _||_ |_||_|" PROCESS-LINE
S" " PROCESS-LINE
S"  _     _  _  _  _     _  _ " PROCESS-LINE
S" |_ |_|  ||_||_  _|  |  ||_|" PROCESS-LINE
S" |_|  |  | _||_||_   |  | _|" PROCESS-LINE
S" " PROCESS-LINE
S"  _  _  _  _     _  _  _  _ " PROCESS-LINE
S"  _||_||_  _|  | _||_||_| _|" PROCESS-LINE
S" |_ |_| _||_   ||_  _||_||_ " PROCESS-LINE
S" " PROCESS-LINE
;
    

: VISUAL-TESTS 
     S" input.txt" PROCESS-FILE ;

: TESTS 
\    TEST-ENCODING-OCR
\    TEST-ENCODING-OCR-LINE
\    TEST-BYTE>DIGIT
\    TEST-PROCESS-LINE
\    TEST-ILLEGIBLE
\    TEST-CHECKSUM
\    TEST-SET-BIT
\    TEST-ADD-ALTERNATIVE
\    TEST-VALID
\    TEST-FIND-ALTERNATIVE
\    TEST-SEVERAL-ALTERNATIVES
\    TEST-SEVERAL-ACCOUNTS
    VISUAL-TESTS 
;
PAGE
TESTS
." SUCCESS"
.S
BYE
