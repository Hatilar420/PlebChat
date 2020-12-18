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
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer;
using ChatApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using ChatApp.Security;
using ChatApp.Util;
using ChatApp.services;
namespace ChatApp
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
            var TokenParam =  new TokenValidationParameters()
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Configuration.GetValue<string>("SecurityKey"))),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    RequireExpirationTime = false,
                    ValidateLifetime = true
                };

             services.AddSingleton(TokenParam);
             services.AddAuthentication(configureOptions:options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(x =>
            {
                x.SaveToken = true;
                x.TokenValidationParameters = TokenParam;

                x.Events = new JwtBearerEvents{
                   OnMessageReceived = context =>{
                       var accessToken = context.Request.Query["access_token"];
                       var path = context.HttpContext.Request.Path;
                       if(!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments("/pleb")){

                           context.Token = accessToken;
                       }
                       return Task.CompletedTask;
                   }     
                };


            });
            services.AddControllersWithViews()
    .AddNewtonsoftJson(options =>
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
);
            services.AddTransient<Iuser, UserService>();
            services.AddSingleton<IUserIdProvider, EmailBasedUserId>();
            services.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<ChatContext>();
            services.AddControllers();
            services.AddSignalR();
            services.AddTransient<ISecurity,SecurityService>();
            services.AddDbContextPool<ChatContext>(option => option.UseSqlServer(Configuration.GetConnectionString("App")));

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
             
            //app.UseHttpsRedirection();
             
            app.UseCors(builder =>
            {
              builder.WithOrigins("http://localhost:3000")
              .AllowAnyHeader().AllowAnyMethod().AllowCredentials();
            });
               
            app.UseStaticFiles();
            
            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<ChatHub>("/hub");
                endpoints.MapHub<PlebChat>("/pleb");
            });
        }
    }
}
