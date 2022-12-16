using Application;
using Application.Commands;
using Application.Commands.Abstractions;
using Application.PreProcessors.Abstractions;
using CoronaVirus;
using CurrencyConverter;
using Scrutor;
using Services.Helpers;
using VkBot.Proxy.Logic;
using WikipediaApi;

namespace VkBot.Extensions;

public static class ServiceProviderExtensions
{
    public static void AddBotFeatures(this IServiceCollection services)
    {
        services.AddSingleton<CurrencyInfo>();
        services.AddSingleton<WikiApi>();
        services.AddSingleton<ProxyProvider>();

        services.AddScoped<ICommandExecutor, CommandExecutor>();
        services.AddScoped<IRolesHelper, RolesHelper>();
        services.AddScoped<CoronaInfo>();

        services.Scan(scan =>
        {
            scan.FromAssembliesOf(typeof(Help))
                .AddClasses(classes =>
                    classes.AssignableTo(typeof(IBotCommand)).Where(x => x.FullName != typeof(Info).FullName))
                .UsingRegistrationStrategy(RegistrationStrategy.Append)
                .AsImplementedInterfaces()
                .WithScopedLifetime();
        });

        services.Scan(scan =>
        {
            scan.FromAssembliesOf(typeof(Help))
                .AddClasses(classes => classes.AssignableTo(typeof(ICommandPreprocessor)))
                .UsingRegistrationStrategy(RegistrationStrategy.Append)
                .AsImplementedInterfaces()
                .WithScopedLifetime();
        });

        services.AddScoped<IInfo, Info>();

        services.AddScoped<ICommandHandler, CommandHandler>();
    }
}