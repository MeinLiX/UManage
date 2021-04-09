using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using UManager.ViewModels;
using UManager.Models;
using UManager.IdentityFilter;
using System.Linq;
using System.Collections.Generic;
using System;

namespace UManager.Controllers
{
    public class AccountController : Controller
    {
        private readonly CustomUserManager<UserModel> _userManager;
        private readonly CustomSignInManager<UserModel> _signInManager;

        public AccountController(CustomUserManager<UserModel> userManager, CustomSignInManager<UserModel> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult Register()
        {
            if (User.Identity.IsAuthenticated)
                return RedirectToAction("Index", "Home");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                UserModel user = new() { Email = model.Email, UserName = model.Email };

                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, false);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
                return RedirectToAction("Index", "Home");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result =
                    await _signInManager.PasswordSignInAsync(model.Email, model.Password, false, false);

                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }
                else if (result.IsLockedOut)
                {
                    ModelState.AddModelError("", "Your account is banned.");
                }
                else
                {
                    ModelState.AddModelError("", "Incorrect email or (and) password");
                }
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult Manage()
        {
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("Index", "Home");

            return View(_userManager.Users.ToList());
        }

        [HttpPost]
        public async Task<ActionResult> Manage(List<UserModel> items, [Bind("ToBlock")] bool ToBlock = false)
        {
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("Index", "Home");

            foreach (var user in items)
            {
                if (user.Selected)
                {
                    var foundedUser = await _userManager.FindByEmailAsync(user.Email);
                    if (foundedUser is not null)
                    {
                        foundedUser.IsBlocked = ToBlock;
                        foundedUser.Selected = false;
                        await _userManager.SetLockoutEnabledAsync(foundedUser, ToBlock);
                        await _userManager.SetLockoutEndDateAsync(foundedUser, ToBlock ? DateTimeOffset.Now.AddDays(5000) : DateTimeOffset.Now);
                        if(ToBlock) 
                            await _userManager.UpdateSecurityStampAsync(foundedUser);
                    }
                }
            }
            return View(_userManager.Users.ToList());
        }
    }
}