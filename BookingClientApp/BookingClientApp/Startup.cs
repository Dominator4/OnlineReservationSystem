using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookingClientApp.Services;

namespace BookingClientApp
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
            services.AddHttpClient<AuthServiceClient>(client =>
            {
                client.BaseAddress = new Uri(Configuration["AuthenticationService:BaseAddress"]); // Pobierz adres URL z pliku konfiguracyjnego
            });

            services.AddHttpClient<ReservationServiceClient>(client =>
            {
                client.BaseAddress = new Uri(Configuration["ReservationService:BaseAddress"]);
            });
            services.AddHttpContextAccessor();
            services.AddControllersWithViews();

            // Dodaj konfiguracjê CORS, jeœli potrzebujesz komunikacji z innymi us³ugami z ró¿nych Ÿróde³
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", // Ta polityka pozwala na ¿¹dania z dowolnego Ÿród³a; dostosuj wed³ug potrzeb
                    builder => builder.AllowAnyOrigin()
                                      .AllowAnyMethod()
                                      .AllowAnyHeader());
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseCors("AllowAll"); // U¿yj skonfigurowanej polityki CORS

            //app.UseAuthentication();
            //app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
