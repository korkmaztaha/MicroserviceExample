using System.Net.Http.Headers;

var builder = WebApplication.CreateBuilder(args);

var MSSQLConn = builder.Configuration.GetConnectionString("MSSQL");


var serviceA = builder.Configuration["HealthCheckEndpoints:ServiceA"];
var serviceB = builder.Configuration["HealthCheckEndpoints:ServiceB"];

builder.Services.AddHealthChecksUI(settings =>
{
    settings.AddHealthCheckEndpoint("Service A", serviceA);
    settings.AddHealthCheckEndpoint("Service B", serviceB);

    //defult kontrol 10 saniyede bu þekilde deðiþtirilebilir
    settings.SetEvaluationTimeInSeconds(3);
    settings.SetApiMaxActiveRequests(3);

    settings.ConfigureApiEndpointHttpclient((sp, client) =>
    {
        client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", "....");
    });

    settings.ConfigureWebhooksEndpointHttpclient((sp, client) =>
    {
        client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", "....");
    });
})
.AddSqlServerStorage(MSSQLConn);

var app = builder.Build();

app.UseHealthChecksUI(options =>
{
    options.UIPath = "/health-ui";
   
});

app.Run();
