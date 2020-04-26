// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


  
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityApi.Data;
using IdentityApi.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace IdentityApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            // uncomment, if you want to add an MVC-based UI
            //services.AddControllersWithViews();
            services.AddControllers();
            services.AddMvc().AddNewtonsoftJson();

            services.AddDbContext<ApplicationContext>(options =>
                options.UseSqlite("Data Source = AspIdUsers.db"));

            services.AddIdentity<User, IdentityRole>()
                    .AddEntityFrameworkStores<ApplicationContext>()
                    .AddDefaultTokenProviders();

            services.Configure<IdentityOptions>(options => {
                options.Password.RequiredLength = 6;
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
            });

            services.AddIdentityServer()
                    .AddDeveloperSigningCredential()
                    .AddInMemoryIdentityResources(Config.Ids)
                    .AddInMemoryApiResources(Config.Apis)
                    .AddInMemoryClients(Config.Clients)
                    .AddAspNetIdentity<User>()
                    .AddProfileService<ProfileService>();

            services.AddAuthentication("Bearer")
                    .AddJwtBearer("Bearer", options => {
                            options.Authority = "http://localhost:5000";
                            options.RequireHttpsMetadata = false;
                            options.Audience = "book";
                    });
        }
        public void Configure(IApplicationBuilder app, 
                              IWebHostEnvironment env,
                              ApplicationContext context,
                              RoleManager<IdentityRole> role,
                              UserManager<User> user)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            // uncomment if you want to add MVC
            //app.UseStaticFiles();
            app.UseRouting();

            app.UseIdentityServer();

            // uncomment, if you want to add MVC
            app.UseAuthorization();
            app.UseAuthentication();
            app.UseEndpoints(endpoints =>
            {
               endpoints.MapDefaultControllerRoute();
            });

            SeedData.Initialize(context,user,role).Wait();
        }
    }
}
