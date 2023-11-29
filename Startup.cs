using System.ComponentModel.Design;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Ventas.Models;
using Ventas.Interfaces;
using Ventas.Services;
using AutoMapper;
using Ventas.Profiles;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IO;
using Microsoft.Extensions.FileProviders;

namespace Ventas
{
    public class Startup
    {

        public Startup(IConfiguration configuration)
        {
            this.Configuration = configuration;

        }
        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //permitiendo acceso desde el frontend
            services.AddCors();

            services.AddControllers();

            //Configurado el contexto a la base de datos sql 
            services.AddDbContext<VentasContext>(options =>
            options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));


            //Configurando todo lo necesario para la autenticaci�n
            //Configurando el uso de JWT para la Autenticacion de la API 
            //Declarando el uso de un seed secreto,definido en appsettings.json 
            var appSettingsSection = Configuration.GetSection("AppSettings");
            services.Configure<AppSettings>(appSettingsSection);

            var appSettings = appSettingsSection.Get<AppSettings>();
            var key = Encoding.ASCII.GetBytes(appSettings.Secret);

            //definiendo el tipo de autenticaci�n(Usando paquete JwtBearer para la autenticaci�n)
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            //configurando valores por defecto
            .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false

                };
            });



            //Declarando la Inyecci�n de Dependencias
            services.AddScoped<IVentas, CVentas>();
            services.AddScoped<IUsuario, CUsuario>();

            //Declarando el uso de automapper
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.AddAutoMapper(typeof(VentasProfile));


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

            // Habilitando el uso de Archivos estaticos
            app.UseStaticFiles();


            //permitiendo acceso desde el frontend
            app.UseCors(x => x
            .AllowAnyMethod()
            .AllowAnyOrigin()
            .AllowAnyHeader());

            //definiendo el uso de autenticacion
            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
