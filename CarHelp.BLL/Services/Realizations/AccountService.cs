using AutoMapper;
using CarHelp.BLL.Model.DTO;
using CarHelp.DAL;
using CarHelp.DAL.Entities;
using CarHelp.DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CarHelp.BLL.Services
{
    public class AccountService : IAccountService
    {
        private readonly IRepository<User> usersRepository;
        private readonly IRepository<UserProfile> profilesRepository;

        public AccountService(IRepository<User> usersRepository, IRepository<UserProfile> profilesRepository)
        {
            this.usersRepository = usersRepository;
            this.profilesRepository = profilesRepository;
        }

        public async Task<bool> UserExistsAsync(string phone)
        {
            return await usersRepository.FirstOrDefaultAsync(filter: u => u.Phone == phone) != null;
        }

        public async Task<User> FindUserByIdAsync(int id)
        {
            return await usersRepository.FirstOrDefaultAsync(filter: u => u.Id == id);
        }

        public async Task<UserProfile> GetUserProfileAsync(int userId)
        {
            return await profilesRepository.FirstOrDefaultAsync(filter: p => p.Id == userId);
        }

        public async Task<User> SignUpUserAsync(UserSignUpDTO userData)
        {
            var user = new User
            {
                Phone = userData.Phone,
                DateOfRegistration = DateTime.Now,
                Roles = new string[] { "client", "worker" }
            };

            int userId = await usersRepository.InsertWithIdAsync(user);
            User createdUser = await usersRepository.FirstOrDefaultAsync(filter: u => u.Id == userId);

            var userProfile = Mapper.Map<UserProfile>(userData);
            userProfile.Id = createdUser.Id;

            await profilesRepository.InsertAsync(userProfile);

            return createdUser;
        }

        public async Task<User> SignInUserAsync(UserSignInDTO userData)
        {
            return await usersRepository.FirstOrDefaultAsync(filter: u => u.Phone == userData.Phone);
        }

        public async Task InvalidateTokenAsync(int userId, string refreshToken)
        {
            User user = await usersRepository.FirstOrDefaultAsync(u => u.Id == userId && u.RefreshToken == refreshToken);
            if (user == null)
            {
                return;
            }

            user.RefreshToken = String.Empty;

            await usersRepository.UpdateAsync(user);
        }

        public async Task StoreRefreshTokenAsync(User user, string refreshToken)
        {
            user.RefreshToken = refreshToken;
            await usersRepository.UpdateAsync(user);
        }
    }
}
