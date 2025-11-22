using Product.Event.Handler.Service.Services;
using Shared.Services;
using Shared.Services.Abstractions;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddHostedService<EventStoreBackgroundService>();

builder.Services.AddSingleton<IEventStoreService, EventStoreService>();

var connectionString = builder.Configuration.GetConnectionString("MongoDB") ?? "mongodb://localhost:27017";
var databaseName = builder.Configuration.GetValue<string>("MongoDatabaseName") ?? "ProductDB";
builder.Services.AddSingleton<IMongoDBService>(sp => new MongoDBService(connectionString, databaseName));

var host = builder.Build();
host.Run();