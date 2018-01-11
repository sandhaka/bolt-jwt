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

[Link to detailed instructions to create certificates](https://deliciousbrains.com/ssl-certificate-authority-for-local-https-development/)

#### Example:
```sh
# Generate certificates
openssl genrsa -des3 -passout "pass:x" -out myCA.key 2048

openssl req -passin "pass:x" -x509 -new -nodes -key myCA.key -sha256 -days 1825 -out myCA.pem -subj "/C=UK/ST=Warwickshire/L=Leamington/O=OrgName/OU=IT Department/CN=example.com"

openssl genrsa -out dev.boltjwt.key 2048

openssl req -new -key dev.boltjwt.key -out dev.boltjwt.csr -subj "/C=UK/ST=Warwickshire/L=Leamington/O=OrgName/OU=IT Department/CN=example.com"

printf "authorityKeyIdentifier=keyid,issuer\n\rbasicConstraints=CA:FALSE\n\rkeyUsage = digitalSignature, nonRepudiation, keyEncipherment, dataEncipherment\n\rsubjectAltName = @alt_names\r\n\n\r[alt_names]\r\nDNS.1 = dev.mergebot.com\n\rDNS.2 = dev.mergebot.com.192.168.1.19.xip.io\n\r" > dev.boltjwt.ext

openssl x509 -req -passin "pass:x" -in dev.boltjwt.csr -CA myCA.pem -CAkey myCA.key -CAcreateserial -out dev.boltjwt.crt -days 1825 -sha256 -extfile dev.boltjwt.ext

openssl pkcs12 -export -passin "pass:x" -passout "pass:x" -in dev.boltjwt.crt -inkey dev.boltjwt.key -out dev.boltjwt.pfx

echo "pass:x" > dev.boltjwt.passphrase 
```