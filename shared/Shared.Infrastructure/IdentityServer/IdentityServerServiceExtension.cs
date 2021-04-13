using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using IdentityServer4;
using Microsoft.Extensions.Configuration;
using Shared.Infrastructure;

namespace Shared.Infrastructure
{
    public static class IdentityServerServiceExtension
    {
        public static IServiceCollection AddIdentityServer(this IServiceCollection services, IConfiguration configuration)
        {
            /*
            //身份验证配置
            services.AddIdentity<ApplicationUser, ApplicationRole>()
                    .AddEntityFrameworkStores<ApplicationDbContext>()
                    .AddDefaultTokenProviders()
                    .AddClaimsPrincipalFactory<ClaimsPrincipalFactory>();
            */
            //认证服务器配置
            services.AddIdentityServer()
                .AddDeveloperSigningCredential()
                .AddInMemoryIdentityResources(IdentityConfig.GetIdentityResources())
                .AddInMemoryApiResources(IdentityConfig.GetApiResources())
                .AddInMemoryClients(IdentityConfig.GetClients())
                .AddTestUsers(IdentityConfig.GetTestUsers())
                .AddResourceOwnerValidator<ResourceOwnerPasswordValidator>();

            services.AddAuthentication("Bearer")
                .AddJwtBearer("Bearer", options =>
                {
                    options.Authority = "http://localhost:5000";
                    options.RequireHttpsMetadata = false;
                    options.Audience = "api";
                })
                .AddOpenIdConnect("oidc", options =>
                {
                    options.SignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme;
                    options.SignOutScheme = IdentityServerConstants.SignoutScheme;
                    options.Authority = "https://demo.identityserver.io/";
                    options.ClientId = "implicit";
                    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                    {
                        NameClaimType = "name",
                        RoleClaimType = "role"
                    };
                });
            /*
            services.AddAuthentication(option =>
            {
                option.DefaultAuthenticateScheme = "Cookie";
                option.DefaultChallengeScheme = "oidc";
            })
            .AddCookie("Cookie")
            .AddOpenIdConnect("oidc", option =>
            {
                option.Authority = "http://localhost:5000";
                option.RequireHttpsMetadata = false;

                option.ClientId = "mvc";
                option.SaveTokens = true;
            });
            */
            return services;
        }
    }
}
