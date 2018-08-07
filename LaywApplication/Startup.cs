using System.Linq;
using LaywApplication.Configuration;
using LaywApplication.Controllers.APIUtils;
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
                 var configurationGoogle = Configuration.GetSection("google-codes").Get<GoogleCodes>();
                 options.ClientId = configurationGoogle.ClientId;
                 options.ClientSecret = configurationGoogle.ClientSecret;

                 options.Events = new OAuthEvents
                 {
                     OnCreatingTicket = async context =>
                     {
                         if (context.Identity.AuthenticationType.Equals("Google")) //A seconda del tipo cambiano gli url sotto. cercare un modo migliore di quegli url
                         {
                             //todo cercare se c'è un modo migliore per recuperare dati
                             string email = context.Identity.Claims.FirstOrDefault(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress").Value;
                             string name = context.Identity.Claims.FirstOrDefault(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name").Value;

                             //todo Gestire che il medico esista già con una get che ancora non c'è
                             string json = "{\"doctor\": {\"name\": \"" + name + "\", \"email\": \"" + email + "\"}}";
                             Utils.Post("http://localhost:4567/api/v1.0/doctors", json);
                         }
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
