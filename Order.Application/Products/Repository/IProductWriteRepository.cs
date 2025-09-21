using Order.Domain.Read;
using Order.Domain.Write;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Order.Application.Products.Repository
{
    public interface IProductWriteRepository
    {
        ValueTask<Category> GetCategory(int id);
        Task<int> AddCategory(Category category);
        Task<string> AddProduct(Product product);
        Task UpdateProduct(Product product);
        Task DeleteProduct(Product product);

    }
}
