# IdentityServer4Org
Authentication server for organizational use with IdentityServer4
## Introduction
An authentication server with OpenID Connect and invitation based user registration. Built using [IdentityServer4](https://github.com/IdentityServer/IdentityServer4) and ASP.NET Core Identity.

## Deployment
Current implementation requires MS SQL server for persistence. Project uses Entity Framework Core for database handling.
1. Create an empty database in MS SQL server and update the connection string in IdentityServer4Org\appsettings.json
2. Run following Entity Framework commands to apply Database migrations from IdentityServer4Org project directory.
    * dotnet ef database update --context ApplicationDbContext
    * dotnet ef database update --context PersistedGrantDbContext
    * dotnet ef database update --context ConfigurationDbContext
3. In order to seed admin user in the first run, specify values for following keys to be loaded through IConfiguration.
    * SeedAdminEmail
    * SeedAdminPassword

## Usage
Application will seed 'admin' user during initial run with above specified credentials. If setup is successful, you should be able to see OIDC configuration information at: `/.well-known/openid-configuration`.
### Administration
Go to administration panel at `/admin`. When prompted to login, use 'admin' for username and value of SeedAdminPassword for password. Users, API resources, and clients are listed in admin page. Only users in 'Administrator' role can access this area. Initialization adds 'Administrator' role to 'admin' user.
### User registration
Select 'Register new user' to open user registration page. Username and email are mandatory fields. If the username is new, a confirmation e-mail is sent to the provided address. Visiting the link will confirm the email address followed by a password setup prompt.
### Password reset
Password can be reset by visiting 'Forgot Password?' link in login page. Application expects each user to have a unique email address. If a user can be found for the provided email address, a reset email is sent.
## Resources
### Identity Server 4
* [The big picture](http://docs.identityserver.io/en/latest/intro/big_picture.html)
* [Quick start](http://docs.identityserver.io/en/latest/quickstarts/0_overview.html)
### ASP.NET Core Identity
* [Introduction to Identity](https://docs.microsoft.com/en-us/aspnet/core/security/authentication/identity?view=aspnetcore-3.1&tabs=visual-studio)
* Pluralsight [course on ASP.NET Authentication](https://www.pluralsight.com/courses/aspdotnet-authentication-big-picture)
* Pluralsight [course on ASP.NET Core Identity Deep Dive](https://www.pluralsight.com/courses/aspdotnet-core-identity-deep-dive)
