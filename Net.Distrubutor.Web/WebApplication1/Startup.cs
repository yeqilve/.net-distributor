using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WebApplication1.Models;
using Autofac.Extensions.DependencyInjection;
namespace WebApplication1
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            this.Configuration = builder.Build();
        }

        public IContainer ApplicationContainer { get; private set; }

        public IConfiguration Configuration { get; private set; }

        // ConfigureServices is where you register dependencies. This gets
        // called by the runtime before the Configure method, below.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            // Add services to the collection.
            services.AddMvc();

            // Create the container builder.
            var builder = new ContainerBuilder();

            builder.Populate(services);
            builder.RegisterType<Test>().As<ITest1>().InstancePerLifetimeScope();
            builder.RegisterType<Test2>().As<ITest2>().InstancePerLifetimeScope();
            this.ApplicationContainer = builder.Build();

            // Create the IServiceProvider based on the container.
            return new AutofacServiceProvider(this.ApplicationContainer);
        }

        // Configure is where you add middleware. This is called after
        // ConfigureServices. You can use IApplicationBuilder.ApplicationServices
        // here if you need to resolve things from the container.
        public void Configure(IApplicationBuilder app, IApplicationLifetime appLifetime)
        {
            app.UseMvc(
                route =>
                {
                    route.MapRoute("Default", "{controller}/{action}/{id?}");
                }
            );

            // If you want to dispose of resources that have been resolved in the
            // application container, register for the "ApplicationStopped" event.
            // You can only do this if you have a direct reference to the container,
            // so it won't work with the above ConfigureContainer mechanism.
            appLifetime.ApplicationStopped.Register(() => this.ApplicationContainer.Dispose());
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        //public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        //{
        //    if (env.IsDevelopment())
        //    {
        //        app.UseBrowserLink();
        //        app.UseDeveloperExceptionPage();
        //    }
        //    else
        //    {
        //        app.UseExceptionHandler("/Home/Error");
        //    }

        //    app.UseStaticFiles();

        //    app.UseMvc( route
        //        => route.MapLocalizedRoute("Test","Index",new { controller="Home", action = "Index"}));
        //}
        public interface ITest1
        {
            void Excute();
        }

        public class Test : ITest1
        {
            private string test = "test2222";

            public void Excute()
            {
                Console.WriteLine("OK");
                Console.Read();
            }
        }

        public interface ITest2
        {
            void Test();
        }
        public class Test2 : ITest2
        {
            private ITest1 test1;
            public Test2(ITest1 test)
            {
                test1 = test;
            }
            public void Test() => Console.WriteLine($"{test1.GetType()}resolved{test1.ToString()}");
        }
    }
}
