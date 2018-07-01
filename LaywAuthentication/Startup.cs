using LaywAuthentication.Configuration;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http;

namespace LaywAuthentication
{
    public class Startup
    {
        private static readonly HttpClient client = new HttpClient();

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<HomestationIP>(Configuration.GetSection("homestation-ip"));
            services.Configure<FitbitCodes>(Configuration.GetSection("fitbit-codes"));

            services.AddAuthentication(options =>
            {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            })

            .AddCookie(options =>
            {
                options.LoginPath = "/login";
                options.LogoutPath = "/signout";
            })

            .AddFitbit(options =>
            {
                // https://dev.fitbit.com/build/reference/web-api/oauth2/
                // https://dev.fitbit.com/
                var configurationFitbit = Configuration.GetSection("fitbit-codes").Get<FitbitCodes>();

                options.ClientId = configurationFitbit.ClientId;
                options.ClientSecret = configurationFitbit.ClientSecret;

                // Importante per avere accesso all'access token.
                options.SaveTokens = true;

                options.Events = new OAuthEvents
                {
                    OnCreatingTicket = async context =>
                    {
                        // Qui è possibile salvare il token.
                        var accessToken = context.AccessToken;

                        var configurationIP = Configuration.GetSection("homestation-ip").Get<HomestationIP>();
                        
                        var content = new StringContent("{ \"accessToken\" : " + accessToken + "}");
                        var response =  await client.PostAsync(configurationIP.getTotalUrl(), content);
                        var responseString = await response.Content.ReadAsStringAsync();    
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

            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseMvc();

            app.Run(async (context) =>
            {
                await context.Response.WriteAsync("Oops something went wrong...");
            });
        }
        
    }
}