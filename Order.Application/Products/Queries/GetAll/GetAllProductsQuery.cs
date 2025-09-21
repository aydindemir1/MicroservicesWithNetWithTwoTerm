using MediatR;
using Order.Application.Products.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Order.Application.Products.Queries.GetAll
{
    public record GetAllProductsQuery: IRequest<List<ProductWithCategoryDto>>;
    
}
