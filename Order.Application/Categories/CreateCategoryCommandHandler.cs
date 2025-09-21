using MediatR;
using Order.Application.Products.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Order.Application.Categories
{
    public class CreateCategoryCommandHandler(IProductWriteRepository productWriteRepository) : IRequestHandler<CreateCategoryCommand, int>
    {
        public async Task<int> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
        {

          return await  productWriteRepository.AddCategory(new Domain.Write.Category() { Name = request.Name });

        }
    }
}
