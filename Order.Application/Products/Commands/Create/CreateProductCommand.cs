using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Order.Application.Products.Commands.Create
{
    public record CreateProductCommand(string Name, int Quantity, decimal Price, int CategoryId):IRequest<string>;
}
