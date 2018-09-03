using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using LaywApplication.Configuration;
using LaywApplication.Controllers.Utils;
using LaywApplication.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace LaywApplication
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<Kendo>(Configuration.GetSection("kendo"));
            services.Configure<Theme>(Configuration.GetSection("theme"));
            services.Configure<ChartGoalPatientPageInfo>(Configuration.GetSection("chart-goal-patient-page-info"));

            var settingsServerIP = Configuration.GetSection("server-ip").Get<ServerIP>();
            services.AddSingleton(settingsServerIP);

            var settingsJsonStructure = Configuration.GetSection("json-structure").Get<JsonStructure>();
            services.AddSingleton(settingsJsonStructure);

            services.AddAuthentication(options =>
            {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            })

            .AddCookie(options =>
            {
                options.LoginPath = "/login";
                options.LogoutPath = "/signout";
            })

            .AddGoogle(options =>
             {
                 var configuration = Configuration.GetSection("google-codes").Get<OAuthCodes>();
                 options.ClientId = configuration.ClientId;
                 options.ClientSecret = configuration.ClientSecret;

                 options.Events = new OAuthEvents
                 {
                     OnCreatingTicket = context =>
                     {
                         Uri apiRequestUri = new Uri("https://www.googleapis.com/oauth2/v2/userinfo?access_token=" + context.AccessToken);
                         dynamic result = JsonConvert.DeserializeObject(APIUtils.Get(apiRequestUri.ToString()).ToString());
                         
                         context.Identity.AddClaim(new Claim(ClaimTypes.Uri, (string)result.picture, ClaimValueTypes.String, "Google"));
                         return Task.FromResult(0);
                     }
                 };
             })
             
            .AddFacebook(options =>
             {
                 var configuration = Configuration.GetSection("facebook-codes").Get<OAuthCodes>();
                 options.ClientId = configuration.ClientId;
                 options.ClientSecret = configuration.ClientSecret;

                 options.Events = new OAuthEvents
                 {
                     OnCreatingTicket = context =>
                     {
                         Uri apiRequestUri = new Uri("https://graph.facebook.com/me/picture?redirect&type=large&access_token=" + context.AccessToken);
                         dynamic result = JsonConvert.DeserializeObject(APIUtils.Get(apiRequestUri.ToString()).ToString());
                         
                         context.Identity.AddClaim(new Claim(ClaimTypes.Uri, (string)result.data.url, ClaimValueTypes.String, "Facebook"));
                         return Task.FromResult(0);
                     }
                 };
             });

            services.AddSession(options =>
            {
                options.Cookie.Name = ".Layw.Session";
            });

            services.AddSignalR();

            services.AddMvc();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            /*
             Shortcut per:

             routes.MapRoute(
                name: "default",
                template: "{controller=Home}/{action=Index}/{id?}");
             */

            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseSession();
            //app.UseMvc();

            app.UseSignalR(routes =>
            {
                routes.MapHub<MQTTHub>("/mqttHub");
            });

            app.UseMvcWithDefaultRoute();

            app.Run(async (context) =>
            {
                await context.Response.WriteAsync("Oops, something went wrong");
            });
        }
    }
}
