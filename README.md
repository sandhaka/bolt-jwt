BoltJwt - Json Web Token provider 
---
*Simplified authentication service based on JWT.*

**...Work in progress!**

#### What's is it?
**BoltJwt** is a tiny .Net Core authentication service. Built as **docker image**, 
it's easy to integrate into docker-based web projects using **JWT** technology.   

> Users, groups, roles, and permissions can be defined via a backoffice web application.

Bolt Jwt share his **public key**. The services can consume the generated tokens.
Every token contains a 'authorizations' private claim with the user configured authorizations.

#### How to start?
To use the service you have to do the following stuff:

Creates a valid **key pair** used to generate the signed tokens. **There is a certificates example list 
and a link with the detailed instructions in certs/README.md.**

##### Run with docker-compose
Run a Mssql instance and the BoltJwt container using the docker-compose file in the project folder. 
```sh 
SQL_CONNECTION_STRING='<your-sql-conn-string>'  SQL_PASSWORD='<your-sa-password>' docker-compose up --build
```
Or create your compose file

To run BoltJwt as a single container, following the instruction in BoltJwt/README.md file.

##### Connect to the web app (*TODO*)
Connect to the web interface with the 'root' user (Default password: 'root'). 
Then, creates / edit users, roles and  groups for your application. 

Use yours authorizations in yours services. Authorizations are embedded in the Token. **You can decode it
with the public key created bofore.**

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
