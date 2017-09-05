using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security.Infrastructure;
using Microsoft.Owin.Security;
using System.Security.Claims;
using System.Security.Principal;

namespace RxTest.Infrastructure.Owin.Authentication
{
    public class DummmyAuthOptions : AuthenticationOptions
    {
        public DummmyAuthOptions(string authenticationType) : base(authenticationType)
        {
        }
    }

    public class DummyAuthenticationMiddleware : AuthenticationMiddleware<DummmyAuthOptions>
    {
        public DummyAuthenticationMiddleware(OwinMiddleware nextMiddleware, DummmyAuthOptions authOptions)
            : base(nextMiddleware, authOptions)
        { }

        protected override AuthenticationHandler<DummmyAuthOptions> CreateHandler()
        {
            return new DummyAuthenticationHandler();
        }
    }

    public class DummyAuthenticationHandler : AuthenticationHandler<DummmyAuthOptions>
    {
        protected async override Task<AuthenticationTicket> AuthenticateCoreAsync()
        {
            var userName = Request.Cookies["userName"] ?? "anonymous";
            var myIdentity = new GenericIdentity(userName, "dummy");
            var identity = new ClaimsIdentity(myIdentity);
            return new AuthenticationTicket(identity, new AuthenticationProperties());
        }
    }
}