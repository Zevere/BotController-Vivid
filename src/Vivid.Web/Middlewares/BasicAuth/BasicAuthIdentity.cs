using System.Security.Principal;

namespace Vivid.Web.Middlewares.BasicAuth
{
    public class BasicAuthIdentity : IIdentity
    {
        public string AuthenticationType => "Basic";

        public bool IsAuthenticated { get; }

        public string Name { get; }

        public BasicAuthIdentity(string name, bool isAuthenticated = true)
        {
            Name = name;
            IsAuthenticated = isAuthenticated;
        }
    }
}
