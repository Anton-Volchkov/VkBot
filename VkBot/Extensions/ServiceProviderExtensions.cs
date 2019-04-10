using Microsoft.Extensions.DependencyInjection;
using VkBot.Bot;
using VkBot.Bot.Commands;
using VkBot.Data.Abstractions;

namespace VkBot
{
    public static class ServiceProviderExtensions
    {
        public static void AddBotFeatures(this IServiceCollection services)
        {
            services.AddScoped<CommandExecutor>();
            services.AddScoped<IBotCommand, Help>();
            services.AddScoped<IBotCommand, Roulette>();
            services.AddScoped<IBotCommand, SetCommon>();
            services.AddScoped<IBotCommand, GetCommon>();
            services.AddScoped<IBotCommand, Random>();
            services.AddScoped<IBotCommand, Bell>();
            services.AddScoped<IBotCommand, BicepsMetr>();
            services.AddScoped<IBotCommand, GetWeather>();
            services.AddScoped<IBotCommand, Calculator>();
            services.AddScoped<IBotCommand, Love>();
            services.AddScoped<IBotCommand, CurrencyRate>();
            services.AddScoped<IBotCommand, Bot.Commands.CurrencyConverter>();
            services.AddScoped<IBotCommand, GetMemory>();
            services.AddScoped<IBotCommand, GiveMemory>();
            services.AddScoped<IBotCommand, DeleteMemory>();
        }
    }
}