using Microsoft.EntityFrameworkCore;
using TestTask.Data;
using TestTask.Enums;
using TestTask.Models;
using TestTask.Services.Interfaces;

namespace TestTask.Services.Implementations
{
    class OrderServiceImplementation : IOrderService
    {
        private readonly ApplicationDbContext _dbContext;

        public OrderServiceImplementation(ApplicationDbContext dbContext)
        {

            _dbContext = dbContext;
        }

        public async Task<Order> GetOrder()
        {

            var order = await _dbContext.Orders
                .Where(o => o.Quantity > 1)
                .OrderByDescending(o => o.CreatedAt)
                .FirstOrDefaultAsync();

            return order;
        }

        public async Task<List<Order>> GetOrders()
        {
            var orders = await _dbContext.Orders
             .Include(o => o.User)
             .Where(o => o.User.Status == UserStatus.Active)
             .OrderBy(o => o.CreatedAt)
             .Select(o => new Order
             {
                 Id = o.Id,
                 UserId = o.UserId,
                 ProductName = o.ProductName,
                 Price = o.Price,
                 Quantity = o.Quantity,
                 CreatedAt = o.CreatedAt,
                 Status = o.Status,
             })
             .ToListAsync();

            return orders;
        }
    }
}
