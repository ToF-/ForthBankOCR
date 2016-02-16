\ BankOcr

CREATE OCR-BUFFER 9 3 * 3 * ALLOT

: BAR? 32 <> 1 AND ;

: OCR-ADDRESS ( n i -- addr  char i or nth ocr digit ) 
    ?DUP 0= IF 1 ELSE
    1- 3 /MOD 1+ 27 * SWAP + THEN 
    SWAP 3 * + OCR-BUFFER + ;
