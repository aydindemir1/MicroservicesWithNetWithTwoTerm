using Order.Application.Products.Dto;
using Order.Domain.Read;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Order.Application.Products.Repository
{
    public interface IProductReadRepository
    {
        Task<List<ProductWithCategory>> GetAll();
    }
}
