using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using DynamicApi.Web.Repositories;
using DynamicApi.Web.Controllers;
using Microsoft.AspNet.Mvc.Controllers;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System.IO;
using System.Linq;
using Microsoft.CodeAnalysis.Emit;
using System.Collections.Generic;
using System;
using System.Reflection;
using System.Reflection.Metadata;
using Microsoft.AspNet.Mvc;

namespace DynamicApi.Web
{
    public class Startup
    {
        public unsafe static void DoMyDynamicCompile()
        {
            SyntaxTree syntaxTree = CSharpSyntaxTree.ParseText(
                "using System.Collections.Generic; " +
                "using Microsoft.AspNet.Mvc; " +
                "using DynamicApi.Web.Models; " +
                "using DynamicApi.Web.Repositories; " +
                "namespace DynamicApi.Web.Controllers {" +
                "[Route(\"api /[controller]\")] " +
                "public class ValuesController : Controller<Values> { public ValuesController(IRepository<Values> repository) : base(repository) { }}}");

            string assemblyName = Path.GetRandomFileName();

            byte* b;
            int length;
            MetadataReference reference = null;
            var assembly = typeof(Controller<>).GetTypeInfo().Assembly; //let's grab the Controller in-memory assembly
            if (assembly.TryGetRawMetadata(out b, out length))
            {
                var moduleMetadata = ModuleMetadata.CreateFromMetadata((IntPtr)b, length);
                var assemblyMetadata = AssemblyMetadata.Create(moduleMetadata);
                reference = assemblyMetadata.GetReference();
            }

            MetadataReference[] references = new MetadataReference[]
           {
                MetadataReference.CreateFromFile(typeof(object).GetTypeInfo().Assembly.Location),
                MetadataReference.CreateFromFile(typeof(Attribute).GetTypeInfo().Assembly.Location),
                MetadataReference.CreateFromFile(typeof(Controller).GetTypeInfo().Assembly.Location),
                MetadataReference.CreateFromFile(typeof(RouteAttribute).GetTypeInfo().Assembly.Location),
                reference
           };

            var compilation = CSharpCompilation.Create(
                assemblyName,
                syntaxTrees: new[] { syntaxTree },
                references: references,
                options: new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));

            using (var ms = new MemoryStream())
            {
                EmitResult result = compilation.Emit(ms);

                if (!result.Success)
                {
                    IEnumerable<Diagnostic> failures = result.Diagnostics.Where(diagnostic =>
                        diagnostic.IsWarningAsError ||
                        diagnostic.Severity == DiagnosticSeverity.Error);

                    foreach (Diagnostic diagnostic in failures)
                    {
                        Console.Error.WriteLine("{0}: {1}", diagnostic.Id, diagnostic.GetMessage());
                    }
                }
                else
                {
                    ms.Seek(0, SeekOrigin.Begin);
                    Assembly.Load(new AssemblyName(assemblyName));
                    //var myAssembly = Assembly.Load(ms.ToArray());
                }
            }
        }

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
            DoMyDynamicCompile();

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