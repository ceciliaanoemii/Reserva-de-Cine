using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Reserva_B.Data;
using Reserva_B.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Reserva_B
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;

            #region Tipo de DB Provider a usar 
            try
            {
                _dbInMemory = Configuration.GetValue<bool>("DbInMem");
            }
            catch 
            {
                _dbInMemory = true;
            }
        }

        public IConfiguration Configuration { get; }

        private bool _dbInMemory = false;

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            if (_dbInMemory)
            {
                //usando inMemoryDataBase
                services.AddDbContext<ReservaContext>(options => options.UseInMemoryDatabase("ReservaDB"));
            }
            else
            {
                //Usando SQL
                services.AddDbContext<ReservaContext>(options => 
                    options.UseSqlServer(Configuration.GetConnectionString("ReservaCS"))
                    );

            }
            #endregion
            

            #region Identity
            services.AddIdentity<Persona, Rol>().AddEntityFrameworkStores<ReservaContext>();

            services.Configure<IdentityOptions>(
                opciones =>
                {
                    opciones.Password.RequiredLength = 8;
                }
                );

            #endregion

            services.PostConfigure<CookieAuthenticationOptions>(IdentityConstants.ApplicationScheme,
                opciones =>
                {
                    opciones.LoginPath = "/Account/IniciarSesion";
                    opciones.AccessDeniedPath = "/Account/AccesoDenegado";
                });

            services.AddScoped<IDbInit, PrecargaDatos>();
            services.AddControllersWithViews();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ReservaContext contexto)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            using (var servicesScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                var contexto1 = servicesScope.ServiceProvider.GetRequiredService<ReservaContext>();
                if (!_dbInMemory)
                {
                    contexto.Database.Migrate();
                }

                servicesScope.ServiceProvider.GetService<IDbInit>().Seed();
            }
          
           
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
