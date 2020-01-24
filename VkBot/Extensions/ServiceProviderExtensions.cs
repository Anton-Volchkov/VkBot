using Microsoft.Extensions.DependencyInjection;
using VkBot.Bot;
using VkBot.Bot.Commands;
using VkBot.Bot.Commands.CommandsByRoles.AdminCommands;
using VkBot.Bot.Commands.CommandsByRoles.EditorCommands;
using VkBot.Bot.Commands.CommandsByRoles.ModerCommands;
using VkBot.Data.Abstractions;

namespace VkBot.Extensions
{
    public static class ServiceProviderExtensions
    {
        public static void AddBotFeatures(this IServiceCollection services)
        {
            services.AddScoped<CommandExecutor>();
            services.AddScoped<RolesHandler>();

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
            services.AddScoped<IBotCommand, SetMemory>();
            services.AddScoped<IBotCommand, DeleteMemory>();
            services.AddScoped<IBotCommand, MailingWeather>();
            services.AddScoped<IBotCommand, WikiPedia>();
            services.AddScoped<IBotCommand, Translate>();
            services.AddScoped<IBotCommand, SetGroup>();
            services.AddScoped<IBotCommand, SetTimeTable>();
            services.AddScoped<IBotCommand, GetSchedule>();
            services.AddScoped<IBotCommand, Kick>();
            services.AddScoped<IBotCommand, SetStatus>();
            services.AddScoped<IBotCommand, SetRole>();
            services.AddScoped<IBotCommand, Statistics>();
            services.AddScoped<IBotCommand, GetRights>();
            services.AddScoped<IBotCommand, CheckOnlineUser>();
            services.AddScoped<IBotCommand, CallEveryone>();
            services.AddScoped<IBotCommand, GetTopUsersInChat>();
            services.AddScoped<Kick>();
            services.AddScoped<IBotCommand, Rebuke>();
            services.AddScoped<Amnesty>();

            services.AddScoped<IInfo, Info>();
        }
    }
}