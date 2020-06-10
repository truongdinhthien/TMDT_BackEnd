using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using BookApi.Persistence;
using BookApi.Services;
using BookApi.Core.DTOs;
using BookApi.Configuration;
using MassTransit;
using BookApi.Consumer;
using MessageBus.Config;

namespace BookApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAutoMapper(typeof(MappingProfile));
            services.AddDbContext<BookContext>(options => options.UseSqlite("Data Source=Book.db"));
            services.AddControllers()
                    .AddNewtonsoftJson(options =>
                    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

            services.AddScoped<RatingConsumer>();

            services.AddMassTransit(cfg =>
            {
                cfg.AddConsumer<RatingConsumer>();

                cfg.AddBus(provider => RabbitMqBus.ConfigureBus(provider, (cfg, host) =>
                {
                    cfg.ReceiveEndpoint(BusConstant.RateQueue, ep =>
                    {
                        ep.ConfigureConsumer<RatingConsumer>(provider);
                    });
                }));
            });

            services.AddMassTransitHostedService();

            services.AddAuthentication("Bearer")
                    .AddJwtBearer("Bearer", options =>
                    {
                        options.Authority = "https://localhost:3117";
                        options.RequireHttpsMetadata = false;
                        options.Audience = "book";
                    });

            services.AddCors(options =>
            {
                options.AddPolicy("_myAllowSpecificOrigins",
                    builder =>
                    {
                        builder.WithOrigins(
                                "http://localhost:3000",
                                "http://localhost:3001"
                            )
                            .AllowAnyHeader()
                            .AllowAnyMethod();
                    });
            });
            services.AddScoped<BookService>();
            services.AddScoped<CategoryService>();
            services.AddScoped<ITokenConfiguration,TokenConfiguration>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors(x => x
               .AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader());

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseStaticFiles();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            // app.Run(async context =>
            // {
            //     context.Response.ContentType = "application/json";
            //     await context.Response.WriteAsync(new JsonResult( new {
            //         success = false,
            //         message = "Can not find page"
            //     }).ToString());
            // });
        }
    }
}
