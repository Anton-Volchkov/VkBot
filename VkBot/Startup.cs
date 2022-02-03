using System;
using System.Globalization;
using Hangfire;
using Hangfire.MemoryStorage;
using ImageFinder;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OpenWeatherMap;
using VkBot.Bot.Hangfire;
using VkBot.Domain;
using VkBot.Extensions;
using VkBot.HostedServices;
using VkBot.Proxy.Logic;
using VkNet;
using VkNet.Abstractions;
using VkNet.Model;
using YandexTranslator;

namespace VkBot
{
    public class Startup
    {
        public Startup(IWebHostEnvironment env)
        {
            var culture = new CultureInfo("ru-RU");
            CultureInfo.DefaultThreadCurrentCulture = culture;
            CultureInfo.DefaultThreadCurrentUICulture = culture;

            var builder = new ConfigurationBuilder()
                          .SetBasePath(env.ContentRootPath)
                          .AddJsonFile("appsettings.json", false, true)
                          .AddJsonFile($"appsettings.{env.EnvironmentName}.json", true)
                          .AddEnvironmentVariables();
            if(env.IsDevelopment())
            {
                builder.AddUserSecrets<Startup>();
            }

            Configuration = builder.Build();
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            var connectionString = Configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<MainContext>(options => options.UseNpgsql(connectionString));

            services.AddSingleton<IVkApi>(sp =>
            {
                var api = new VkApi();
                api.Authorize(new ApiAuthParams { AccessToken = Configuration["Config:AccessToken"] });
                return api;
            });

            services.AddHostedService<MigrationHostedService>();

            services.AddSingleton(x => new WeatherInfo(Configuration["Config:OWM_Token"]));

            services.AddSingleton(x => new Translator(Configuration["Config:YT_Token"]));

            services.AddBotFeatures();

            services.AddSingleton(sp =>
            {
                return new ImageProvider(Configuration["Config:PathToChromeDriver"],sp.GetService<ProxyProvider>() );
            });

            services.AddHangfire(config => { config.UseMemoryStorage(); });
            
            services.AddControllers()
                    .AddNewtonsoftJson();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            var options = new BackgroundJobServerOptions { WorkerCount = Environment.ProcessorCount * 2 };
            app.UseHangfireServer(options);

            if(env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            ConfigureJobs();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }

        private void ConfigureJobs()
        {
            BackgroundJob.Enqueue<ScheduledTask>(x => x.InitJobs()); //TODO:
        }
    }
}