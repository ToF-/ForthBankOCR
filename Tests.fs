\ run tests with: gforth Tests.fs

S" BankOcr.fs" REQUIRED

CHAR _ CONSTANT BAR
CHAR | CONSTANT PIPE

: LINE ROT C, SWAP C, C, ;
:  _   BL BAR BL     LINE ;

: |.|  PIPE BL PIPE  LINE ;

: |_   PIPE BAR BL   LINE ;

:  _|  BL  BAR  PIPE LINE ;

: |    BL  BL  PIPE  LINE ;

: |_|  PIPE BAR PIPE LINE ;

: ...  BL  BL  BL    LINE ;


: CHECK-ADDRESS ( n i p -- f|t )
    OCR + -ROT ADDRESS = ;

CREATE TEST
 _  ...  _   _  ...  _   _  _   _   _
|.|   |  _|  _| |_| |_  |_   | |_| |_|
|_|   | |_   _|   |  _| |_|  | |_|  _|

TEST      30 TYPE CR
TEST 30 + 30 TYPE CR
TEST 60 + 30 TYPE CR

CREATE ILL-TEST
 _  ...  _   _  ...  _   _  _   _   _
|.|   |  _|  _| |_| |_  |_   | |_| |_|
|_|   | |_   _|   |  _| |_|  | |_|  _|



10 ACCOUNT-SIZE !

: AUTOTEST
    ASSERT( BL   BAR? 0 = )
    ASSERT( PIPE BAR? 1 = )
    ASSERT( BAR  BAR? 1 = )

    ASSERT( 0 0  1 CHECK-ADDRESS )
    ASSERT( 0 1 30 CHECK-ADDRESS )
    ASSERT( 0 2 31 CHECK-ADDRESS )
    ASSERT( 0 3 32 CHECK-ADDRESS )
    ASSERT( 0 4 60 CHECK-ADDRESS )
    ASSERT( 0 5 61 CHECK-ADDRESS )
    ASSERT( 0 6 62 CHECK-ADDRESS )
    ASSERT( 1 0  4 CHECK-ADDRESS )
    ASSERT( 1 1 33 CHECK-ADDRESS )
    ASSERT( 1 2 34 CHECK-ADDRESS )
    ASSERT( 9 6 89 CHECK-ADDRESS )


    TEST OCR 90 CMOVE
    ASSERT( 0 [ 2 BASE ! ] OCR>PATTERN 1101111 = [ DECIMAL ] )
    ASSERT( 1 [ 2 BASE ! ] OCR>PATTERN 0001001 = [ DECIMAL ] )
    ASSERT( 5 [ 2 BASE ! ] OCR>PATTERN 1110011 = [ DECIMAL ] )

    ASSERT( [ 2 BASE ! ] 1111111 [ DECIMAL ] PATTERN>DIGIT [CHAR] 8 = )
    
    OCR>ACCOUNT .ACCOUNT SPACE

    ILL-TEST OCR 90 CMOVE
    ASSERT( BL 0 0 ADDRESS C! OCR>ACCOUNT ACCOUNT C@ [CHAR] ? = ) 
    ASSERT( ILLEGIBLE? )
    BL 1 3 ADDRESS C!
    
    OCR>ACCOUNT .ACCOUNT SPACE


    ." TESTS PASSED"
    DEPTH ?DUP IF CR .S THEN 
;

AUTOTEST CR

BYE
