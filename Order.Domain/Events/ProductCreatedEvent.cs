using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Order.Domain.Events
{
    public record ProductCreatedEvent(string Id, string Name,  int Quantity, decimal Price, string CategoryName);
}
