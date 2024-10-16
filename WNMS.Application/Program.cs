using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog.Web;
using WNMS.Utility.Log;

namespace WNMS.Application
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
            //CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
            }).UseServiceProviderFactory(new AutofacServiceProviderFactory()).AddNlogService();


        //public static IHostBuilder CreateHostBuilder1(string[] args)
        //{
        //    var config = new ConfigurationBuilder()
        //                .SetBasePath(Directory.GetCurrentDirectory())
        //                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)//
        //                .AddCommandLine(args)
        //                .Build();
        //    return Host.CreateDefaultBuilder(args)
        //        .UseServiceProviderFactory(new AutofacServiceProviderFactory()).AddNlogService()
        //        .ConfigureWebHostDefaults(webBuilder =>
        //        {
        //            webBuilder.UseKestrel(option =>
        //            {
        //                //option.Listen(System.Net.IPAddress.Any, 10014, (lop) =>
        //                option.Listen(System.Net.IPAddress.Any, 5000, (lop) =>
        //                {
        //                    lop.UseHttps("wnms.pfx", "12345678");

        //                });
        //            });
        //            webBuilder.UseConfiguration(config);
        //            webBuilder.UseIISIntegration();
        //            //webBuilder.UseUrls("https://47.104.187.1:10014");
        //            webBuilder.UseUrls("https://localhost:5000");
        //            //webBuilder.UseUrls("https://127.0.0.1:81");
        //            //���Բ�ͨ�������ļ���ͨ��UseUrls�÷���ָ��һ��url�Էֺŷָ�
        //            webBuilder.UseStartup<Startup>();
        //        })
        //        //for autofac ��Ĭ��ServiceProviderFactoryָ��ΪAutofacServiceProviderFactory
        //        .UseServiceProviderFactory(new AutofacServiceProviderFactory())
        //        // for aspcectcore //��Ҫ��������AspectCore.Extensions.DependencyInjection;
        //        //.UseServiceProviderFactory(new AspectCoreServiceProviderFactory())
        //        ;
        //}
    }
}
