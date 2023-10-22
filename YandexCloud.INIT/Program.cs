using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using YandexCloud.BD.Ozon;
using YandexCloud.BD.Postgres;
using YandexCloud.CORE.BL.Managers;
using YandexCloud.CORE.Repositories;
using YandexCloud.CORE.Services;
using YandexCloud.INIT;
using YandexCloud.INIT.Infrastructure;

var builder = Host.CreateApplicationBuilder();
builder.Services.AddTransient<IBlService, BlService>();
builder.Services.AddTransient<IOzonFullData, WebOzonData>();
builder.Services.AddScoped<IUoW, UoW>();
builder.Services.AddTransient<IRequestReader, RequestReader>();
builder.Services.AddTransient<IConsoleReader, ConsoleReader>();
builder.Services.AddTransient<IOzonManager, OzonManager>();

builder.Services.AddHttpClient();
builder.Services.AddLogging(l => l.SetMinimumLevel(LogLevel.Error));
using IHost host = builder.Build();

var reader = host.Services.GetService<IRequestReader>();
var requestModel = reader.Read();

var baseCl = host.Services.GetService<IBlService>();

baseCl.OzonEventHandler += (message => Console.WriteLine(message));
var dataGettingTask = baseCl.GetDataAsync(requestModel);
Console.ReadLine();

await dataGettingTask;
baseCl.OzonEventHandler -= (message => Console.WriteLine(message));

await host.RunAsync();
