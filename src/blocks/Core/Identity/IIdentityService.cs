using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Core.Identity
{
    public interface IIdentityService
    {
        ClaimsPrincipal GetUserIdentityById(int userId);
        ClaimsPrincipal GetUserIdentity();
        string GetUserName();
    }
}