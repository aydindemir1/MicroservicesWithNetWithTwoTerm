using Order.Domain.Read;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Order.Application.Consumers
{
    public interface ISyncWriteRepository
    {
        public Task Create(ProductWithCategory productWithCategory);
    }
}
