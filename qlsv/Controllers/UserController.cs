using Domain.Entities;
using Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace presentation.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserRepository _userRepository;

        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        // Hiển thị form đăng nhập
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        // Xử lý đăng nhập
        [HttpPost]
        public IActionResult Login(string username, string password)
        {
            // Kiểm tra thông tin đăng nhập
            var isValidUser = _userRepository.ValidateUserCredentials(username, password);
            if (isValidUser)
            {
                // Đăng nhập thành công
                HttpContext.Session.SetString("Username", username); // Lưu thông tin vào 
                return RedirectToAction("","students"); // Chuyển hướng đến danh sách sinh viên
            }
            else
            {
                ModelState.AddModelError("", "Tên đăng nhập hoặc mật khẩu không đúng.");
                return View(new User { Username = username }); // Trả về view đăng nhập với tên người dùng đã nhập
            }
        }


        // Thêm phương thức để đăng xuất
        public IActionResult Logout()
        {
            // Xóa session hoặc cookie khi đăng xuất
            // Ví dụ: HttpContext.Session.Remove("Username");
            return RedirectToAction("Login"); // Chuyển hướng đến trang đăng nhập
        }
    }
}
