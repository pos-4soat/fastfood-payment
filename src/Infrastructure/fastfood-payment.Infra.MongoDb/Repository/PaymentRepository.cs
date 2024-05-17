using fastfood_payment.Domain.Contracts.Mongo;
using fastfood_payment.Domain.Entity;
using fastfood_payment.Infra.MongoDb.Context;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace fastfood_payment.Infra.MongoDb.Repository
{
    public class PaymentRepository : IPaymentRepository
    {
        private readonly IMongoCollection<PaymentEntity> _collection;

        public PaymentRepository(
            MongoDbContext dbContext,
            IConfiguration config
        )
        {
            string collection = config.GetConnectionString("PaymentCollection") ?? throw new ArgumentNullException("Null collection");
            _collection = dbContext.GetCollection<PaymentEntity>(collection);
        }

        public async Task<PaymentEntity> FindAsync(int orderId, CancellationToken cancellationToken)
        {
            FilterDefinition<PaymentEntity> filter = Builders<PaymentEntity>.Filter.Eq(x => x.OrderId, orderId);

            PaymentEntity response = await _collection.Find(filter).FirstOrDefaultAsync(cancellationToken);

            return response;
        }

        public async Task InsertAsync(PaymentEntity paymentEntity, CancellationToken cancellationToken)
        {
            InsertOneOptions options = new();

            await _collection.InsertOneAsync(paymentEntity, options, cancellationToken);
        }

        public async Task UpdateAsync(int orderId, CancellationToken cancellationToken)
        {
            FilterDefinition<PaymentEntity> filter = Builders<PaymentEntity>.Filter.Eq(x => x.OrderId, orderId);

            UpdateDefinition<PaymentEntity> update = Builders<PaymentEntity>.Update.Set(x => x.Payed, true);

            UpdateOptions options = new UpdateOptions() { IsUpsert = true };

            await _collection.UpdateOneAsync(filter, update, options, cancellationToken);
        }
    }
}
