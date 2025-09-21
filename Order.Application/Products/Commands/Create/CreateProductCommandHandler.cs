using MassTransit;
using MediatR;
using Order.Application.Products.Repository;
using Order.Domain.Events;
using Order.Domain.Read;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Order.Application.Products.Commands.Create
{
    public class CreateProductCommandHandler(IProductWriteRepository productWriteRepository, IPublishEndpoint publishEndpoint) : IRequestHandler<CreateProductCommand, string>
    {
        public async Task<string> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            var product = new Domain.Write.Product()
            {
                Id = NewId.NextGuid().ToString(),
                Name = request.Name,
                Price = request.Price,
                
                Quantity = request.Quantity,
                CategoryId = request.CategoryId
            };

            var newProductId = await productWriteRepository.AddProduct(product);

            var category = await productWriteRepository.GetCategory(request.CategoryId);




            await publishEndpoint.Publish(new ProductCreatedEvent(newProductId, request.Name, request.Quantity, request.Price, category.Name));

            return newProductId;

        }
    }
}
