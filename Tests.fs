\ run tests with: gforth Tests.fs

S" BankOcr.fs" REQUIRED



: AUTOTEST
    PAD 1 ERASE
    S"  _    " PAD OCR>BITS
    S" | |  |" PAD OCR>BITS
    S" |_|  |" PAD OCR>BITS
    [ 2 BASE ! ]
    ASSERT( PAD    C@ 10101111 = )
    ASSERT( PAD 1+ C@ 00001001 = )
    [ DECIMAL ] 
    
    PAD 10 ERASE
S"  _     _  _     _  _  _  _  _ " PAD OCR>BITs
S" | |  | _| _||_||_ |_   ||_||_|" PAD OCR>BITs
S" |_|  ||_  _|  | _||_|  ||_| _|" PAD OCR>BITs
    [ 2 BASE ! ]
    ASSERT( PAD 1001 + C@ 10111011 = )
    [ DECIMAL ]
    PAD 10 >DIGITS
    ASSERT( S" 0123456789" PAD 10 COMPARE 0= )
    PAD 10 ERASE
S"  _     _  _     _  _  _     _ " PAD OCR>BITs
S" | |  | _| _|| ||_ |_   ||_||_|" PAD OCR>BITs
S" |_|  ||_  _|  | _||_|  ||_| _|" PAD OCR>BITs
    PAD 10 >DIGITS
    ASSERT( S" 0123?567?9" PAD 10 COMPARE 0= )


  ." TESTS PASSED" 
  DEPTH ?DUP IF CR .S THEN 
;

AUTOTEST CR BYE
