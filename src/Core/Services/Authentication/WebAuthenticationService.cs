using System.Web.Security;

namespace CommunitySite.Core.Services.Authentication
{
    public class WebAuthenticationService : AuthenticationService
    {
        public void SignIn(string username)
        {
            FormsAuthentication.SetAuthCookie(username, false);
        }
    }
}