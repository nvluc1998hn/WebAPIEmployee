using Base.Common.Cache.MemCache;
using Base.Common.Cache;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Base.Common.Jwt;
using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.ResponseCompression;
using System.IO.Compression;
using System.IdentityModel.Tokens.Jwt;
using Base.Common.Mvc;
using Microsoft.Net.Http.Headers;
using Microsoft.OpenApi.Models;

namespace Base.Common
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Đăng ký các dịch vụ dùng chung cho toàn Solution
        /// </summary>
        public static IServiceCollection AddServiceCommon(this IServiceCollection services)
        {
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(c =>
            {
                // Define the Bearer token authentication
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.Http,
                    In = ParameterLocation.Header,
                    Name = "Authorization",
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    Description = "Enter 'Bearer' followed by a space and your JWT token."
                });

                // Require Bearer token authentication for all endpoints
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
            });
            services.AddHttpContextAccessor();
            services.AddJwtConfig();
            services.AddHttpClient();
            services.AddRedisServices();
            // Enable Response Compression
            services.Configure<GzipCompressionProviderOptions>(options => options.Level = CompressionLevel.Optimal);
            services.AddResponseCompression(options =>
            {
                options.Providers.Add<GzipCompressionProvider>();
                IEnumerable<string> MimeTypes = new[]
                {
                     "application/javascript",
                     "application/json",
                     "application/xml",
                     "text/css",
                     "text/plain",
                     "text/html",
                     "text/json",
                     "text/xml"
                };
                options.MimeTypes = MimeTypes;
                options.EnableForHttps = true;
            });

            // Tùy chỉnh tên FluentValidation
            ValidatorOptions.Global.DisplayNameResolver = (type, member, expression) =>
            {
                return member?.Name;
            };
            return services;
        }

        public static IApplicationBuilder UseServiceCommon(this IApplicationBuilder app)
        {
            // Enable Response Compression
            app.UseResponseCompression();
            app.UseStaticFiles();
            app.UseXContentTypeOptions();
            app.UseReferrerPolicy(opts => opts.NoReferrer());
            app.UseXXssProtection(options => options.EnabledWithBlockMode());
            app.UseXfo(options => options.Deny());
            app.UseHttpsRedirection();
            app.UseSwagger();
            app.UseRouting();
            app.UseCors("GPSOrigins");
            app.UseOptionsMethod();

            app.UseJwtTokenWithValidator();
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            return app;
        }
    }
}
