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

namespace VkBot
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            var culture = new CultureInfo("ru-RU");
            CultureInfo.DefaultThreadCurrentCulture = culture;
            CultureInfo.DefaultThreadCurrentUICulture = culture;

            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IVkApi>(sp =>
            {
                var api = new VkApi();
                api.Authorize(new ApiAuthParams { AccessToken = Configuration["Config:AccessToken"] });
                return api;
            });
            services.AddSingleton(x => new WeatherInfo(Configuration["Config:OWM_Token"]));
            services.AddSingleton<CurrencyInfo>();

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
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

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