using Domain.Entities;
using Domain.Interfaces;
using System;

namespace Application.UseCases
{
    public class LoginUseCase
    {
        private readonly IUserRepository _userRepository;

        public LoginUseCase(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public bool Execute(User loginModel)
        {
            var user = _userRepository.GetUserByUsername(loginModel.Username);
            return user != null && VerifyPassword(loginModel.Password, user.Password); // So sánh mật khẩu
        }

        private bool VerifyPassword(string password, string dbpassword)
        {
            return password == dbpassword;
        }
    }
}
