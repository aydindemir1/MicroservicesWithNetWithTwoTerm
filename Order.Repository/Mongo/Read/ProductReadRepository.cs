using MongoDB.Driver;
using Order.Application.Products.Dto;
using Order.Application.Products.Repository;
using Order.Domain.Read;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Mongo.Read
{
    public class ProductReadRepository(MongoDbContext client) : IProductReadRepository
    {
        public async Task<List<ProductWithCategory>> GetAll()=>
            await client.Products.Find(f => true).ToListAsync();
    }
}
