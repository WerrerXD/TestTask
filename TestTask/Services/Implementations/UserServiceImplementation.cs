using Microsoft.EntityFrameworkCore;
using TestTask.Data;
using TestTask.Enums;
using TestTask.Models;
using TestTask.Services.Interfaces;

namespace TestTask.Services.Implementations
{
    public class UserServiceImplementation: IUserService
    {
        private readonly ApplicationDbContext _dbContext;

        public UserServiceImplementation(ApplicationDbContext dbContext)
        {

            _dbContext = dbContext;
        }

        public async Task<User> GetUser()
        {
            var userWithMaxOrderTotal = await _dbContext.Orders
            .Where(o => o.CreatedAt.Year == 2003)
            .GroupBy(o => o.UserId)
            .Select(g => new
            {
                UserId = g.Key,
                TotalOrderAmount = g.Sum(o => o.Quantity * o.Price)
            })
            .OrderByDescending(x => x.TotalOrderAmount)
            .Select(x => x.UserId)
            .FirstOrDefaultAsync();

            if (userWithMaxOrderTotal == null)
                return null;

            var user = await _dbContext.Users.FindAsync(userWithMaxOrderTotal);
            return user;

        }

        public async Task<List<User>> GetUsers()
        {
            var usersWithPaidOrdersIn2010 = await _dbContext.Users
            .Where(u => u.Orders.Any(o => o.CreatedAt.Year == 2010 && o.Status == OrderStatus.Paid))
            .ToListAsync();

            return usersWithPaidOrdersIn2010;
        }
    }
}
