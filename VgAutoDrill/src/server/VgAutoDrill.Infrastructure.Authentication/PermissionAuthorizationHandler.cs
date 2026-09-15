using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace VgAutoDrill.Infrastructure.Authentication
{
    public class PermissionAuthorizationHandler : AuthorizationHandler<PermissionAuthorizationRequirement>
    {
        public PermissionAuthorizationHandler()
        {

        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionAuthorizationRequirement requirement)
        {
            var authCode = requirement.AuthCode;
            if (context.User != null)
            {
                var userIdClaim = context.User.FindFirst(_ => _.Type == ClaimTypes.Name);
                if (context.User.IsInRole("1") || userIdClaim != null)//admin
                {
                    context.Succeed(requirement);
                }
                //else
                //{
                //    var userIdClaim = context.User.FindFirst(_ => _.Type == ClaimTypes.NameIdentifier);
                //    var userData = context.User.FindFirst(_ => _.Type == ClaimTypes.UserData) == null ? "" : context.User.FindFirst(_ => _.Type == ClaimTypes.UserData).ToString();
                //    if (!string.IsNullOrEmpty(userData))
                //    {
                //        if (userData.Contains(authCode))
                //        {
                //            context.Succeed(requirement);
                //            return Task.CompletedTask;
                //        }
                //        else
                //        {
                //            context.Fail();
                //        }
                //    }
                //    context.Fail();
                //}
            }
            return Task.CompletedTask;
        }
    }
}
