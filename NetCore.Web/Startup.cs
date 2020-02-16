using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NetCore.MyService;

namespace NetCore.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            //注入异常
            services.AddSingleton(typeof(MyExceptionFilter));

            //services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.AddMvc(option =>
            {
                //注入MyExceptionFilter
               var serviceprovider= services.BuildServiceProvider();
                var filter = serviceprovider.GetService< MyExceptionFilter>();
                option.Filters.Add(filter);
            });
            services.AddMemoryCache();//添加缓冲功能
            services.AddSession();//添加session

            //注入一个类
            //services.AddSingleton(typeof(TestService), new TestService());
            //services.AddSingleton(typeof(TestService));
            //services.AddSingleton(typeof(DataService));
           
            //通过反射注入

            Assembly asm = Assembly.Load(new AssemblyName("MyService"));

            //拿到MyService命名空间(程序集)中实现类,并且该类继承自IServiceSupport类,同时该类不是抽象类.
            var serviceTypes = asm.GetTypes().Where(t => typeof(IServiceSupport).IsAssignableFrom(t) && !t.GetTypeInfo().IsAbstract);
            foreach (var serviceType in serviceTypes)
            {
                //对拿到的所有类进行遍历
                foreach (var infoType in serviceType.GetInterfaces())
                {
                    //拿到该实现类的所有接口,并把每个接口类都注册一个当前实现类
                    //例如:UserService实现类,它的接口类包括:
                    //则实现:services.AddSingleton(IUserService, UserService);
                    services.AddSingleton(infoType, serviceType);
                }
            }
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();
            app.UseSession();//允许session用于程序

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
