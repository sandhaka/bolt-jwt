BoltJwt - Json Web Token provider 
---
*Simplified authentication service based on JWT.*

**...Work in progress!**

#### What's is it?
**BoltJwt** is a tiny .Net Core authentication service. Built as **docker image**, 
it's easy to integrate into docker-based web projects that use **JWT** technology.   

> Users, groups, roles, and permissions can be defined via a backoffice web application.

Bolt Jwt share his **public key**. The services can then consume the generated tokens.
Every token contains a 'authorizations' private claim with the user configured authorizations.

#### How to start?
To use the service you have to do the following stuff:

- Creates a valid **key pair** used to generate the signed tokens. There is a certificates example list 
and a link with the detailed instructions in certs/README.md. 
Place yours keys and certificates in the certs folder with the correct names. 
If you want to edit the names you have to edit the settings of the project with the corrects one (In a production
environment you should to take care of the location security of these certificates).

- Run a Mssql instance and the BoltJwt container using the docker-compose file in the project folder. 
*TODO: Create a script to init the DB, at the moment you have to run: `dotnet ef database update -v`
before starting the service at the first time. At the moment you have also to customize the connection string.*

- (*TODO*) Connect to the web interface with the 'root' user (Default password: 'root'). 
Then, creates / edit users, roles and  groups for your application. 

- Use yours authorizations in yours services. Authorizations are embedded in the Token. **You can decode it
with the public key created bofore.**

#### Consumers setup example
*Coming soon*

#### Diagram
![alt text](https://albumizr.com/ia/caaf8ed99c4884152d2d867a68dcd306.jpg)

#### Contained Claims in Token
Into Jwt payload there are some registered claims:
- Iss (Issuer) 
- Sub (Subject)
- Aud (Audience) 
- Iat (Issued At)
- Jti (JWT ID)

>Have a look to the [RFC 7519](https://tools.ietf.org/html/rfc7519) for more details about the registered claims.

After these claims there are a list of "private claims" defined by the application:
- isRoot Is a flag for the root user. Only the "Boltjwt" root has this flag set to true.
- userId Is the Id of the user
- username Username of course
- authorizations: That's the most important part. This is a json (string) that **contains a full list of the 
authorizations of the user. The consumers services have to read this claim to determine the level of
authorization of the request initiator.**
