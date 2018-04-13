using CarHelp.AppLayer.Models.DTO;
using CarHelp.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CarHelp.AppLayer.Services
{
    public interface IAccountService
    {
        Task<bool> UserExistsAsync(string phone);
        Task<User> FindUserByIdAsync(int id);
        Task<UserProfile> GetUserProfileAsync(int userId);

        Task<User> SignUpUserAsync(UserSignUpDTO userData);
        Task<User> SignInUserAsync(UserSignInDTO userData);

        Task InvalidateTokenAsync(int userId, string refreshToken);
        Task StoreRefreshTokenAsync(User user, string refreshToken);
    }
}
