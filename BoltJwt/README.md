## BoltJwt docker image

### Availables tags
1.7.1, dev

[(BoltJwt/Dockerfile)](https://github.com/sandhaka/bolt-jwt/blob/master/BoltJwt/Dockerfile)

### Usage

#### Exposed ports:
- 443 tcp

#### Database
Only MS SQL is supported

#### Security
You have to use a certificate to generate valid tokens:

- The signed certificate to decode the token used by all other consumer services
- A file with the private key passphrase

Mount these files to '/app/certs' container folder.

#### Start a mssql instance
```sh 
docker run -e 'ACCEPT_EULA=Y' -e 'SA_PASSWORD=<your strong password>' -p 1433:1433 -d microsoft/mssql-server-linux
```
#### Start BoltJwt
```sh
docker run -p 443:443 -e 'SQL_CONNECTION_STRING=<your sql conn string>' -v <your certs path>:/app/certs -d sandhaka/bolt-jwt:dev 
```

#### Available environment variables

##### SQL_CONNECTION_STRING
Connection string of the MS SQL Server instance.
##### CERT_NAME
Path of the certificate
##### CERT_PWD_NAME
Path of the file with the passphrase

#### Api
```text
/api/token
```
Retrieve the token

```text
/api/tokenrenew
```
Renew the token

Example:

```sh 
curl -H "Content-Type: application/json" -X POST -d "{'username':'root','password':'root'}" -v http://127.0.0.1:5000/api/token
```
Output:
```sh
* timeout on name lookup is not supported
*   Trying 127.0.0.1...
* TCP_NODELAY set
* Connected to 127.0.0.1 (127.0.0.1) port 5000 (#0)
> POST /api/token HTTP/1.1
> Host: 127.0.0.1:5000
> User-Agent: curl/7.50.3
> Accept: */*
> Content-Type: application/json
> Content-Length: 37
>
* upload completely sent off: 37 out of 37 bytes
< HTTP/1.1 200 OK
< Date: Mon, 06 Nov 2017 17:30:10 GMT
< Content-Type: application/json
< Server: Kestrel
< Transfer-Encoding: chunked
<
{
  "access_token": "eyJhbGciOiJSUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJCb2x0Snd0Iiwic3ViIjoicm9vdCIsImF1ZCI6WyJCb2x0Snd0QXVkIiwiQm9sdEp3dEF1ZCJdLCJpYXQiOjE1MDk5ODk0MTAsImp0aSI6ImViMWRkNjEwLWUzNGItNDRiMS05OTBhLTcyZGFiZDU2OTE2YyIsImh0dHA6Ly9zY2hlbWFzLnhtbHNvYXAub3JnL3dzLzIwMDUvMDUvaWRlbnRpdHkvY2xhaW1zL25hbWUiOiJyb290IiwiaXNSb290IjoidHJ1ZSIsInVzZXJJZCI6IjEiLCJ1c2VybmFtZSI6InJvb3QiLCJhdXRob3JpemF0aW9ucyI6IltdIiwibmJmIjoxNTA5OTg5NDEwLCJleHAiOjE1MTA1OTQyMTB9.b7FX_C6b-KgJLOYbJh-bZFHD9hgrYG8DVNUJtv4ebNPVwLa1eSva1FaevkhiJZ1pvpF8PyEDpzhsSjrvwcOinVPXBaYJQE6ylpI5o8_fMSXVeXpYk2jvp4GGYUqg36G35fJgfIyyGbTPUC24o6pKfgxHAgf5jWawPFLfVpk8HHAerz8xFbBUP4USQUvJC6yvDhL_GzsAChW3bVNEXvESPDVNUHDZyhvW_qx3r0UvTQjjIjDTE6MWp-FoT3N5QhptG4G9oCXLxFDG7IFF-UVRPcOb4TGP3Av4Fx4Zxq6Rlm2m3MlLpEorjsHHFAPV8O3sNe40tQjwd0shmu7uqQ_idQ",
  "expires_in": 604800
}* Curl_http_done: called premature == 0
* Connection #0 to host 127.0.0.1 left intact
```
On windows system the above command become:
```sh
curl -H "Content-Type: application/json" -X POST -d "{\"username\":\"root\",\"password\":\"root\"}" -v http://127.0.0.1:5000/api/token
```

#### Management:
Manage from the web interface administration, default is https://localhost/#/login
Default root user password: 'root'