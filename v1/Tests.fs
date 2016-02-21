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
    \  0 .012
    \ 30 .345
    \ 60 .678
    
    ASSERT( 0 OCR-BIT-POS 0 = )
    ASSERT( 1 OCR-BIT-POS 1 = )
    ASSERT( 2 OCR-BIT-POS 2 = )
    ASSERT( 3 OCR-BIT-POS 30 = )
    ASSERT( 4 OCR-BIT-POS 31 = )
    ASSERT( 5 OCR-BIT-POS 32 = )
    ASSERT( 8 OCR-BIT-POS 62 = )

    ASSERT( 0 OCR-POS 0  = )
    ASSERT( 1 OCR-POS 3  = )
    ASSERT( 4 OCR-POS 12 = ) 
    TEST PAD 90 CMOVE
    \ converts OCR digits into patterns
    ASSERT( PAD WIDTH 0 * + [ 2 BASE ! ] OCR>PATTERN 10101111 = [ DECIMAL ] )
    ASSERT( PAD WIDTH 1 * + [ 2 BASE ! ] OCR>PATTERN 00001001 = [ DECIMAL ] )
    ASSERT( PAD WIDTH 5 * + [ 2 BASE ! ] OCR>PATTERN 10110011 = [ DECIMAL ] )
    ASSERT( PAD WIDTH 9 * + [ 2 BASE ! ] OCR>PATTERN 10111011 = [ DECIMAL ] )

    ASSERT( [ 2 BASE ! ] 10111111 [ DECIMAL ] PATTERN>DIGIT [CHAR] 8 = )

    \ a missing bar in one digit makes the account illegible
    ASSERT( BL PAD 1+ C! PAD OCR>ACCOUNT ACCOUNT C@ [CHAR] ? = ) 
    ASSERT( ILLEGIBLE? )
    
    PAD OCR>ACCOUNT .ACCOUNT SPACE
    9 ACCOUNT-SIZE !
    S" 457508000" ACCOUNT SWAP CMOVE
    ASSERT( ERROR? 0= )
    S" 457508001" ACCOUNT SWAP CMOVE
    ASSERT( ERROR? )

    9 ACCOUNT-SIZE !
    CR
    VALID-TEST PAD 81 CMOVE 
    PAD PROCESS-OCR 
    ERROR-TEST PAD 81 CMOVE 
    PAD PROCESS-OCR 
    ILLEGIBLE-TEST PAD 81 CMOVE
    PAD PROCESS-OCR

    S" sample.txt" PROCESS-FILE

    ." TESTS PASSED"
    DEPTH ?DUP IF CR .S THEN 
;

AUTOTEST CR

BYE
