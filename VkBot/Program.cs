using System.Reflection;
using Hangfire;
using Hangfire.MemoryStorage;
using ImageFinder;
using Microsoft.EntityFrameworkCore;
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

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;

var services = builder.Services;


if (builder.Environment.IsDevelopment()) configuration.AddUserSecrets(Assembly.GetExecutingAssembly());

var connectionString = configuration.GetConnectionString("DefaultConnection");
services.AddDbContext<MainContext>(options => options.UseNpgsql(connectionString));

services.AddSingleton<IVkApi>(sp =>
{
    var api = new VkApi();
    api.Authorize(new ApiAuthParams { AccessToken = configuration["Config:AccessToken"] });
    return api;
});

services.AddHostedService<MigrationHostedService>();

services.AddSingleton(x => new WeatherInfo(configuration["Config:OWM_Token"]));

services.AddSingleton(x => new Translator(configuration["Config:YT_Token"]));

services.AddBotFeatures();

services.AddSingleton(sp => new ImageProvider(sp.GetService<ProxyProvider>()));

services.AddHangfire(config => { config.UseMemoryStorage(); });

services.AddHangfireServer(x => { x.WorkerCount = Environment.ProcessorCount * 2; });

services.AddControllers()
    .AddNewtonsoftJson();

var app = builder.Build();

app.UseHangfireDashboard();

if (builder.Environment.IsDevelopment()) app.UseDeveloperExceptionPage();

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthorization();

app.MapControllers();


BackgroundJob.Enqueue<ScheduledTask>(x => x.InitJobs());

await app.RunAsync();