using CoronaVirus;
using CurrencyConverter;
using Microsoft.Extensions.DependencyInjection;
using VkBot.Bot;
using VkBot.Bot.Commands;
using VkBot.Data.Abstractions;
using VkBot.PreProcessors.Abstractions;
using VkBot.Proxy.Logic;
using WikipediaApi;

namespace VkBot.Extensions
{
    public static class ServiceProviderExtensions
    {
        public static void AddBotFeatures(this IServiceCollection services)
        {
            services.AddSingleton<CurrencyInfo>();
            services.AddSingleton<WikiApi>();
            services.AddSingleton<ProxyProvider>();
           

            services.AddScoped<CommandExecutor>();
            services.AddScoped<RolesHandler>();

            services.AddScoped<CoronaInfo>();

            services.Scan(scan =>
            {
                scan.FromAssembliesOf(typeof(Help))
                    .AddClasses(classes => classes.AssignableTo(typeof(IBotCommand)).NotInNamespaceOf<Info>())
                    .AsImplementedInterfaces()
                    .WithScopedLifetime();
            });


            services.Scan(scan =>
            {
                scan.FromAssembliesOf(typeof(Help))
                    .AddClasses(classes => classes.AssignableTo(typeof(ICommandPreprocessor)))
                    .AsImplementedInterfaces()
                    .WithScopedLifetime();
            });

            services.AddScoped<IInfo, Info>();


            services.AddScoped<ICommandHandler, CommandHandler>();

            //services.AddScoped<IBotCommand, SetTimeTable>();
            //services.AddScoped<IBotCommand, GetSchedule>();
            
        }
    }
}