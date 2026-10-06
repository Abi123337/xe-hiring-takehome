using RateAlerts.Api.BackGroundService;
using RateAlerts.Api.Interfaces;
using RateAlerts.Api.Repositories;
using RateAlerts.Api.Services;
using static RateAlerts.Api.Interfaces.IRateXEService;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddSingleton<IAlertRepository, AlertRepository>();
builder.Services.AddScoped<IAlertService, AlertService>();
builder.Services.AddScoped<IXeRateService, XEService>();
builder.Services.AddHostedService<RateCheckPollService>();
var app = builder.Build();

app.MapControllers();

app.Run();
