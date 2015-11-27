Passpad
-------

A simple *(portable, single-executable)* editor for encrypted textfiles

Supports:

 - AES-256
 - Blowfish
 - Twofish
 - CAST-128
 - DES
 - Triple DES

##[Download latest release](https://github.com/Mikescher/Passpad/releases)
##[Homepage](http://www.mikescher.de/programs/view/Passpad)

![MainWindow](https://raw.githubusercontent.com/Mikescher/Passpad/master/README-DATA/main.png)

The encrypted data is encoded as base-64 and saved in a simple text-file

![MainWindow](https://raw.githubusercontent.com/Mikescher/Passpad/master/README-DATA/fileformat.png)

You can optionally specify a password hint for your encrypted files:

![MainWindow](https://raw.githubusercontent.com/Mikescher/Passpad/master/README-DATA/hint.png)

#File format

The file format contains two parts. The plaintext hint and the encrypted data (together with the used encryption algorithm).

The encrypted data is encoded in base-64. 

 - The first 32 byte block is the SHA-256 checksum of the unencrypted data (for verification).
 - After that the next bytes are the used IV (length dependent on the algorithm)
 - Then comes the raw encrypted data (encrypted with the specified algorithm)

As the Key derivation function we use 40,000 rounds of PBKDF2 over the UTF-16 encoded password.

For the precise implementation details please feel free to look at the source code.
