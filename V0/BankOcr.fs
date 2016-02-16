\ BankOCR.fs  Solving the Bank OCR kata

: BINARY 2 BASE ! ;

    32 CONSTANT SP
CHAR _ CONSTANT HBAR
CHAR | CONSTANT VBAR
     3 CONSTANT OCR-WIDTH 
     3 CONSTANT OCR-HEIGHT
     9 CONSTANT ACCOUNT-SIZE
CHAR ? CONSTANT NOT-FOUND

: OCR-LINES OCR-WIDTH ACCOUNT-SIZE * * ;
: OCR-COLS  OCR-WIDTH * ;

OCR-WIDTH ACCOUNT-SIZE * CONSTANT OCR-LINE-SIZE


CREATE OCR-BUFFER OCR-WIDTH OCR-HEIGHT ACCOUNT-SIZE * * ALLOT 

\ table of binary patterns for digits 0..9
\ each OCR digit is coded on 9 chars in [space,|,_]
\  _     _  _     _  _  _  _  _ 
\ | |  | _| _||_||_ |_   ||_||_|
\ |_|  ||_  _|  | _||_|  ||_| _|
\
\ a bar in the middle column is always horizontal
\ a bar in column 0 or 2 is always vertical 
\ therefore each char can be coded on 1 bit.
\ the chars in column 0 and 2,row 0 are always space
\ therefore each OCR digit can be coded on 8 bits 

\ bits#   0   1   2   3   4   5   6   7   8   9  
\  76    10  00  10  01  00  10  10  10  10  10
\ 543   101 001 001 011 111 110 110 001 111 111
\ 210   111 001 111 011 001 011 111 001 111 011

: CTABLE CREATE DOES> + C@ ; 

CTABLE DIGIT>PATTERN
2 BASE !
10101111 C, 00001001 C, 10011110 C, 10011011 C, 00111001 C,
10110011 C, 10110111 C, 10001001 C, 10111111 C, 10111011 C, 
DECIMAL

CREATE ACCOUNT ACCOUNT-SIZE ALLOT 
ACCOUNT ACCOUNT-SIZE ERASE

: BAR?   ( c -- f|t  leaves True is the char is a bar, False is a space ) 
    32 <> 1 AND ;

: BIT<<   ( byte,bit -- byte'  shift-left the byte and store a new bit into the right position ) 
    SWAP 1 LSHIFT OR ;

: BIT>>   ( byte -- byte',bit extract the lower bit and shift-right the byte )
    DUP 1 RSHIFT SWAP 1 AND ;

: OCR-ADDRESS ( n,m -- addr  address of the OCR char m of nth ocr digit )
    OCR-HEIGHT /MOD OCR-LINES + SWAP OCR-COLS + OCR-BUFFER + ;

: OCR>PATTERN ( n -- byte  converts one OCR digit into its binary pattern ) 
    0  9 0 DO OVER I OCR-ADDRESS C@ BAR? BIT<< LOOP NIP ;

: BIT>OCR-CHAR ( b,n -- c  converts bit #n into OCR char SP,| or _ )
    2 + 3 MOD IF VBAR ELSE HBAR THEN SWAP 0= IF DROP SP THEN ;

: PATTERN>OCR ( byte,n -- converts a binary pattern into the n OCR digit)
    9 0 DO
        SWAP BIT>>
        I BIT>OCR-CHAR
        ROT DUP 8 I - OCR-ADDRESS
        ROT SWAP C! 
    LOOP 2DROP ; 

: DIGIT>CHAR ( n -- c  convert digit [0..9] to O..9, and 15 to ? )
    [ 2 BASE ! ] 110000 OR [ DECIMAL ] ;

: CHAR>DIGIT ( n -- c  convert char [0..9] to 0..9 and ? to 15 )
    [ 2 BASE ! ] 001111 AND [ DECIMAL ] ;

: PATTERN>DIGIT  ( b -- c|?  search the digits table for b. ? if not found) 
    NOT-FOUND   10 0 DO
        OVER I DIGIT>PATTERN = IF  DROP I DIGIT>CHAR LEAVE THEN  
    LOOP NIP ; 

: OCR>ACCOUNT ( --   converts OCR data to account number )
    9 0 DO   
        I OCR>PATTERN PATTERN>DIGIT 
        ACCOUNT I + C! 
    LOOP ;

: ACCOUNT>OCR ( -- converts account number to OCR data )
    9 0 DO
        ACCOUNT I + C@ CHAR>DIGIT DIGIT>PATTERN
        I PATTERN>OCR LOOP ;

: SUMCHECK-ERROR? ( -- f|t  checks that [d0*9+d1*8+..+d8] is a multiple of 11 )
    0   9 0 DO 
        ACCOUNT I + C@ 
        CHAR>DIGIT 9 I - * + 
    LOOP   11 MOD ;

: ILLEGIBLE? ( -- f|t    checks the account for illegible digit )
    FALSE   9 0 DO
        ACCOUNT I + C@ 
        NOT-FOUND = IF DROP TRUE LEAVE THEN 
    LOOP ;

: PRINT-ACCOUNT \ prints the account with suffix if illegal or error
    ACCOUNT 9 TYPE 
    ILLEGIBLE?      IF ."  ILL" ELSE 
    SUMCHECK-ERROR? IF ."  ERR" THEN THEN ;

: PRINT-OCR \ prints the OCR 3 line text
    3 0 DO 0 I 3 * OCR-ADDRESS 
           OCR-LINE-SIZE TYPE CR LOOP ;
    

