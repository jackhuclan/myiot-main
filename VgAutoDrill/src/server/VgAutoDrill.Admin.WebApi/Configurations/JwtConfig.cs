using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using VgAutoDrill.Admin.Application.Helper;
using VgAutoDrill.Admin.Common.Configuration;
using VgAutoDrill.Infrastructure.Authentication;

namespace VgAutoDrill.Admin.WebApi.Configurations
{
    public static class JwtConfig
    {
        public static void AddJwtConfiguration(this IServiceCollection services)
        {
            //注册服务
            services.AddAuthentication(options =>
            {
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true, //是否验证Issuer
                    ValidIssuer = AppSettingsHelper.GetContent<string>("JwtConfig", "Issuer"), //发行人Issuer
                    ValidateAudience = true, //是否验证Audience
                    ValidAudience = AppSettingsHelper.GetContent<string>("JwtConfig", "Audience"), //订阅人Audience
                    ValidateIssuerSigningKey = true, //是否验证SecurityKey
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(AppSettingsHelper.GetContent<string>("JwtConfig", "SecretKey"))), //SecurityKey
                    ValidateLifetime = true, //是否验证失效时间
                    ClockSkew = TimeSpan.FromSeconds(30), //过期时间容错值，解决服务器端时间不同步问题（秒）
                    RequireExpirationTime = true,
                };
            });
            services.AddSingleton<IAuthorizationHandler, PermissionAuthorizationHandler>();
            services.AddSingleton<JwtHelper>();
        }
    }
}
