using ECommerceSystem.GUI.Services;
using ECommerceSystem.Shared.DTOs.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ECommerceSystem.GUI.Controllers
{
    public class AccountController : Controller
    {
        private readonly AuthService _authService;
        private readonly ILogger<AccountController> _logger;

        public AccountController(AuthService authService, ILogger<AccountController> logger)
        {
            _authService = authService ?? throw new ArgumentNullException(nameof(authService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View(new LoginModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.ErrorMessage = "Dữ liệu không hợp lệ.";
                return View(model);
            }

            var (success, token, role) = await _authService.LoginAsync(model);
            if (!success)
            {
                ViewBag.ErrorMessage = "Tên đăng nhập hoặc mật khẩu không đúng.";
                return View(model);
            }

            TempData["UserRole"] = role;
            return RedirectToAction("CheckOnboarding");
        }



        [HttpGet]
        public async Task<IActionResult> CheckOnboarding()
        {
            // Lấy từ TempData, nếu không có thì lấy từ token
            var role = TempData["UserRole"]?.ToString()
                    ?? _authService.GetRoleFromToken();

            if (string.IsNullOrEmpty(role))
            {
                _logger.LogWarning("Không xác định được vai trò người dùng.");
                return RedirectToAction("Login"); // fallback an toàn
            }

            if (role == "User")
            {
                var isOnboarded = await _authService.IsOnboardingCompletedAsync();
                if (!isOnboarded)
                {
                    var step = await _authService.GetCurrentStepAsync();
                    return RedirectToAction($"Index", "Onboarding");
                }
            }

            return role switch
            {
                "Admin" => RedirectToAction("Index", "Admin"),
                "User" => RedirectToAction("Index", "Home"),
                _ => RedirectToAction("Index", "Home")
            };
        }



        [HttpGet]
        public IActionResult Register()
        {
            return View(new RegisterModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            if (!ModelState.IsValid)
            {
                foreach (var entry in ModelState)
                {
                    foreach (var error in entry.Value.Errors)
                    {
                        _logger.LogWarning("Lỗi trường {Field}: {Error}", entry.Key, error.ErrorMessage);
                    }
                }

                ViewBag.ErrorMessage = "Dữ liệu không hợp lệ. Vui lòng kiểm tra lại.";
                return View(model);
            }

            try
            {
                var (success, message) = await _authService.RegisterAsync(model);

                if (!success)
                {
                    ViewBag.ErrorMessage = message;
                    return View(model);
                }


                

                _logger.LogInformation("Đăng ký thành công cho username: {Username}", model.UserName);
                TempData["SuccessMessage"] = "Đăng ký thành công! Vui lòng đăng nhập.";
                return RedirectToAction("Login");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi đăng ký cho username: {Username}", model.UserName);
                ViewBag.ErrorMessage = "Đã xảy ra lỗi khi đăng ký. Vui lòng thử lại sau.";
                return View(model);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            try
            {
                await HttpContext.SignOutAsync("MyCookieAuth");
                _authService.Logout(); // Xóa cookie token
                _logger.LogInformation("User logged out successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during logout process.");
                return View("Error");
            }

            return RedirectToAction("Index", "Home");
        }
    }
}
