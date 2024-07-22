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
using Core.ViewModel;
using Core.Repository;
using BusinessLayer.Ultils;

namespace CoreDemo.Controllers
{
    [AllowAnonymous]
    public class LoginController : Controller
    {
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly IEmailSender emailSender;

        public LoginController(SignInManager<User> signInManager, UserManager<User> userManager,IEmailSender email)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            emailSender=email;
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
                        ModelState.AddModelError("UserName", "Người dùng không tồn tại");
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
                    ModelState.AddModelError("UserName", "Tên người dùng hoặc mật khẩu sai");
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


        public async Task<IActionResult> ForgotPass()
        {
            return View();
        }

        [HttpPost]
		public async Task<IActionResult> ForgotPass(ForgotPass forgot)
		{
            //if(!ModelState.IsValid)
            //{
            //    return View(forgot);
            //}

            var user = await _userManager.FindByEmailAsync(forgot.Email);
            if(user != null)
            {
                var code = await _userManager.GeneratePasswordResetTokenAsync(user);
                var callbackUrl = Url.Action("ResetPass", "Login", new {userId = user.Id, Token=code}, protocol:Request.Scheme);
                bool isSendEmail = await emailSender.EmailSendAsync(forgot.Email, "Đổi mật khẩu", EmailTemplate.GenerateEmailTemplate(user.Email,callbackUrl,"Click here"));
                if (isSendEmail)
                {
                    Response response = new Response();
                    response.Message = "Đổi link ";
                    return RedirectToAction("ConfirmPass", "Login");
                }
            }


			return View();
		}
        public IActionResult ConfirmPass(Response response)
        {
            return View(response);
        }

		[HttpGet("resetpass")]
		public IActionResult ResetPass(string userId, string Token)
		{
			var model = new ForgotPass()
			{
				Token = Token,
				UserId = userId,
			};
			return View(model);
		}

		[HttpPost("resetpass")]
		public async Task<IActionResult> ResetPass(ForgotPass forgot)
		{
			Response re = new Response();
			ModelState.Remove("Email");
			if (!ModelState.IsValid)
			{
				return View(forgot);
			}
			var user = await _userManager.FindByIdAsync(forgot.UserId);
			if (user == null)
			{
				return View(forgot);
			}
			var result = await _userManager.ResetPasswordAsync(user, forgot.Token, forgot.Password);
			if (result.Succeeded)
			{
				re.Message = "Mật khẩu của bạn đã được đổi";
				return RedirectToAction("ConfirmPass", re);
			}
			return View(forgot);
		}
	}
}
