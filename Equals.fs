\ Equals.fs

: =? ( n,m -- f|t with message if n <> m )
    2DUP <> IF ." EXPECTED " . ." BUT WAS " . CR FALSE ELSE = THEN ;

