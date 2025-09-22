using MassTransit;
using MongoDB.Driver;
using Shared;
using Shared.Events;
using Shared.Messages;
using Stock.API.Services;

namespace Stock.API.Consumers
{
    public class OrderCreatedEventConsumer : IConsumer<OrderCreatedEvent>
    {

        IMongoCollection<Models.Entities.Stock> _stockColleciton;
        readonly ISendEndpointProvider _sendEndpointprovider;
        public OrderCreatedEventConsumer(MongoDbService mongoDbService, ISendEndpointProvider sendEndpointprovider)
        {

            _stockColleciton = mongoDbService.GetCollection<Models.Entities.Stock>();
            _sendEndpointprovider = sendEndpointprovider;
        }

        public async Task Consume(ConsumeContext<OrderCreatedEvent> context)
        {
            List<bool> stockResult = new();
            foreach (OrderItemMessage orderItem in context.Message.OrderItems)
            {
                stockResult.Add((await _stockColleciton.FindAsync(s => s.ProductId == orderItem.ProductId && s.Count >= orderItem.Count)).Any());


            }
            if (stockResult.TrueForAll(sr => sr.Equals(true)))
            {
                foreach (OrderItemMessage orderItem in context.Message.OrderItems)
                {
                    Models.Entities.Stock stock = await (await _stockColleciton.FindAsync(s => s.ProductId == orderItem.ProductId)).FirstOrDefaultAsync();

                    stock.Count -= orderItem.Count;
                    await _stockColleciton.FindOneAndReplaceAsync(s => s.ProductId == orderItem.ProductId, stock);
                }
                StockReservedEvent stockReservedEvent = new()
                {
                    BuyerId = context.Message.BuyerId,
                    OrderId = context.Message.OrderId,
                    TotalPrice = context.Message.TotalPrice,
                };
                //queue bazlı gönderim yalnızca bu kuyruğu dinleyenler haberdar olacak.
                ISendEndpoint sendEndpoint = await _sendEndpointprovider.GetSendEndpoint(new Uri($"queue:{RabbitMQSettings.Payment_StockReservedEventQueue}"));
                await sendEndpoint.Send(stockReservedEvent);


            }
            else
            {
                //geçersiz sipariş
            }

        }
    }
}
