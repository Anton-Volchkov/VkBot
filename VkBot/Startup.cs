using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using VkBot.Bot;
using VkBot.Bot.Commands;
using VkBot.Data.Abstractions;
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

            var connectionString = Configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<MainContext>(options => options.UseNpgsql(connectionString));

            services.AddScoped<CommandExecutor>();
            services.AddScoped<IBotCommand, Help>();
            services.AddScoped<IBotCommand, Roulette>();
            services.AddScoped<IBotCommand, BotTimeTable>();
            services.AddScoped<IBotCommand, BotGetTimeTable>();
            services.AddScoped<IBotCommand, Random>();
            services.AddScoped<IBotCommand, Bell>();
            services.AddScoped<IBotCommand, BicepsMetr>();
            services.AddScoped<IBotCommand, GetWeather>();
            services.AddScoped<IBotCommand, Calculator>();
            services.AddScoped<IBotCommand, Love>();
            services.AddScoped<IBotCommand, Valute>();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if(env.IsDevelopment())
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
        }
    }
}