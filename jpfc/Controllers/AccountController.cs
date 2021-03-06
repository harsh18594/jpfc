﻿using jpfc.Models;
using jpfc.Models.AccountViewModels;
using jpfc.Services.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace jpfc.Controllers
{
    [Authorize]
    [Route("[controller]/[action]")]
    public class AccountController : JpfcController
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ILogger _logger;
        private readonly ILoginService _loginService;

        public AccountController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ILogger<AccountController> logger,
            ILoginService loginService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _loginService = loginService;
        }

        [TempData]
        public string ErrorMessage { get; set; }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Login(string returnUrl = null)
        {
            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {
                var result = await _loginService.LogUserIn(model);
                if (result.Success)
                {
                    _logger.LogInformation("User logged in.");
                    if (!string.IsNullOrEmpty(returnUrl))
                    {
                        return RedirectToLocal(returnUrl);
                    }
                    return RedirectToAction(controllerName: "Admin", actionName: nameof(AdminController.Index));
                }
                else
                {
                    SetSiteMessage(MessageType.Error, DisplayFor.FullRequest, result.Error);
                    return View(model);
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            _logger.LogInformation("User logged out.");
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }

        public async Task<IActionResult> ManageProfile()
        {
            return View();
        }

        [HttpGet]
        public IActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var userId = _userManager.GetUserId(User);
            var result = await _loginService.ChangePasswordAsync(model, userId);
            if (result.Success)
            {
                SetSiteMessage(MessageType.Success, DisplayFor.FullRequest, "Your password has been updated.");
                return RedirectToAction(nameof(ManageProfile));
            }

            SetSiteMessage(MessageType.Error, DisplayFor.FullRequest, result.Error);
            return View(model);
        }

        [HttpGet]
        public IActionResult ResetAccessCode()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetAccessCode(Models.ManageViewModels.AccessCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var userId = _userManager.GetUserId(User);
            var result = await _loginService.ResetAccessCodeAsync(model, userId);
            if (result.Success)
            {
                SetSiteMessage(MessageType.Success, DisplayFor.FullRequest, "Access code has been updated.");
                return RedirectToAction(nameof(ManageProfile));
            }

            SetSiteMessage(MessageType.Error, DisplayFor.FullRequest, result.Error);
            return View(model);
        }

        [AllowAnonymous]
        public async Task<IActionResult> Reset(string verificationCode)
        {
            if (string.IsNullOrEmpty(verificationCode))
            {
                return Ok();
            }

            var success = await _loginService.ResetAccount(verificationCode);
            if (success)
            {
                SetSiteMessage(MessageType.Success, DisplayFor.FullRequest, "Reset operation was successfully completed.");
                return RedirectToAction("Login");
            }
            else
            {
                return BadRequest();
            }
        }

        #region Helpers

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
        }

        #endregion
    }
}
