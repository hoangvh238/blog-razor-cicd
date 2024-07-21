using Core.Models;
using DocumentFormat.OpenXml.Spreadsheet;
using EntityLayer.Concrete;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CoreDemo.Controllers
{
	[AllowAnonymous]
	public class LoginController : Controller
	{
		private readonly SignInManager<User> _signInManager;
		private readonly UserManager<User> _userManager;

		public LoginController(SignInManager<User> signInManager, UserManager<User> userManager)
		{
			_signInManager = signInManager;
			_userManager = userManager;
		}

		[HttpGet]
		public IActionResult Index()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Index(UserLoginViewModel userLoginViewModel)
		{
			if (ModelState.IsValid)
			{
				var userName = await _signInManager.UserManager.FindByNameAsync(userLoginViewModel.UserName);
				var isPasswordCorrect = await _userManager.CheckPasswordAsync(userName, userLoginViewModel.Password);
				var result = await _signInManager.PasswordSignInAsync(userLoginViewModel.UserName, userLoginViewModel.Password, userLoginViewModel.IsPersistent, true);

				if (userName != null && isPasswordCorrect)
				{
					if (result.IsLockedOut)
					{
						ModelState.AddModelError("UserName", "Người dùng không hoạt động");
						return View();
					}
					else if (result.RequiresTwoFactor)
					{
						ModelState.AddModelError("UserName", "Xác thực hai yếu tố không hoạt động");
						return View();
					}
				}

				if (!result.Succeeded)
				{
					ModelState.AddModelError("UserName", "Tên người dùng hoặc mật khẩu không chính xác");
					return View();
				}

				if (result.Succeeded)
				{
					return RedirectToAction("Index", "Home");
				}
				else
				{
					return RedirectToAction("Index", "Login");
				}
			}
			return View();
		}

		public async Task<IActionResult> LogOut()
		{
			await _signInManager.SignOutAsync();
			return RedirectToAction("Index", "Login");
		}

		public IActionResult AccessDenied()
		{
			return View();
		}
	}
}
