using Coordinator.Models.Contexts;
using Coordinator.Services;
using Coordinator.Services.Abstractions;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<TwoPhaseCommitContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("SQLServer")));



//Coordinator un iletiþime geçeceði servislerin base adresleri Merkezi, db veya bir serviceden çekilerek yapýlabilir.
builder.Services.AddHttpClient("OrderAPI", client => client.BaseAddress = new("https://localhost:7021/"));
builder.Services.AddHttpClient("StockAPI", client => client.BaseAddress = new("https://localhost:7172/"));
builder.Services.AddHttpClient("PaymentAPI", client => client.BaseAddress = new("https://localhost:7116/"));


builder.Services.AddTransient<ITransactionService, TransactionService>();
var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/create-order-transaction", async (ITransactionService transactionService) =>
{
    //1. Adým: prapere
    var transactionId = await transactionService.CreateTransactionAsync();
    await transactionService.PrepareServicesAsync(transactionId);
    bool transactionState = await transactionService.CheckReadyServicesAsync(transactionId);

    if (transactionState)
    {
        //2.adým
        await transactionService.CommitAsync(transactionId);
        transactionState = await transactionService.CheckTransactionStateServiceAsync(transactionId);
    }

    if (!transactionState)
        await transactionService.RollbackAsync(transactionId);
});


app.Run();
