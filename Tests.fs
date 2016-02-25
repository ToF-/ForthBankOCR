\ gforth Tests.fs

S" BankOcr.fs" REQUIRED

\ bit patterns
\
\  10
\ 101
\ 111
\  _     _  _     _  _  _  _  _ 
\ | |  | _| _||_||_ |_   ||_||_|
\ |_|  ||_  _|  | _||_|  ||_| _|

: TESTS
    ASSERT( BL BAR? 0 = )
    ASSERT( [CHAR] _ BAR? 1 = ) 
    ASSERT( [CHAR] | BAR? 1 = ) 
    
    ASSERT( 0 PAD C!
            1 PAD <<BIT!
            0 PAD <<BIT!
            1 PAD <<BIT!  
            PAD C@ 5 = )

    ASSERT( 0  BYTE# 0 = ) 
    ASSERT( 23 BYTE# 7 = ) 
    PAD 10 ERASE
    ASSERT( 23 1 PAD OCR-BIT! 
            PAD 7 + C@ 1 = )
    ASSERT( 4 1 PAD OCR-BIT!
            5 1 PAD OCR-BIT!
            PAD 1 + C@ 3 = )
PAD 10 ERASE 
S"  _     _  _     _  _  _  _  _ " PAD OCR-BITS!
PAD 10 DUMP
    ASSERT( PAD 0 + C@ 2 = )
    ASSERT( PAD 1 + C@ 0 = )
    ASSERT( PAD 2 + C@ 2 = )

ASSERT( S" _ " DROP 0 #BAR? 1 = )
ASSERT( S" _ " DROP 1 #BAR? 0 = )


;

TESTS
." SUCCESS"
BYE
