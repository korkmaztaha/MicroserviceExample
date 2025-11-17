using EventStore.Client;
using System.Text.Json; 

EventStoreService eventStoreService = new();


AccountCreatedEvent accountCreatedEvent = new()
{
    AccountId = "12345",      
    CostumerId = "98765",    
    StartBalance = 0,       
    Date = DateTime.UtcNow.Date
};


MoneyDepositedEvent moneyDepositedEvent1 = new()
{
    AccountId = "12345",
    Amount = 1000,
    Date = DateTime.UtcNow.Date
};
MoneyDepositedEvent moneyDepositedEvent2 = new()
{
    AccountId = "12345",
    Amount = 500,
    Date = DateTime.UtcNow.Date
};


MoneyWithdrawnEvent moneyWithdrawnEvent = new()
{
    AccountId = "12345",
    Amount = 200,
    Date = DateTime.UtcNow.Date
};

MoneyDepositedEvent moneyDepositedEvent3 = new()
{
    AccountId = "12345",
    Amount = 50,
    Date = DateTime.UtcNow.Date
};

MoneyTransferredEvent moneyTransferredEvent1 = new()
{
    AccountId = "12345",
    Amount = 250,
    Date = DateTime.UtcNow.Date
};
MoneyTransferredEvent moneyTransferredEvent2 = new()
{
    AccountId = "12345",
    Amount = 150,
    Date = DateTime.UtcNow.Date
};


MoneyDepositedEvent moneyDepositedEvent4 = new()
{
    AccountId = "12345",
    Amount = 2000,
    Date = DateTime.UtcNow.Date
};



await eventStoreService.AppendToStreamAsync(
    streamName: $"costumer-{accountCreatedEvent.CostumerId}-stream", // Stream ismi (müşteriye özel)
    new[] {
        eventStoreService.GenerateEventData(accountCreatedEvent), // Her eventi EventData formatına çevir
        eventStoreService.GenerateEventData(moneyDepositedEvent1),
        eventStoreService.GenerateEventData(moneyDepositedEvent2),
        eventStoreService.GenerateEventData(moneyWithdrawnEvent),
        eventStoreService.GenerateEventData(moneyDepositedEvent3),
        eventStoreService.GenerateEventData(moneyTransferredEvent1),
        eventStoreService.GenerateEventData(moneyTransferredEvent2),
        eventStoreService.GenerateEventData(moneyDepositedEvent4)
    }
);



// Hesap bakiyesini tutacak nesne
BalanceInfo balanceInfo = new();

// Stream'i baştan okuyacak ve yeni eventler geldiğinde callback çalışacak
await eventStoreService.SubscribeToStreamAsync(
    streamName: $"costumer-{accountCreatedEvent.CostumerId}-stream", // Aynı stream
    async (ss, re, ct) => // Her event geldiğinde çalışacak fonksiyon
    {
        string eventType = re.Event.EventType; // Event tipini string olarak al
        // EventStore'dan gelen JSON byte dizisini C# objesine çevir
        object @event = JsonSerializer.Deserialize(
            re.Event.Data.ToArray(),
            Type.GetType(eventType)
        );

       
        switch (@event)
        {
            case AccountCreatedEvent e: 
                balanceInfo.AccountId = e.AccountId;
                balanceInfo.Balance = e.StartBalance;
                break;

            case MoneyDepositedEvent e: 
                balanceInfo.Balance += e.Amount;
                break;

            case MoneyWithdrawnEvent e:
                balanceInfo.Balance -= e.Amount;
                break;

            case MoneyTransferredEvent e: 
                balanceInfo.Balance -= e.Amount;
                break;
        }

        
        await Console.Out.WriteLineAsync("*******Balance*******");
        await Console.Out.WriteLineAsync(JsonSerializer.Serialize(balanceInfo));
        await Console.Out.WriteLineAsync("*******Balance*******");
        await Console.Out.WriteLineAsync("");
        await Console.Out.WriteLineAsync("");
    }
);

Console.Read();

class EventStoreService
{

    EventStoreClientSettings GetEventStoreClientSettings(string connectionString = "esdb://admin:changeit@localhost:2113?tls=false&tlsVerifyCert=false")
        => EventStoreClientSettings.Create(connectionString);

    // EventStore client nesnesi, her çağrıldığında yeni bağlantı nesnesi döner
    EventStoreClient Client { get => new EventStoreClient(GetEventStoreClientSettings()); }

    public async Task AppendToStreamAsync(string streamName, IEnumerable<EventData> eventData)
        => await Client.AppendToStreamAsync(
            streamName: streamName,        // Eventlerin yazılacağı stream adı
            eventData: eventData,          // Stream'e yazılacak EventData listesi
            expectedState: StreamState.Any // Beklenen stream durumu
                                           // StreamState.Any -> stream durumu fark etmez, concurrency kontrolü yok
                                           // StreamState.NoStream -> stream yoksa yaz, varsa hata verir
                                           // StreamState.StreamExists -> stream varsa yaz, yoksa hata verir
        );

    // Event objesini EventData formatına çevirir
    public EventData GenerateEventData(object @event)
        => new(
            eventId: Uuid.NewUuid(),                     
            type: @event.GetType().Name,                  
            data: JsonSerializer.SerializeToUtf8Bytes(@event) // Event içeriğini JSON byte[] formatına çevir
        );


    public async Task SubscribeToStreamAsync(
        string streamName,
        Func<StreamSubscription, ResolvedEvent, CancellationToken, Task> eventAppeared)
        => Client.SubscribeToStreamAsync(
            streamName: streamName,  // Abone olunacak stream
            start: FromStream.Start, // Nereden başlayacağı
                                     // FromStream.Start -> stream başından oku
                                     // FromStream.End   -> sadece yeni gelen eventleri dinle
                                     // FromStream.Revision(n) -> belirli bir revision'dan başla
            eventAppeared: eventAppeared, // Her event geldiğinde tetiklenecek callback fonksiyonu
                                          // Parametreler:
                                          // StreamSubscription -> aboneliğin kendisi
                                          // ResolvedEvent      -> gelen event verisi
                                          // CancellationToken -> iptal durumunu kontrol için
            subscriptionDropped: (x, y, z) => Console.WriteLine("Disconnected!") // Abonelik koptuğunda çalışır
        );
}


class BalanceInfo
{
    public string AccountId { get; set; }
    public int Balance { get; set; }
}

class AccountCreatedEvent
{
    public string AccountId { get; set; }
    public string CostumerId { get; set; }
    public int StartBalance { get; set; }
    public DateTime Date { get; set; }
}

class MoneyDepositedEvent
{
    public string AccountId { get; set; }
    public int Amount { get; set; }
    public DateTime Date { get; set; }
}

class MoneyWithdrawnEvent
{
    public string AccountId { get; set; }
    public int Amount { get; set; }
    public DateTime Date { get; set; }
}

class MoneyTransferredEvent
{
    public string AccountId { get; set; }
    public string TargetAccountId { get; set; }
    public int Amount { get; set; }
    public DateTime Date { get; set; }
}
