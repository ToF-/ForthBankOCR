4096 CONSTANT MAX-LINE
VARIABLE FILE-ID

: PROCESS-FILE ( addr u -- )
    R/O OPEN-FILE THROW FILE-ID !
    BEGIN PAD MAX-LINE FILE-ID @ READ-LINE THROW
    WHILE PAD SWAP TYPE CR
    REPEAT 
    FILE-ID @ CLOSE-FILE ;
        
: CAT
    NEXT-ARG 2DUP 0 0 D<> IF
        PROCESS-FILE
    ELSE 2DROP THEN ;
        
CAT BYE
