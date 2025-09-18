using MassTransit;
using MongoDB.Driver;
using Shared.Events;
using Shared.Messages;
using Stock.API.Services;

namespace Stock.API.Consumers
{
    public class OrderCreatedEventConsumer : IConsumer<OrderCreatedEvent>
    {

        IMongoCollection<Models.Entities.Stock> _stockColleciton;
        public OrderCreatedEventConsumer(MongoDbService mongoDbService)
        {

            _stockColleciton = mongoDbService.GetCollection<Models.Entities.Stock>();
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

                //PaymentService'e event gönderilecek
            }
            else
            {
                //geçersiz sipariş
            }

        }
    }
}
