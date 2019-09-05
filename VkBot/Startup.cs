using System;
using System.Globalization;
using CurrencyConverter;
using Hangfire;
using Hangfire.MemoryStorage;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OpenWeatherMap;
using VkBot.Bot.Help;
using VkBot.Data.Models;
using VkNet;
using VkNet.Abstractions;
using VkNet.Model;
using WikipediaApi;
using YandexTranslator;

namespace VkBot
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var culture = new CultureInfo("ru-RU");
            CultureInfo.DefaultThreadCurrentCulture = culture;
            CultureInfo.DefaultThreadCurrentUICulture = culture;

            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", false, true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", true)
                .AddEnvironmentVariables();
            if (env.IsDevelopment()) builder.AddUserSecrets<Startup>();

            Configuration = builder.Build();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IVkApi>(sp =>
            {
                var api = new VkApi();
                api.Authorize(new ApiAuthParams { AccessToken = Configuration["Config:AccessToken"] });
                api.RequestsPerSecond = 20;
                return api;
            });

            services.AddSingleton(x => new WeatherInfo(Configuration["Config:OWM_Token"]));

            services.AddSingleton(x => new Translator(Configuration["Config:YT_Token"]));

            services.AddSingleton<CurrencyInfo>();

            services.AddSingleton<WikiApi>();

            var connectionString = Configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<MainContext>(options => options.UseNpgsql(connectionString));

            services.AddBotFeatures();

            services.AddHangfire(config => { config.UseMemoryStorage(); });

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            var options = new BackgroundJobServerOptions { WorkerCount = Environment.ProcessorCount * 2 };
            app.UseHangfireServer(options);

            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();
            else
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();

            app.UseHttpsRedirection();
            app.UseMvc();

            ConfigureJobs();
        }

        private void ConfigureJobs()
        {
            BackgroundJob.Enqueue<ScheduledTask>(x => x.Dummy()); //TODO:
        }
    }
}