using AppDev.Data;
using AppDev.Helpers;
using AppDev.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AppDev.Areas.StoreOwner.Controllers
{
    [Area("StoreOwner")]
    [Authorize(Roles = Roles.StoreOwner)]
    public class StoreController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> userManager;

        private string? _storeId;

        private string StoreId
        {
            get
            {
                _storeId = _storeId ?? userManager.GetUserId(User);
                return _storeId;
            }
        }

        public StoreController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            this.userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var store = await _context.Stores
                .Include(s => s.StoreOwner)
                .FirstOrDefaultAsync(s => s.Id == StoreId);
            if (store == null)
            {
                return RedirectToAction(nameof(Create));
            }

            return View(store);
        }

        // GET: StoreOwner/Create
        public IActionResult Create()
        {
            var model = new Store()
            {
                Id = StoreId,
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Store store)
        {
            if (ModelState.IsValid)
            {
                _context.Add(store);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(store);
        }

        // GET: StoreOwner/Edit/5
        public async Task<IActionResult> Edit()
        {
            var store = await _context.Stores.FindAsync(StoreId);
            if (store == null)
            {
                return NotFound();
            }

            return View(store);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Store store)
        {
            if (StoreId != store.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _context.Update(store);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(store);
        }
    }
}
