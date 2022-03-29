using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using nat.Manager.Centers;
using nat.Manager.Customers;
using nat.Manager.Trainers;
using nat.Services;
using nat.Storage.Migrations;

namespace nat
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        private IConfigurationRoot _configurationRoot;
        public Startup(IWebHostEnvironment hostEnvironment)
        {
            _configurationRoot = new ConfigurationBuilder().SetBasePath(hostEnvironment.ContentRootPath).AddJsonFile("FitnessCenterDbSetting.json").Build();
            // Интерфейс IWebHostEnvironment - Предоставляет информацию о среде веб-хостинга, в которой запущено приложение.
            // ContentRootPath: возвращает путь к корневой папке приложения
            // Интерфейс IConfigurationBuilder - представляет тип, используемый для построения конфигурации приложения.
            // Метод SetBasePath() - Задает FileProvider для поставщиков на основе файлов равным PhysicalFileProvider с базовым путем.
            // Метод AddJsonFile() - Добавляет источник конфигурации JSON в builder.
            // Метод Buid() - Создает IConfiguration с ключами и значениями на основе набора источников, зарегистрированных в Sources.
        }
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<CenterDataContext>(options => options.UseSqlServer(_configurationRoot.GetConnectionString("FitnessCenterDb")));
            services.AddMvc(option => option.EnableEndpointRouting = false);
            services.AddTransient<ICenterManager, CenterManager>();
            services.AddTransient<ICustomerManager, CustomersManager>();
            services.AddTransient<ITrainerManager, TrainerManager>();
            services.AddTransient<IConvertToExcel, ConvertToExcel>();
            services.AddTransient<ICryptographyService, CryptographyService>();

            services.AddTransient<ISettingEDSFileService, SettingEDSFileService>();
            services.AddTransient<IPortInfoService, PortInfoService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseDeveloperExceptionPage();
            app.UseStatusCodePages();
            app.UseRequestLocalization();
            app.UseStaticFiles();
            app.UseMvcWithDefaultRoute();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Center}/{action=Index}/{id?}/{TrainerId?}/{CustomerId?}");
            });
        }
    }
}
