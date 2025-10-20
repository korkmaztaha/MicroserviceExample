using MassTransit;
using Stock.API.Services;

var builder = WebApplication.CreateBuilder(args);



builder.Services.AddMassTransit(x =>
{
    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host(builder.Configuration["RabbitMQ"]);
    });
});

builder.Services.AddSingleton<MongoDBService>();
var app = builder.Build();


app.Run();
