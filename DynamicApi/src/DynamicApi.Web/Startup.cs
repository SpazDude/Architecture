using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using DynamicApi.Web.Repositories;
using DynamicApi.Web.Controllers;
using Microsoft.AspNet.Mvc.Controllers;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace DynamicApi.Web
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            // Set up configuration sources.
            var builder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // http://www.strathweb.com/2015/04/asp-net-mvc-6-discovers-controllers/ 
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddMvc();

            //services.AddMvc().AddControllersAsServices(new[] { typeof(Controller<>) });
            //services.Replace(ServiceDescriptor.Singleton<IControllerFactory, GenericControllerFactory>());
            //services.Replace(ServiceDescriptor.Transient<IControllerActivator, GenericControllerActivator>());
            services.AddSingleton(typeof(IRepository<>), typeof(Repository<>));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            app.UseIISPlatformHandler();

            app.UseStaticFiles();

            app.UseMvc();
        }

        // Entry point for the application.
        public static void Main(string[] args) => WebApplication.Run<Startup>(args);
    }
}