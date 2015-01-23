using System.Security.Principal;
using System.Web.Security;

namespace Hello_SelfHost_WebAPI.Attributes
{
    public class MembershipHttpAuthorizeAttribute : BasicHttpAuthorizeAttribute
    {
        protected override bool TryCreatePrincipal(string user, string password,
            out IPrincipal principal)
        {
            principal = null;
            if (!Membership.Provider.ValidateUser(user, password))
                return false;
            var roles = new string[0];
            principal = new GenericPrincipal(new GenericIdentity(user), roles);
            return true;
        }
    }
}