using Core.Models;
using EntityLayer.Concrete;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace CoreDemo.Controllers
{
    [AllowAnonymous]
    public class HomeController : Controller
    {
        private readonly UserManager<User> _userManager;

        public HomeController(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        // Hiển thị trang chính
        public IActionResult Index()
        {
            return View();
        }

        // Hiển thị trang lỗi
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        // Xử lý tìm kiếm
        public IActionResult Search(SearchViewModel searchViewModel)
        {
            var roles = GetRoles().Result;

            // Xác định loại người dùng
            if (roles.Contains("Admin"))
            {
                searchViewModel.UserType = "Admin";
            }
            else if (roles.Contains("Writer"))
            {
                searchViewModel.UserType = "Writer";
            }
            else
            {
                searchViewModel.UserType = "User/Member";
            }

            var searchResults = SearchPages(searchViewModel);

            return View(searchResults);
        }

        // Tìm kiếm các trang dựa trên mẫu tìm kiếm
        private static List<SearchResultViewModel> SearchPages(SearchViewModel model)
        {
            List<SearchResultViewModel> searchResults = new();

            // Các controller và action cho admin
            List<(string controller, string action)> adminControllerAndActions = new()
            {
                ("Admin", "Index"),
                ("Blog", "Index"),
                ("Category", "Index"),
                ("Category", "AddCategory"),
                ("Chart", "Index"),
                ("Comment","Index"),
                ("Excel", "BlogListToExcel"),
                ("Message", "Inbox"),
                ("Message", "SendBox"),
                ("Message", "SendMessage"),
                ("Role", "Index"),
                ("Role", "UserRoleList"),
                ("Role", "AddRole"),
                ("Writer", "Index"),
                ("Writer", "WriterAdd"),
            };

            // Các controller và action cho writer
            List<(string controller, string action)> writerControllerAndActions = new()
            {
                ("About", "Index"),
                ("Blog", "BlogAdd"),
                ("Writer", "WriterEditProfile"),
                ("Dashboard", "Index"),
                ("Message", "Inbox"),
                ("Message", "SendBox"),
                ("Message", "SendMessage"),
            };

            // Các controller và action cho user
            List<(string controller, string action)> userControllerAndActions = new()
            {
                ("Blog", "Index"),
                ("Contact", "Index"),
                ("Home", "Index"),
                ("Login", "Index"),
                ("Register", "Index"),
            };

            // Tìm kiếm cho người dùng thông thường
            foreach (var (controller, action) in userControllerAndActions)
            {
                if (string.Equals(model.SearchTerm, controller, System.StringComparison.OrdinalIgnoreCase)
                    || string.Equals(model.SearchTerm, action, System.StringComparison.OrdinalIgnoreCase))
                {
                    var result = new SearchResultViewModel
                    {
                        Payload = "Kết quả cho từ khóa: " + model.SearchTerm,
                        ControllerName = "/" + controller + "/",
                        ActionName = action + "/",
                    };

                    searchResults.Add(result);
                }
            }

            // Tìm kiếm cho admin
            if (model.UserType == "Admin")
            {
                foreach (var (controller, action) in adminControllerAndActions)
                {
                    if (string.Equals(model.SearchTerm, controller, System.StringComparison.OrdinalIgnoreCase)
                        || string.Equals(model.SearchTerm, action, System.StringComparison.OrdinalIgnoreCase))
                    {
                        var result = new SearchResultViewModel
                        {
                            Payload = "Kết quả cho từ khóa: " + model.SearchTerm,
                            ControllerName = "/Admin/" + controller + "/",
                            ActionName = action + "/",
                        };

                        searchResults.Add(result);
                    }
                }

                foreach (var (controller, action) in writerControllerAndActions)
                {
                    if (string.Equals(model.SearchTerm, controller, System.StringComparison.OrdinalIgnoreCase)
                        || string.Equals(model.SearchTerm, action, System.StringComparison.OrdinalIgnoreCase))
                    {
                        var result = new SearchResultViewModel
                        {
                            Payload = "Kết quả cho từ khóa: " + model.SearchTerm,
                            ControllerName = "/" + controller + "/",
                            ActionName = action + "/",
                        };

                        searchResults.Add(result);
                    }
                }
            }

            // Tìm kiếm cho writer
            if (model.UserType == "Writer")
            {
                foreach (var (controller, action) in writerControllerAndActions)
                {
                    if (string.Equals(model.SearchTerm, controller, System.StringComparison.OrdinalIgnoreCase)
                        || string.Equals(model.SearchTerm, action, System.StringComparison.OrdinalIgnoreCase))
                    {
                        var result = new SearchResultViewModel
                        {
                            Payload = "Kết quả cho từ khóa: " + model.SearchTerm,
                            ControllerName = "/" + controller + "/",
                            ActionName = action + "/",
                        };

                        searchResults.Add(result);
                    }
                }
            }

            return searchResults;
        }

        // Lấy các vai trò của người dùng
        private async Task<List<string>> GetRoles()
        {
            try
            {
                var user = await _userManager.FindByNameAsync(User.Identity.Name);
                var roles = _userManager.GetRolesAsync(user).Result.ToList();

                return roles;
            }
            catch
            {
                List<string> roles = new();
                return roles;
            }
        }
    }
}
