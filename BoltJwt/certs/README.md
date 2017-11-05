How to create a signed certificate to use during development phase:
---

#### Created CA root:

**myCA.key** is the private key of the root certificate

**myCA.pem** is the root certificate

#### Created certificate for the development:

**dev.boltjwt.key** is the private key --> using in dev

**dev.boltjwt.csr** is the CSR file (cert signin request)

**dev.boltjwt.ext** is the configuration file

**dev.boltjwt.crt** is the signed certificate --> using in dev

**dev.boltjwt.pfx** this a pfx format certificate to simplify the access to the private key though the code

>*To create the last pfx certificate:*
>```sh
>openssl pkcs12 -export -in dev.boltjwt.com.crt -inkey dev.boltjwt.com.key -out dev.boltjwt.pfx
>``` 
>Use it in the code:
>```shaderlab
>var prvtKeyPassphrase = "your_passphrase";
>var privateKey = new X509Certificate2("certs/dev.boltjwt.pfx", prvtKeyPassphrase).GetRSAPrivateKey();
>```

[**Detailed instructions to create certificates**](https://deliciousbrains.com/ssl-certificate-authority-for-local-https-development/)