using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Order.Domain;
using Order.Application.Order;

namespace Order.Repository
{
    public class AppDbContext(DbContextOptions options): DbContext(options)
    {
        public DbSet<Domain.Order> Orders { get; set; }
    }
}
