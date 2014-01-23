mycroft
=======

After cloning the repository, run a clean build of the application before
running it. The `settings.xml` file in the output directory (probably
`Mycroft\bin\Debug`) needs to have the thumbprint of a certificate that has
been installed to the Root Certificate Authority store.

To run the server without TLS, specify `--no-tls` as a command line argument.
To manually specify the certificate to use, use `--cert` followed by the path
to the `.pfx` file you'd like to use for the server certificate.
