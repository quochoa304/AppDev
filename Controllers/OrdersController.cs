using AppDev.Data;
using AppDev.Helpers;
using AppDev.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AppDev.Controllers
{
    [Authorize(Roles = Roles.User)]
    public class OrdersController : Controller
    {
        private readonly ApplicationDbContext context;
        private readonly UserManager<ApplicationUser> userManager;

        public OrdersController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            this.context = context;
            this.userManager = userManager;
        }

        private string? _customerId;

        private string CustomerId
        {
            get
            {
                _customerId ??= userManager.GetUserId(User);
                return _customerId;
            }
        }

        // GET: Orders
        public async Task<IActionResult> Index()
        {
            var orders = await context.Orders
                .Include(o => o.Store)
                .Where(o => o.CustomerId == CustomerId)
                .ToListAsync();
            return View(orders);
        }

        // GET: Orders/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await context.Orders
                .Include(o => o.Store)
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Book)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (order == null || order.CustomerId != CustomerId)
            {
                return NotFound();
            }

            order.Customer = await userManager.FindByIdAsync(CustomerId);

            return View(order);
        }
    }
}
