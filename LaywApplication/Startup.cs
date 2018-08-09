using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using LaywApplication.Configuration;
using LaywApplication.Controllers.APIUtils;
using LaywApplication.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

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
            services.Configure<ServerIP>(Configuration.GetSection("server-ip"));

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
                         context.Identity.AddClaim(new Claim(ClaimTypes.Uri, "https://www.googleapis.com/oauth2/v2/userinfo?access_token=" + context.AccessToken, ClaimValueTypes.String, "Google"));
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
                         context.Identity.AddClaim(new Claim(ClaimTypes.Uri, "https://graph.facebook.com/me/picture?redirect&type=large&access_token=" + context.AccessToken, ClaimValueTypes.String, "Facebook"));
                         return Task.FromResult(0);
                     }
                 };
             });

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
            //app.UseMvc();
            app.UseMvcWithDefaultRoute();

            app.Run(async (context) =>
            {
                await context.Response.WriteAsync("Oops, something went wrong");
            });
        }
    }
}
