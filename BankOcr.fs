3 CONSTANT WIDTH 
3 CONSTANT HEIGHT
9 CONSTANT ACCOUNTSIZE
WIDTH ACCOUNTSIZE * CONSTANT LINESIZE

CREATE OCRBITS 81 ALLOT 

( TEST OCR PATTERNS )
\  0         1         2         3         4         5         6         7         8
\  01234567890123456789012345678901234567890123456789012345678901234567890123456789012
S"  _     _  _     _  _  _  _ | |  | _| _||_||_ |_   ||_||_|  ||_  _|  | _||_|  ||_|"
OCRBITS SWAP CMOVE

2 BASE !
CREATE DIGITS 010101111 C, 00001001 C, 010011110 C, 10011011 C, 00111001 C,
              010110011 C, 10110111 C, 010001001 C, 10111111 C, 10111011 C, 
DECIMAL

CREATE ACCOUNT ACCOUNTSIZE ALLOT 
ACCOUNT ACCOUNTSIZE ERASE

: BAR?   ( c -- f|t ) 32 <> 1 AND ;
: 2*OR   ( c,b -- c*2 or b ) SWAP 2* OR ;
: BITPOS ( n,p -- p ) HEIGHT /MOD LINESIZE * + SWAP WIDTH * + ;
: PATTERN ( n -- byte) 0 ACCOUNTSIZE 0 DO OVER I BITPOS OCRBITS + C@ BAR? 2*OR LOOP SWAP DROP ;
: DIGIT  ( byte -- n|-1) -1 10 0 DO OVER DIGITS I + C@ = IF DROP I THEN LOOP SWAP DROP ; 
: ACCOUNTDIGIT ( i -- n ) DUP PATTERN DIGIT SWAP ACCOUNT + ! ;

: CHECKS
    ASSERT( 32  BAR? 0 = ) 
    ASSERT( 95  BAR? 1 = ) 
    ASSERT( 124 BAR? 1 = )
    [ 2 BASE ! ]
    ASSERT( 100101 1 2*OR 1001011 = )
    ASSERT( 111001 0 2*OR 1110010 = )
    [ DECIMAL ]
    ASSERT( 0 0 BITPOS 0  = )
    ASSERT( 0 2 BITPOS 2  = )
    ASSERT( 0 3 BITPOS 27 = )
    ASSERT( 0 8 BITPOS 56 = )
    ASSERT( 8 6 BITPOS 78 = )
    ASSERT( 8 7 BITPOS 79 = )
    ASSERT( 8 8 BITPOS 80 = )
    ASSERT( 0 PATTERN [ 2 BASE ! ] 010101111 = [ DECIMAL ] )
    ASSERT( 1 PATTERN [ 2 BASE ! ] 000001001 = [ DECIMAL ] )
    ASSERT( 8 PATTERN [ 2 BASE ! ] 010111111 = [ DECIMAL ] )
    ASSERT( [ 2 BASE ! ] 011111111 [ DECIMAL ] DIGIT -1 =  )
    ASSERT( [ 2 BASE ! ] 000111001 [ DECIMAL ] DIGIT 4 =  )
    ASSERT( 0 ACCOUNTDIGIT ACCOUNT 0 + C@ 0 = ) 
    ASSERT( 1 ACCOUNTDIGIT ACCOUNT 1 + C@ 1 = ) 
    ASSERT( 2 ACCOUNTDIGIT ACCOUNT 2 + C@ 2 = ) 
    ASSERT( 3 ACCOUNTDIGIT ACCOUNT 3 + C@ 3 = ) 
    ASSERT( 4 ACCOUNTDIGIT ACCOUNT 4 + C@ 4 = ) 
    ASSERT( 5 ACCOUNTDIGIT ACCOUNT 5 + C@ 5 = ) 
    ASSERT( 6 ACCOUNTDIGIT ACCOUNT 6 + C@ 6 = ) 
    ASSERT( 7 ACCOUNTDIGIT ACCOUNT 7 + C@ 7 = ) 
    ASSERT( 8 ACCOUNTDIGIT ACCOUNT 8 + C@ 8 = ) 
    \ altering an ocr digit
    OCRBITS 1+ 32 SWAP C!
    ASSERT( 0 ACCOUNTDIGIT ACCOUNT 0 + C@ 255 = ) 
    
; 
CHECKS
CR .S CR
BYE


\ CREATE OCRBITS 9 3 * 3 * ALLOT 
\ : BAR? ( c -- 0|1 ) 32 <> 1 AND ;
\ : >BIN ( b1,b2,b2 -- binary pattern ) SWAP 2* OR SWAP 2* 2* OR ;
\ : >BYTE ( b1,b2,b3 -- byte ) ROT 2* 2* 2* ROT OR 2* 2* 2* OR  ; 
\ 
\ : 2*OR ( n b -- n*2 or b ) SWAP 2* OR ;
\ : OCRBIT ( r c -- b ) SWAP 27 * + OCRBITS C@ BAR? ;  
\ 
\ 
 
\ 
\ CREATE DIGITS 9 ALLOT 









