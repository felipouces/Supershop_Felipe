using Microsoft.EntityFrameworkCore;
using Supershop.Data.Entities;
using Supershop.Helpers;
using Supershop.Models;
using System.Linq;
using System.Threading.Tasks;

namespace Supershop.Data
{
    
    public class OrderRepository : GenericRepository<Order>, IOrderRepository
    {
        private readonly DataContext _context;
        private readonly IUserHelper _userHelper;

        public OrderRepository(DataContext context, IUserHelper userHelper) : base(context)
        {
            _context = context;
            _userHelper = userHelper;
        }

        public async Task AddItemToOrderAsync(AddItemViewModel model, string userName)
        {
            var user = await _userHelper.GetUserByEmailAsync(userName);
            if (user == null)
            {
                return;
            }

            var product = await _context.Products.FindAsync(model.ProductId);
            if (product == null)
            {
                return;
            }

            var orderDetailTemp = await  _context.OrderDetailsTemps.Where(odt => odt.User == user && odt.Product == product).FirstOrDefaultAsync();

            if (orderDetailTemp == null)
            {
                orderDetailTemp = new OrderDetailsTemp
                {
                    Price = product.Price,
                    Product = product,
                    Quantity = model.Quantity,
                    User = user,
                };

                _context.OrderDetailsTemps.Add(orderDetailTemp);
            }
            else
            {
                orderDetailTemp.Quantity += model.Quantity;
                _context.OrderDetailsTemps.Update(orderDetailTemp);
            }

            await _context.SaveChangesAsync();
        }

        public async Task<bool> ConfirmOrderAsync(string userName)
        {
            // Get the user by their email (userName)
            var user = await _userHelper.GetUserByEmailAsync(userName);
            if (user == null)
            {
                return false;
            }

            // Retrieve all temporary order details for the user
            var orderTmps = await _context.OrderDetailsTemps
                .Include(o => o.Product)
                .Where(o => o.User == user)
                .ToListAsync();

            // If there are no temporary order details, return false
            if (orderTmps == null || orderTmps.Count == 0)
            {
                return false;
            }

            // Create order details from the temporary order details
            var details = orderTmps.Select(o => new OrderDetail
            {
                Price = o.Price,
                Product = o.Product,
                Quantity = o.Quantity,
            }).ToList();

            // Create a new order with the user and the order details
            var order = new Order
            {
                OrderDate = System.DateTime.UtcNow,
                User = user,
                Items = details
            };

            // Save the new order to the database
            await CreateAsync(order);
            _context.OrderDetailsTemps.RemoveRange(orderTmps);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task DeleteDetailTempAsync(int id)
        {
            var orderDetailTemp = await _context.OrderDetailsTemps.FindAsync(id);
            if (orderDetailTemp == null)
            {
                return;
            }

            _context.OrderDetailsTemps.Remove(orderDetailTemp);
            await _context.SaveChangesAsync();
        }

        public async Task<IQueryable<OrderDetailsTemp>> GetDetailTempsAsync(string userName)
        {
            var user = await  _userHelper.GetUserByEmailAsync(userName);
            if (user == null)
            {
                return null;
            }

            return _context.OrderDetailsTemps
                .Include(p => p.Product)
                .Where(o => o.User.Id == user.Id)
                //.Where(o => o.User == user)
                //.OrderBy(p => p.OrderDate);
                .OrderBy(o => o.Product.Name);

        }

        public async Task<IQueryable<Order>> GetOrderAsync(string userName)
        {
            var user = await _userHelper.GetUserByEmailAsync(userName);
            if (user == null) // || !(await _userHelper.IsUserInRoleAsync(user, "Admin"))
            {
                return null;
            }

            if (await _userHelper.IsUserInRoleAsync(user, "Admin"))
            {
                return _context.Orders
                    .Include(o => o.User)
                    .Include(o => o.Items)
                    .ThenInclude(p => p.Product)
                    .OrderByDescending(o => o.OrderDate);
            }
            
            return _context.Orders
                .Include(o => o.Items)
                .ThenInclude(p => p.Product)
                .Where(o => o.User == user)
                .OrderByDescending(o => o.OrderDate);
        }

        public async Task ModifyOrderDetailTempQuantityAsync(int id, double quantity)
        {
            var orderDetailTemp = await _context.OrderDetailsTemps.FindAsync(id);
            if(orderDetailTemp == null)
            {
                return;
            }

            orderDetailTemp.Quantity += quantity;
            if (orderDetailTemp.Quantity > 0)
            {
                _context.OrderDetailsTemps.Update(orderDetailTemp);
                await _context.SaveChangesAsync();
            }
            
        }
    }
}
