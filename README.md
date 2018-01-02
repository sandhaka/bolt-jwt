BoltJwt - Json Web Token provider 
---
*Simplified authentication service based on JWT.*

**...Work in progress!**

#### What's is it?
**BoltJwt** is a tiny .Net Core authentication service. Built as **docker image**,  easy to integrate into 
docker-based web projects using **JWT** technology.   

> Users, groups, roles, and permissions can be defined via a backoffice web application.

Bolt Jwt share his **public key**. The services can consume the generated tokens.
Every token contains the 'authorizations' private claim with the user configured authorizations.

#### How to start?
##### Build your key pair
Creates a valid **key pair** used to generate the signed tokens. **There is a certificates example list 
and a link with the detailed instructions in certs/README.md.**

##### Run with docker-compose
Run a Mssql instance and the BoltJwt container using the docker-compose file in the project folder. 

Or create your compose file, 
following the instruction in [BoltJwt/README.md](https://github.com/sandhaka/bolt-jwt/blob/master/README.md) file.

##### Connect to the web app
Connect to the web interface with the 'root' user (Default password: 'root'). (default: http://localhost/#/login)
Then, creates / edit users, roles and  groups for your application.

Fixed entities:

'administrative' authorization is a built-in entity. Give this authorization to the users designed to be the 
administrator of the service.
The administrator users are allowed to manage the service entities (users, groups and roles) but not the 
system configuration.

##### Configure
Configure the system settings from the page http://localhost/#/configuration

Use yours authorizations in yours services. Authorizations are embedded in the Token. **You can decode it
with the public key created bofore.**

##### TODO
Roles and groups management aren't yet implemented.

#### Consumers setup example
##### ASP.Net Core implementation.

Create a Startup extension class and call it from 'ConfigureServices' startup method.
 
Add the authentication 'JWT Bearer'.
```c#
public static class JwtBearerServiceCollectionExtensions
{
    /// <summary>
    /// Add Jwt bearer through ServiceCollection interface
    /// </summary>
    /// <param name="services">Services collection</param>
    /// <param name="configuration">Configuration</param>
    /// <returns>Services collection</returns>
    public static IServiceCollection AddJwtBearerAuthentication(this IServiceCollection services,
        IConfiguration configuration)
    {
        var pubKeyPath = "certs/dev.boltjwt.crt";
        var publicKey = new X509Certificate2(pubKeyPath).GetRSAPublicKey();
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
            .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new RsaSecurityKey(publicKey),
                ValidateIssuer = true,
                ValidIssuer = configuration.GetSection("Jwt:Issuer").Value,
                ValidateAudience = true,
                ValidAudience = configuration.GetSection("Jwt:Audience").Value,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };
        });

        return services;
    }
}
```
Add yours policy
```c#
/* Startup.cs */
...
// Add authorization policies
services.AddAuthorization(options =>
{
    // Adding a custom policy to control the access to the controllers.
    options.AddCustomPolicies();
});
...
```
```c#
public static AuthorizationOptions AddCustomPolicies(this AuthorizationOptions authOptions)
{
    authOptions.AddPolicy("yourPolicy", policyBuilder => policyBuilder.AddRequirements(
        new AuthorizationsRequirement(Constants.AdministrativeAuth)));
    
    // Others ...
    
    return authOptions;
}
```
```c#
...
public class AuthorizationsRequirement : IAuthorizationRequirement
{
    public readonly string[] Authorizations;

    public AuthorizationsRequirement(params string[] authorizations)
    {
        Authorizations = authorizations;
    }
}
```
Create a handler for the policies
```c#
public class AuthorizationsHandler : AuthorizationHandler<AuthorizationsRequirement>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, AuthorizationsRequirement requirement)
    {

        /*
         * Check the custom authorizations list
         */

        // Get the authorizations list from the claims contained in the token obtained before
        var authorizationsList = context.User.Claims.FirstOrDefault(i => i.Type == "authorizations");

        var authorizations =
            JsonConvert.DeserializeObject<string[]>(
                authorizationsList?.Value ?? throw new NullReferenceException("Authorizations claim"));

        // Check is the user is authorized to access the resources
        foreach (var authorization in authorizations)
        {
            if (requirement.Authorizations.Contains(authorization))
            {
                context.Succeed(requirement);
                break;
            }
        }

        return Task.CompletedTask;
    }
}
```
Register the handler in the DI container as Singleton
```c#
...
builder.RegisterType<AuthorizationsHandler>()
    .As<IAuthorizationHandler>()
    .SingleInstance();
    ...
```
Now, you can protect your Endpoints, for example:
```c#
[HttpPost]
[Authorize(Policy = "yourPolicy")]
public async Task<IActionResult> UpdateAsync([FromBody] Command command)
{
// Code here ...
```

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

#### Diagram
![alt text](https://albumizr.com/ia/caaf8ed99c4884152d2d867a68dcd306.jpg)

#### Token Claims
Into Jwt payload there are some registered claims:
- Iss (Issuer) 
- Sub (Subject)
- Aud (Audience) 
- Iat (Issued At)
- Jti (JWT ID)

> Have a look to the [RFC 7519](https://tools.ietf.org/html/rfc7519) for more details about the registered claims.

After these claims there are a list of "private claims" defined by the application:
- isRoot Is a flag for the root user. Only the "Boltjwt" root has this flag set to true.
- userId Is the Id of the user
- username Username of course
- authorizations: That's the most important part. This is a json (string) that **contains a full list of the 
authorizations of the user. The consumers services have to read this claim to determine the level of
authorization of the request initiator.**
