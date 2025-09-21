using Order.Application.Consumers;
using Order.Domain.Read;
using Repository.Mongo.Read;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Mongo.Write
{
    public class SyncWriteRepository(MongoDbContext context) : ISyncWriteRepository
    {
        public async Task Create(ProductWithCategory productWithCategory)
        {
            
            await context.Products.InsertOneAsync(productWithCategory);

        }
    }
}
