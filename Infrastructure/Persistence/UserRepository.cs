using Domain.Entities;
using Domain.Interfaces;
using System.Linq;

namespace Infrastructure.Persistence
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;

        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        // Lấy người dùng theo tên đăng nhập
        public User GetUserByUsername(string username)
        {
            User user =  _context.Users.FirstOrDefault(u => u.Username.Equals(username));
            if (user == null)
            {
                Console.WriteLine("No user found with the given username.");
            }
            else 
            {
                Console.WriteLine($"User found: {user.Username}, Password: {user.Password.Trim()}");
            }
            return user;
        }

        // Xác thực thông tin đăng nhập
        public bool ValidateUserCredentials(string username, string password)
        {
            var user = GetUserByUsername(username.Trim());
            // So sánh mật khẩu (nên sử dụng mã hóa)
            return user != null && VerifyPassword(password.Trim(), user.Password.Trim()); // Thay thế bằng logic thực tế
        }

        private bool VerifyPassword(string password, string storedPassword)
        {
            // Ở đây bạn nên sử dụng phương pháp mã hóa để kiểm tra mật khẩu
            return password == storedPassword; // Đây chỉ là một ví dụ, hãy thay thế bằng logic mã hóa
        }
    }
}
