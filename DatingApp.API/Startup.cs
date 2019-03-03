using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using DatingApp.API.Base;
using DatingApp.API.Business;
using DatingApp.API.Common;
using DatingApp.API.Common.ActionFilters;
using DatingApp.API.Common.Extensions;
using DatingApp.API.Common.Paging;
using DatingApp.API.Data;
using DatingApp.API.Helpers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace DatingApp.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            //var configBuilder = new ConfigurationBuilder()
            //.SetBasePath(Directory.GetCurrentDirectory())
            //.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Added SqlServer Connection
            services.AddDbContext<DataContext>(x => x.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            // Mvc, also added Json Serialize
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
                .AddJsonOptions(opt =>
                {
                    opt.SerializerSettings.ReferenceLoopHandling =
                    Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                });

            // Added Repository Service
            services.AddScoped<ILog, Log>();

            // // Added IAuthRepository Service
            // services.AddScoped<IAuthRepository, AuthRepository>();

            // // Added IDatingRepository Service
            // services.AddScoped<IDatingRepository, DatingRepository>();

            // // Added IPhotoRepository Service
            // services.AddScoped<IPhotoRepository, PhotoRepository>();

            // Added Repository Wrapper
            services.ConfigureRepositoryWrapper();

            // Added IPhotoBus
            services.AddScoped<IPhotoBus, PhotoBus>();
            services.AddScoped<IRoleBus, RoleBus>();

            // Add Cors
            services.AddCors();

            // Add User Seed
            services.AddTransient<Seed>();

            // Add AutoMapper
            services.AddAutoMapper();

            // services.AddCors(options =>
            //     {
            //         options.AddPolicy("AllowSpecificOrigin",
            //             builder => builder.WithOrigins("http://localhost:4200").AllowAnyHeader()
            //             .AllowAnyMethod());
            //     });

            // Get ClouldinarySettings from appsettings.json
            services.Configure<CloudinarySettings>(Configuration.GetSection("ClouldinarySettings"));
            // services.Configure<Params>(Configuration.GetSection("Paging"));

            // Jwt Authentication
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Configuration.GetSection("AppSettings:Token").Value)),
                        ValidateIssuer = false,
                        ValidateAudience = false
                    };
                });

            // Added ActionFilters Section
            services.AddScoped<UserFilterAttribute>();
            services.AddScoped<ValidationFilterAttribute>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, Seed seeder)
        {
            if (!env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // handler error from common/exceptionmiddleware
                app.ConfigureExceptionHandler();

                // app.UseHsts();
            }

            //app.UseHttpsRedirection();
            //seeder.SeedUsers();

            // app.UseCors(x => x.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
            //loggerFactory.AddConsole(Configuration.GetSection("Logging"));

            app.UseCors(x => x.WithOrigins("http://localhost:4200").AllowAnyHeader().AllowAnyMethod());

            //.UseCors("AllowSpecificOrigin");

            app.UseAuthentication();

            app.UseMvc();
        }
    }
}
