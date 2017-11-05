BoltJwt docker image
-
*Work in progress*

### Availables tags
0.1-dev [(BoltJwt/Dockerfile)](https://github.com/sandhaka/bolt-jwt/blob/master/BoltJwt/Dockerfile)

### Usage

#### Exposed ports:
- 80 tcp

#### Database
Only MS SQL is supported

#### Key pair
You have to use yours key pair to generate valid tokens though a docker volume

- **dev.boltjwt.pfx**: Private key
- **dev.boltjwt.crt**: The signed certificate to decode the token used by all other consumer services
- **dev.boltjwt.passphrase**: A file with the private key passphrase

At the moment the key names are fixed.

#### Start a mssql instance
```sh 
docker run -e 'ACCEPT_EULA=Y' -e 'SA_PASSWORD=<your strong password>' -p 1433:1433 -d microsoft/mssql-server-linux
```
#### Start BoltJwt
```sh
docker run -p 80:80 -e 'SQL_CONNECTION_STRING=<your sql conn string>' -v <your certs path>:/app/certs -d sandhaka/bolt-jwt:0.1-dev 
```

#### Available environment variables

##### SQL_CONNECTION_STRING
Specity the connection string to a MS SQL Server instance.

##### SQL_PASSWORD
Specity a password for the SQL 'sa' user (optional, default is 'Password&1').