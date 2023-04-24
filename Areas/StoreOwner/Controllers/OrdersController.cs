using AppDev.Data;
using AppDev.Helpers;
using AppDev.Models;
using AppDev.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AppDev.Areas.StoreOwner.Controllers
{
    [Area("StoreOwner")]
    [Authorize(Roles = Roles.StoreOwner)]
    public class OrdersController : Controller
    {
        private readonly ApplicationDbContext context;
        private readonly UserManager<ApplicationUser> userManager;

        public OrdersController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            this.context = context;
            this.userManager = userManager;
        }

        private string? _storeOwnerId;

        private string StoreOwnerId
        {
            get
            {
                _storeOwnerId ??= userManager.GetUserId(User);
                return _storeOwnerId;
            }
        }

        public async Task<IActionResult> Index(SearchViewModel? model)
        {
            var query = context.Orders
                .Include(o => o.Customer)
                .AsQueryable()
                .Where(o => o.StoreId == StoreOwnerId);

            if (model != null && !string.IsNullOrWhiteSpace(model.KeyWord))
            {
                var keyword = model.KeyWord.Trim().ToUpper();
                query = query.Where(u => u.Customer.NormalizedEmail.Contains(keyword));
            }

            var orders = await query.ToListAsync();

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
                .Include(o => o.Customer)
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Book)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (order == null || order.StoreId != StoreOwnerId)
            {
                return NotFound();
            }

            return View(order);
        }
    }
}
