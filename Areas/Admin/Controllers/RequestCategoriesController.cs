using AppDev.Data;
using AppDev.Helpers;
using AppDev.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AppDev.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = Roles.Admin)]
    public class RequestCategoriesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RequestCategoriesController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var requests = await _context.RequestCategories
                .OrderBy(r => r.IsApproved)
                .ThenByDescending(r => r.Id)
                .ToListAsync();

            return View(requests);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var requestCategory = await _context.RequestCategories
                .Include(r => r.StoreOwner)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (requestCategory == null)
            {
                return NotFound();
            }

            return View(requestCategory);
        }


        public async Task<IActionResult> Approve(int? id)
        {
            if (id == null || _context.RequestCategories == null)
            {
                return NotFound();
            }

            var requestCategory = await _context.RequestCategories
                .Include(r => r.StoreOwner)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (requestCategory == null || !requestCategory.IsApprovable())
            {
                return NotFound();
            }
            return View(requestCategory);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Approve(int id, [Bind("Id,Name,StoreOwnerId,IsApproved,Message")] RequestCategory requestCategory)
        {
            if (id != requestCategory.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                if (requestCategory.IsApproved == true)
                {
                    if (await _context.Categories.AnyAsync(c => c.Name == requestCategory.Name))
                    {
                        ModelState.AddModelError("", "Category is duplicated");
                        return View(requestCategory);
                    }

                    _context.Categories.Add(new Category()
                    {
                        Name = requestCategory.Name,
                    });
                }

                _context.Update(requestCategory);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            requestCategory = await _context.RequestCategories
                .Include(r => r.StoreOwner)
                .FirstAsync(m => m.Id == id);

            return View(requestCategory);
        }

        private bool RequestCategoryExists(int id)
        {
            return _context.RequestCategories.Any(e => e.Id == id);
        }
    }
}
