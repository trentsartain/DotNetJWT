using System;
using System.Collections.Generic;
using System.DirectoryServices.Protocols;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using AuthorizeServer.api.Models;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OAuth;
using System.DirectoryServices.ActiveDirectory;

namespace AuthorizeServer.api.Providers
{
    public class CustomOAuthProvider : OAuthAuthorizationServerProvider
    {

        public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            string clientId;
            string clientSecret;

            if (!context.TryGetBasicCredentials(out clientId, out clientSecret))
            {
                context.TryGetFormCredentials(out clientId, out clientSecret);
            }

            if (context.ClientId == null)
            {
                context.SetError("invalid_clientId", "client_Id is not set");
                return Task.FromResult<object>(null);
            }

            var audience = AudiencesStore.FindAudience(context.ClientId);

            if (audience == null)
            {
                context.SetError("invalid_clientId", $"Invalid client_id '{context.ClientId}'");
                return Task.FromResult<object>(null);
            }

            context.Validated();
            return Task.FromResult<object>(null);
        }

        public override Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            /* POTENTIAL ACTIVE DIRECTORY CODE
            object user = null;
            if (AuthenticateActiveDirectory(context.UserName, context.Password, "ADDomain"))
            {
                //TODO: Get user from DB by username (password validated by Active Directory)
            }
            else
            {
                //TODO: Get user from DB by username & password (Active Directory validation failed)
            }

            if (user == null)
            {
                context.SetError("invalid_grant", "The user name or password is incorrect.");
                return Task.FromResult<object>(null); ;
            }
            */

            context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { "*" });

            if (context.UserName != context.Password)
            {
                context.SetError("invalid_grant", "The user name or password is incorrect");
                return Task.FromResult<object>(null);
            }

            var identity = new ClaimsIdentity("JWT");

            identity.AddClaim(new Claim(ClaimTypes.Name, context.UserName));
            identity.AddClaim(new Claim("sub", context.UserName));
            identity.AddClaim(new Claim(ClaimTypes.Role, "Manager"));
            identity.AddClaim(new Claim(ClaimTypes.Role, "Supervisor"));

            var props = new AuthenticationProperties(new Dictionary<string, string> {{ "audience", context.ClientId ?? string.Empty }});
            var ticket = new AuthenticationTicket(identity, props);
            context.Validated(ticket);

            return Task.FromResult<object>(null);
        }

        private bool AuthenticateActiveDirectory(string username, string password, string domainName)
        {
            bool validation;
            try
            {
                var domainContext = new DirectoryContext(DirectoryContextType.Domain, domainName, username, password);
                var domain = Domain.GetDomain(domainContext);
                var controller = domain.FindDomainController();

                var lcon = new LdapConnection(new LdapDirectoryIdentifier(controller.IPAddress, false, false));
                var nc = new NetworkCredential(username, password, domainName);
                lcon.Credential = nc;
                lcon.AuthType = AuthType.Negotiate;
                lcon.Bind(nc);
                validation = true;
            }
            catch (Exception)
            {
                validation = false;
            }

            return validation;
        }
    }
}