using Base.Common.Jwt.Models;
using Base.Common.Jwt.Service;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Common.Jwt
{
    public static class Extensions
    {
        private static readonly string SectionName = "jwt";

        public static IServiceCollection AddJwtConfig(this IServiceCollection services)
        {
            var svcProvider = services.BuildServiceProvider();
            var config = svcProvider.GetRequiredService<IConfiguration>();

            var jwtOptions = new JwtOptions();
            config.Bind(SectionName, jwtOptions);
            services.AddSingleton(jwtOptions);
            if (!jwtOptions.Enabled)
            {
                return services;
            }
            services.AddTransient<IJwtService, JwtService>();
            services.AddTransient<JwtTokenValidatorMiddleware>();
            services
                .AddAuthentication(x =>
                {
                    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(cfg =>
                {
                    cfg.TokenValidationParameters = new TokenValidationParameters
                    {
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.SecretKey)),
                        ValidIssuer = JwtService.ServerIP,
                        ValidAudience = jwtOptions.ValidAudience,
                        ValidateAudience = jwtOptions.ValidateAudience,
                        ValidateLifetime = false,
                        ClockSkew = TimeSpan.Zero  // remove delay of token when expire
                    };

                    cfg.SaveToken = true;

                    cfg.Events = new JwtBearerEvents
                    {
                        OnMessageReceived = context =>
                        {
                            var accessToken = context.Request.Query["access_token"];

                            // If the request is for our hub...
                            var path = context.HttpContext.Request.Path;
                            if (!string.IsNullOrEmpty(accessToken) &&
                                path.StartsWithSegments("/signalr"))
                            {
                                // Read the token out of the query string
                                context.Token = accessToken;
                            }
                            return Task.CompletedTask;
                        },
                        OnAuthenticationFailed = context =>
                        {
                            var te = context.Exception;
                            return Task.CompletedTask;
                        },
                        OnForbidden = context =>
                        {
                            var te = context.Properties;
                            return Task.CompletedTask;
                        }
                    };
                });

            services.AddAuthorization();
            return services;
        }

        public static IApplicationBuilder UseJwtTokenWithValidator(this IApplicationBuilder app)
        {
            using (var scope = app.ApplicationServices.CreateScope())
            {
                var options = scope.ServiceProvider.GetService<JwtOptions>();
                if (!options.Enabled)
                {
                    return app;
                }
                app.UseMiddleware<JwtTokenValidatorMiddleware>();
            }
            return app;
        }

        public static long ToTimestamp(this DateTime dateTime)
        {
            var centuryBegin = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            var expectedDate = dateTime.Subtract(new TimeSpan(centuryBegin.Ticks));

            return expectedDate.Ticks / 10000;
        }

        public static Guid GetSessionKey(this HttpContext httpContext)
        {
            var claim = httpContext?.User?.Claims?.FirstOrDefault(c => c.Type.Equals(JwtRegisteredClaimNames.Jti))?.Value;
            return claim != null ? new Guid(claim) : Guid.Empty;
        }

        public static string GetRequestToken(this HttpContext httpContext)
        {
            return httpContext.GetTokenAsync("access_token")?.Result;
        }
    }
}
