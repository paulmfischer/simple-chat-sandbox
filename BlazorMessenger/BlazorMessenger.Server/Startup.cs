using BlazorMessenger.Shared;
using ElectronNET.API;
using ElectronNET.API.Entities;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Linq;

namespace BlazorMessenger.Server
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            Console.WriteLine("--------------------------Configure!!");
            services.AddSingleton<IClientService, ClientService>();
            //services.AddSingleton<ClientService>();

            //services.AddScoped<IClientService, ClientService>();
                //services.AddScoped<IClientService>(s =>
                //{
                //    return new ClientService();
                //});

            services.AddMvc().AddNewtonsoftJson();
            services.AddResponseCompression(opts =>
            {
                opts.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(
                    new[] { "application/octet-stream" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseResponseCompression();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBlazorDebugging();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });

            app.UseBlazor<Client.Startup>();

            ElectronBootstrap(env);
        }

        private async void ElectronBootstrap(IWebHostEnvironment env)
        {
            var browserWindow = await Electron.WindowManager.CreateWindowAsync(new BrowserWindowOptions
            {
                Width = 1280,
                Height = 1080,
                WebPreferences = new WebPreferences
                {
                    DevTools = env.IsDevelopment()
                }
            });

            if (env.IsDevelopment())
            {
                browserWindow.WebContents.OpenDevTools();
            }

            browserWindow.SetTitle("Electron.NET Blazor Messenger Sandbox");
        }
    }
}
