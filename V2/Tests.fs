\ run tests with: gforth Tests.fs

S" BankOcr.fs" REQUIRED

CREATE ACCOUNT 10 ALLOT


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
    PAD ACCOUNT 10 >DIGITS
    ASSERT( S" 0123456789" ACCOUNT 10 COMPARE 0= )
    PAD 10 ERASE
S"  _     _  _     _  _  _     _ " PAD OCR>BITs
S" | |  | _| _|| ||_ |_   ||_||_|" PAD OCR>BITs
S" |_|  ||_  _|  | _||_|  ||_| _|" PAD OCR>BITs
    PAD ACCOUNT 10 >DIGITS
    ASSERT( S" 0123?567?9" ACCOUNT 10 COMPARE 0= )
    ASSERT( S" 0123456789" LEGIBLE ) 
    ASSERT( S" 012345?789" LEGIBLE 0= ) 
    ASSERT( S" 0?23?5?789" NOT-FOUNDS 3 = )
    ASSERT( S" 457508000"  CHECKSUM )   
    ASSERT( S" 457518000"  CHECKSUM 0= )   
    PAD 4 4 ADD-OCR-BIT
    PAD 8 7 ADD-OCR-BIT
    PAD ACCOUNT 10 >DIGITS
    ASSERT( S" 0123456789" ACCOUNT 10 COMPARE 0= )

\ note on missing bits: on an account that doesn't checksums or is illegible, 
\ there can be only one missing bit. So we try every possibly missing bit on
\ each byte, keeping track of possible missing bits. There can be only 9 
\ alternatives because if a missing bit added makes a corrcect account number,
\ then another missing bit on the same bit can't. Changing a digits changes the 
\ checksum. Algorithm is thus:
\ if account is ILL or ERR
\   erase array Alt
\   for D from 0 to 8
\       for B from 0 to 8 
\           if byte[D] or 2^b <> byte[D] 
\               saved = byte[D]
\               byte[D] |= 2^b
\               
\               if account` is OK
\                   alt[D] = 2^b
\               endif
\               byte[D] = saved
\           endif
\       endfor
\   endfor
\   for D from 0 to 8
\       if alt[D] <> 0 
\           saved = byte[D]
\           byte[D] |= 2^b
\           convert to account 
\           print it
\       endif
\   endfor
\               
    


  ." TESTS PASSED" 
  DEPTH ?DUP IF CR .S THEN 
;

AUTOTEST CR BYE
