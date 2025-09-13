using Order.Application.Order.CreateOrderUseCase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Order.Application.Order
{
    public interface IOrderService
    {
       
            Task<CreateOrderResponse> Create(CreateOrderRequest request);
        
    }
}
