using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace WebApi.Shared.Services
{
    public static class  JWTToken
    {
        public static void JWTAuthentication(this IServiceCollection services,  IConfiguration Configuration)
        {
            //configure strongly typed settings objects
            var appSettingsSection = Configuration.GetSection("AppSettings");
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer("Bearer", options =>
           {
               options.TokenValidationParameters = new TokenValidationParameters
               {
                   ValidateIssuer = true,
                   ValidateAudience = true,
                   ValidateLifetime = true,
                   ValidateIssuerSigningKey = true,
                   ValidIssuer = Configuration["Jwt:Issuer"],
                   ValidAudience = Configuration["Jwt:Audience"],
                   ValidateActor = true,
                   IssuerSigningKey = new SymmetricSecurityKey
                 (Encoding.UTF8.GetBytes(Configuration["Jwt:Key"]))
               };
           }).AddJwtBearer("Private", x =>
           {
               x.RequireHttpsMetadata = false;
               x.SaveToken = true;
               x.TokenValidationParameters = new TokenValidationParameters
               {
                   ValidateIssuerSigningKey = true,
                   IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Configuration["Jwt:key"])),
                   ValidateIssuer = false,
                   ValidateAudience = false
               };
           });
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie();
        }
    }
}
