*Work in progress*
### Availables tags
0.1-dev [(BoltJwt/Dockerfile)](https://github.com/sandhaka/bolt-jwt/blob/master/BoltJwt/Dockerfile)
### Usage
#### Exposed ports:
- 80 tcp
#### Start a mssql instance
```sh 
docker run -e 'ACCEPT_EULA=Y' -e 'SA_PASSWORD=yourStrong(!)Password' -p 1433:1433 -d microsoft/mssql-server-linux
```
#### Start BoltJwt
*Coming soon*
#### Available environment variables
*Coming soon*