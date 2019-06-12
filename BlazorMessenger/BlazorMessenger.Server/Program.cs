using ElectronNET.API;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace BlazorMessenger.Server
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //System.Console.WriteLine(string.Format("args: {0} {1} {2} {3}", args));
            BuildWebHost(args).Run();
            //BuildWebHost2(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseConfiguration(
                    new ConfigurationBuilder()
                        .AddCommandLine(args)
                        .Build()
                )
                .UseElectron(args)
                .UseStartup<Startup>()
                .Build();

        //public static IWebHost BuildWebHost2(string[] args)
        //{
        //    var builder = WebHost.CreateDefaultBuilder(args)
        //        .UseConfiguration(new ConfigurationBuilder()
        //            .AddCommandLine(args)
        //            .Build()
        //        )//;
        //        .UseElectron(args)
        //        .UseStartup<Startup>()
        //        .Build();

        //    return builder;

        //    //if (args.Contains("electron"))
        //    //{
        //    //    builder.UseElectron(args);
        //    //}

        //    //return builder.UseStartup<Startup>()
        //    //    .Build();
        //}
    }
}
