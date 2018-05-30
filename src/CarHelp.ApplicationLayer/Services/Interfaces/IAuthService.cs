using CarHelp.AppLayer.Models.DTO;
using CarHelp.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CarHelp.AppLayer.Services
{
    public interface IAuthService
    {
        Task<User> SignUpUserAsync(SignUpInput userData);
        Task<User> SignInUserAsync(SignInInput userData);

        Task<(string refresh, string access)> GenerateAndStoreTokensAsync(User user);
        Task<((string refresh, string access), User user)> RefreshTokenAsync(string refreshToken);
        Task InvalidateTokenAsync(string refreshToken);
    }
}
