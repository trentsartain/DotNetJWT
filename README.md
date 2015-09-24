# DotNetJWT
This is an example project that provides a .NET Authentication Server using OWIN and a custom OAuthProvider and a resource server.  There is also an example AngularJS project that utilizes the authentication server to get the JWT and then passes it in the header to the resource server.

To test, simply run both projects in visual studio (the solution is set up to run both when start is clicked), then run the TestJWTApp AngularJs project via command line.

# AuthorizeServer.Api
This is the authentication server.  Tokens can be requested from the endpoint /oauth2/token.  Posted data should be:

* username
* password
* grant_type
* clientId

Currently, the only audience has an ID of d3c2e8f35db549df8b6507f7e025301d.  The audience and its secret can be found in AuthorizeServer.Api -> Models -> AudiencesStore.cs.

# Resource Server.Api
This is the test resource server.  It has one endpoint at /api/protected.  This endpoint requires both authentication and authorization.  The authenticated user must be a supervisor.  To hit this endpoint, one must bear a JWT with the correct credentials and have it be signed by the authentication server using the resource server's secret.  

# TestJWTApp
To run, simply:
> npm install

> gulp dev

To login, enter a username and password that are equal to eachother.  See that the resource server API call is rejected until you are logged in.  Once you are logged in, the JWT information will be visible and the API call will be accepted. 



