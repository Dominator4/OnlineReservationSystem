using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AuthenticationService.Services;
using AuthenticationService.Models;

namespace AuthenticationService
{
    /// <summary>
    /// Startup class for configuring services and middleware for the authentication service application.
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// Initializes a new instance of the Startup class.
        /// </summary>
        /// <param name="configuration">The configuration properties for the application.</param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        /// <summary>
        /// Gets the configuration properties for the application.
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// Configures the services to add to the container. This method gets called by the runtime.
        /// </summary>
        /// <param name="services">The collection of services to configure.</param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            // Configure the database context with SQL Server support.
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            // Add JWT authentication support.
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Key"])),
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidIssuer = Configuration["Jwt:Issuer"],
                        ValidAudience = Configuration["Jwt:Audience"]
                    };
                });

            // Setup CORS policy for the BookingClientApp.
/*
            services.AddCors(options =>
            {
                options.AddPolicy("AllowBookingClientApp", builder =>
                    builder.WithOrigins("https://localhost:44300")
                            .AllowAnyMethod()
                            .AllowAnyHeader());
            });
*/

            // Register AuthService with dependency injection.
            services.AddScoped<AuthService>();
        }

        /// <summary>
        /// Configures the HTTP request pipeline. This method gets called by the runtime.
        /// </summary>
        /// <param name="app">The application builder to configure.</param>
        /// <param name="env">The hosting environment information.</param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();
//            app.UseCors("AllowBookingClientApp");

            // Apply authentication and authorization middleware.
            app.UseAuthentication();
            app.UseAuthorization();

            // Define endpoints for the controllers.
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
