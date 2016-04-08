using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Hosting;
using Microsoft.AspNet.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.PlatformAbstractions;
using NoSql;
using NoSql.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace UnitTests
{
    public class IntegrationTests
    {
        private readonly Action<IApplicationBuilder> _app;
        private readonly Action<IServiceCollection> _services;

        private TestServer CreateTestServer()
        {
            return TestServer.Create(app =>
            {
                var env = app.ApplicationServices.GetRequiredService<IHostingEnvironment>();
                var loggerFactory = app.ApplicationServices.GetRequiredService<ILoggerFactory>();
                new Startup(env).Configure(app, env, loggerFactory);
            }, services =>
            {
                services.AddMvc();
                services.AddScoped<IRepository<dynamic>, DynamicMemoryRepository>();
                services.AddScoped<IIdFactory, IdFactory>();
            });
        }

        [Fact]
        public async void ValuesControllerShouldReturnOkStatuscode()
        {
            var server = CreateTestServer();
            var response = await server.CreateClient().GetAsync("/api/values/");
            Console.WriteLine(response.Content);
            Assert.True(response.IsSuccessStatusCode);
        }
    }
}
