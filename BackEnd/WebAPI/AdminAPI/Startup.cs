using Admin.Domain;
using AutoMapper;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Base.Common;
using Base.Common.Cache;
using Admin.Application;
using Admin.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Admin.Application.Mapper;
using System;

namespace AdminAPI
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            services.AddMediatR(configuration =>
            {
                configuration.RegisterServicesFromAssemblies(typeof(Program).Assembly);
            });
            services.AddControllers();
            services.AddSession();
            services.AddAutoMapperSetup();
            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("WebMockDB")));
            services.AddSession();
            services.AddRepositories();
            services.AddServices();
            services.AddMvcCore().AddApiExplorer();
            services.AddServiceCommon();
            services.AddEfCoreSqlServer<ApplicationDbContext>();
            services.AddResponseCompression();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //if (env.IsDevelopment())
            //{
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                // Đặt Swagger endpoint cho mỗi phiên bản API (nếu có)
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "Admin API");

                // Tùy chọn: Đặt DefaultModelsExpandDepth để giao diện gọn gàng hơn
                options.DefaultModelsExpandDepth(-1);

            });
            //  }

            app.UseHttpsRedirection();

            app.UseAuthentication();

            app.UseRouting();

            app.UseCors(x => x
               .AllowAnyMethod()
               .AllowAnyHeader()
               .SetIsOriginAllowed(origin => true) // allow any origin
               .AllowCredentials());

            app.UseAuthorization();
            app.UseServiceCommon();

            app.UseSession();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
