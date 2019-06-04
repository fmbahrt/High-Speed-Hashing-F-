CC=fsharpc

library:
	$(CC) -a hashtable.fs

clean:
	rm -f hashtable.dll
