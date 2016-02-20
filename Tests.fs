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
    PAD + -ROT ADDRESS = ;

CREATE TEST
 _  ...  _   _  ...  _   _  _   _   _
|.|   |  _|  _| |_| |_  |_   | |_| |_|
|_|   | |_   _|   |  _| |_|  | |_|  _|


10 ACCOUNT-SIZE !

CREATE VALID-TEST
...  _  _   _   _   _   _   _   _ 
|_| |_   | |_  |.| |_| |.| |.| |.| 
  |  _|  |  _| |_| |_| |_| |_| |_|
 
CREATE ERROR-TEST
...  _  _   _  ...  _   _   _   _ 
|_| |_   | |_    | |_| |.| |.| |.| 
  |  _|  |  _|   | |_| |_| |_| |_|

CREATE ILLEGIBLE-TEST
...  _   _   _   _   _   _   _   _ 
|_| |_  ... |_  |.| |_| |.| |.| |.| 
  |  _|   |  _| |_| |_| |_| |_| |_|

 


: AUTOTEST
    \ detecting a vertical or horizontal bar
    ASSERT( BL   BAR? 0 = )
    ASSERT( PIPE BAR? 1 = )
    ASSERT( BAR  BAR? 1 = )

    \ position of an OCR bar (10 x 3 char digits per line) : 
    \     0123
    \    ..... 
    \  0 . 0  
    \ 30 .123
    \ 60 .456
    
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

    TEST PAD 90 CMOVE
    \ converts OCR digits into patterns
    ASSERT( 0 [ 2 BASE ! ] OCR>PATTERN 1101111 = [ DECIMAL ] )
    ASSERT( 1 [ 2 BASE ! ] OCR>PATTERN 0001001 = [ DECIMAL ] )
    ASSERT( 5 [ 2 BASE ! ] OCR>PATTERN 1110011 = [ DECIMAL ] )
    ASSERT( 9 [ 2 BASE ! ] OCR>PATTERN 1111011 = [ DECIMAL ] )

    ASSERT( [ 2 BASE ! ] 1111111 [ DECIMAL ] PATTERN>DIGIT [CHAR] 8 = )
    
    \ converts OCR digits into account number
    OCR>ACCOUNT .ACCOUNT SPACE

    \ a missing bar in one digit makes the account illegible
    ASSERT( BL 0 0 ADDRESS C!  
            OCR>ACCOUNT ACCOUNT C@ [CHAR] ? = ) 
    ASSERT( ILLEGIBLE? )
    
    BL 1 3 ADDRESS C!
    
    OCR>ACCOUNT .ACCOUNT SPACE

    9 ACCOUNT-SIZE !
    S" 457508000" ACCOUNT SWAP CMOVE
    ASSERT( ERROR? 0= )
    S" 457508001" ACCOUNT SWAP CMOVE
    ASSERT( ERROR? )

    9 ACCOUNT-SIZE !
    CR
    VALID-TEST PAD 81 CMOVE 
    PROCESS-OCR 
    ERROR-TEST PAD 81 CMOVE 
    PROCESS-OCR 
    ILLEGIBLE-TEST PAD 81 CMOVE
    PROCESS-OCR

    S" sample.txt" PROCESS-FILE

    ." TESTS PASSED"
    DEPTH ?DUP IF CR .S THEN 
;

AUTOTEST CR

BYE
