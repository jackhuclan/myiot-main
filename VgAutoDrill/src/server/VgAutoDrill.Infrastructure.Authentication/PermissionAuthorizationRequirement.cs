using Microsoft.AspNetCore.Authorization;

namespace VgAutoDrill.Infrastructure.Authentication
{
    public class PermissionAuthorizationRequirement : IAuthorizationRequirement
    {
        public string AuthCode { set; get; }
        public PermissionAuthorizationRequirement(string authCode)
        {
            AuthCode = authCode;
        }
    }
}
