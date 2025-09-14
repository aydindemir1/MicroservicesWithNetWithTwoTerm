using Order.Application.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Order.Domain;
using Order.Application.Order.Queries;

namespace Order.Repository
{
    public class OrderRepository(AppDbContext context) : IOrderRepository
    {
        public int CreateOrder(Domain.Order order)
        {
            context.Orders.Add(order);

            context.SaveChanges();

            return order.Id;
        }

        public  ValueTask<Domain.Order?> GetByIdAsync(int id)
        {
            // CancellationToken asenkron işlemler için iptal token'ı oluşturur
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            cancellationTokenSource.Cancel();
            cancellationTokenSource.CancelAfter(TimeSpan.FromSeconds(30));

            return context.Orders.FindAsync(id);
        }
    }
}
