using AppDev.Areas.Admin.ViewModels;
using AppDev.Data;
using AppDev.Helpers;
using AppDev.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AppDev.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = Roles.Admin)]
    public class AccountsController : Controller
    {
        private readonly ApplicationDbContext context;
        private readonly UserManager<ApplicationUser> userManager;

        public AccountsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            this.context = context;
            this.userManager = userManager;
        }
        public ActionResult Index()
        {
            return View();
        }


        public async Task<ActionResult> Customers()
        {
            var users = await userManager.GetUsersInRoleAsync(Roles.User);

            ViewData["Title"] = "Customer Accounts";
            ViewData["ReturnUrl"] = HttpContext.Request.Path;
            return View("Users", users);
        }

        public async Task<ActionResult> StoreOwners()
        {
            var users = await userManager.GetUsersInRoleAsync(Roles.StoreOwner);

            ViewData["Title"] = "Store Owner Accounts";
            ViewData["ReturnUrl"] = HttpContext.Request.Path;
            return View("Users", users);
        }


        public async Task<ActionResult> ResetPassword(string? id, string? returnUrl)
        {
            if (id is null)
                return View();

            var user = await userManager.FindByIdAsync(id);

            if (user == null)
                return NotFound();

            var model = new ResetPasswordViewModel()
            {
                ReturnUrl = returnUrl,
                Email = user.Email,
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(string? id, string? returnUrl, ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = await userManager.FindByEmailAsync(model.Email);

            if (user == null)
            {
                ModelState.AddModelError("Email", "Cannot find user with email.");
                return View(model);
            }

            var permission = await userManager.IsInRoleAsync(user, Roles.User)
                || await userManager.IsInRoleAsync(user, Roles.StoreOwner);

            if (!permission)
            {
                ModelState.AddModelError("", "Cannot reset password. Permission denied.");
                return View(model);
            }

            var code = await userManager.GeneratePasswordResetTokenAsync(user);

            var result = await userManager.ResetPasswordAsync(user, code, model.Password);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                    return View(model);
                }
            }

            if (returnUrl != null)
                return Redirect(returnUrl);

            return RedirectToAction(nameof(Index));
        }
    }
}
