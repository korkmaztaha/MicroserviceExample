using MassTransit;
using MongoDB.Driver;
using Shared;
using Stock.API.Consumers;
using Stock.API.Models.Entities;
using Stock.API.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddMassTransit(configurator =>
{
    configurator.AddConsumer<OrderCreatedEventConsumer>();
    configurator.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host(builder.Configuration["RabbitMQ"]);
        cfg.ReceiveEndpoint(RabbitMQSettings.Stock_OrderCreatedEventQueue, e =>
        {
            e.ConfigureConsumer<OrderCreatedEventConsumer>(context);
        });
    });
});

builder.Services.AddSingleton<MongoDbService>(); 


#region MongoDB seed veri ekleme

using IServiceScope scope = builder.Services.BuildServiceProvider().CreateScope();
MongoDbService mongoDbService = scope.ServiceProvider.GetService<MongoDbService>();
var collection = mongoDbService.GetCollection<Stock.API.Models.Entities.Stock>();

var data = collection.Find(_ => true);  
if (!await data.AnyAsync())
{
    await collection.InsertOneAsync(new() { ProductId = Guid.NewGuid(), Count = 4000 });
    await collection.InsertOneAsync(new() { ProductId = Guid.NewGuid(), Count = 5000 });
    await collection.InsertOneAsync(new() { ProductId = Guid.NewGuid(), Count = 6000 });
    await collection.InsertOneAsync(new() { ProductId = Guid.NewGuid(), Count = 1000 });
}
#endregion

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
