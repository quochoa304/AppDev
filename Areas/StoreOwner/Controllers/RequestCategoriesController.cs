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
    public class RequestCategoriesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> userManager;

        public RequestCategoriesController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            this.userManager = userManager;
        }

        private string? _storeOwnerId;

        private string StoreOwnerId
        {
            get
            {
                _storeOwnerId = _storeOwnerId ?? userManager.GetUserId(User);
                return _storeOwnerId;
            }
        }

        public async Task<IActionResult> Index()
        {
            var requests = await _context.RequestCategories
                .Where(r => r.StoreOwnerId == StoreOwnerId)
                .OrderBy(r => r.IsApproved)
                .ThenBy(r => r.Id)
                .ToListAsync();

            return View(requests);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.RequestCategories == null)
            {
                return NotFound();
            }

            var requestCategory = await _context.RequestCategories
                .FirstOrDefaultAsync(r => r.Id == id);

            if (requestCategory == null || requestCategory.StoreOwnerId != StoreOwnerId)
            {
                return NotFound();
            }

            return View(requestCategory);
        }

        public IActionResult Create()
        {
            var request = new RequestCategory()
            {
                StoreOwnerId = StoreOwnerId,
            };

            return View(request);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,StoreOwnerId")] RequestCategory requestCategory)
        {
            if (ModelState.IsValid)
            {
                _context.RequestCategories.Add(requestCategory);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(requestCategory);
        }

        // GET: StoreOwner/RequestCategories/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.RequestCategories == null)
            {
                return NotFound();
            }

            var requestCategory = await _context.RequestCategories
                .FirstOrDefaultAsync(m => m.Id == id);
            if (requestCategory == null || !requestCategory.IsEditable(StoreOwnerId))
            {
                return NotFound();
            }
            return View(requestCategory);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,StoreOwnerId")] RequestCategory requestCategory)
        {
            if (id != requestCategory.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _context.RequestCategories.Update(requestCategory);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            return View(requestCategory);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var requestCategory = await _context.RequestCategories
                .FirstOrDefaultAsync(m => m.Id == id);
            if (requestCategory == null || !requestCategory.IsEditable(StoreOwnerId))
            {
                return NotFound();
            }

            return View(requestCategory);
        }

        // POST: StoreOwner/RequestCategories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var requestCategory = await _context.RequestCategories
                .FirstOrDefaultAsync(m => m.Id == id);
            if (requestCategory != null && requestCategory.IsEditable(StoreOwnerId))
            {
                _context.RequestCategories.Remove(requestCategory);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
