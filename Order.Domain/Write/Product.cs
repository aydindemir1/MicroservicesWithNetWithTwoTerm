using Order.Domain.Write;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Order.Domain.Write
{
    public class Product
    {
        public string Id { get; set; } // MomgoDB requires string Id for ObjectId

        public string Name { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
      
        public int CategoryId { get; set; }
        public Category Category { get; set; } = default!;

    }
}
