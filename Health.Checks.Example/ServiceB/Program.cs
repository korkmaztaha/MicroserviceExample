using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using MongoDB.Driver;

var builder = WebApplication.CreateBuilder(args);

var mongoConn = builder.Configuration.GetConnectionString("MongoDB");
var redisConn = builder.Configuration.GetConnectionString("Redis");
var postgresConn = builder.Configuration.GetConnectionString("Postgres");

builder.Services.AddSingleton<IMongoClient>(sp =>
    new MongoClient(mongoConn)
);

builder.Services.AddHealthChecks()
    .AddRedis(
        redisConnectionString: redisConn,
        name: "Redis Check",
        failureStatus: HealthStatus.Degraded,
        tags: new string[] { "redis" }
    )
    .AddMongoDb(
        sp => sp.GetRequiredService<IMongoClient>().GetDatabase("testDb"),
        name: "MongoDB Check",
        failureStatus: HealthStatus.Degraded,
        tags: new string[] { "mongodb", "nosql", "db" }
    );
   

var app = builder.Build();

//görselleþtime  için  using HealthChecks.UI.Client;  eklenmeli
app.UseHealthChecks("/health", new HealthCheckOptions
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});


app.Run();