CREATE BUFFER 500 ALLOT

: ECHO
    BEGIN
    BUFFER 500 STDIN READ-LINE THROW
    WHILE
        BUFFER SWAP TYPE CR
    REPEAT ;

ECHO
BYE
