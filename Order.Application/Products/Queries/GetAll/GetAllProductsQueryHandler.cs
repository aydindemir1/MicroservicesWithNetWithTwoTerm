using MediatR;
using Order.Application.Products.Dto;
using Order.Application.Products.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Order.Application.Products.Queries.GetAll
{
    public class GetAllProductsQueryHandler(IProductReadRepository productReadRepository) : IRequestHandler<GetAllProductsQuery, List<ProductWithCategoryDto>>
    {
        public async Task<List<ProductWithCategoryDto>> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
        {
            
            var products = await  productReadRepository.GetAll();

            return products.Select(p => new ProductWithCategoryDto(p.Id,p.Name, p.Quantity, p.Price, p.CategoryName)).ToList();

        }
    }
}
