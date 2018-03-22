﻿using CarHelp.BLL.Model.DTO;
using CarHelp.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CarHelp.BLL.Services
{
    public interface IAccountService
    {
        Task GenerateSmsCodeAsync(string phone);
        bool ValidatePhone(string phone);

        Task<bool> UserExistsAsync(string phone);
        Task<User> FindUserByIdAsync(int id);

        Task<User> SignUpUserAsync(UserSignUpDTO userData);
        Task<User> SignInUserAsync(UserSignInDTO userData);

        Task InvalidateTokenAsync(int userId, string refreshToken);
        Task SaveUserTokenAsync(User user, string refreshToken);
    }
}
