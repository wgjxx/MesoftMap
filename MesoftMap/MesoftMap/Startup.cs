using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mesoft.MapAPP.Interface;
using Mesoft.MapAPP.Services;
using MesoftMap.DataContext;
using MesoftMap.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace MesoftMap
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
            services.AddSession();
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.LoginPath = new PathString("/home/Login");
                    options.AccessDeniedPath = new PathString("/Home/Privacy");

                });//��cookie�ķ�ʽ��֤��˳���ʼ����¼��ַ
            services.AddControllersWithViews()
                 .AddRazorRuntimeCompilation();   //����֧�ֶ�̬���룬�޸�cshtml�ļ���̬����;

            services.AddEntityFrameworkSqlServer()
                .AddDbContext<MeEPADBContext>(options =>
                  options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddControllers().AddNewtonsoftJson(options =>
            {
                options.UseCamelCasing(true);//����˷��ص�JSON���ݴ��ڴ�Сд������,��Сд����
                //options.SerializerSettings.ContractResolver =  new CamelCasePropertyNamesContractResolver(); 
            });

            services.AddTransient<IMapService, MapService>();
            services.AddTransient<IOperatorService, OperatorService>();
            services.Configure<MapDisplayOption>(Configuration.GetSection("MapDisplayOptions"));  //���õ�ͼ��ʾ����
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseSession();
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();
            app.UseAuthentication();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Login}");
            });
        }
    }
}
