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
        private static Random rnd = new Random();
        private readonly IRepository<SmsCode> smsCodesRepository;
        private readonly IRepository<User> usersRepository;
        private readonly IRepository<UserProfile> profilesRepository;

        public AccountService(IRepository<SmsCode> smsCodesRepository, IRepository<User> usersRepository, IRepository<UserProfile> profilesRepository)
        {
            this.smsCodesRepository = smsCodesRepository;
            this.usersRepository = usersRepository;
            this.profilesRepository = profilesRepository;
        }

        public bool ValidatePhone(string phone) => Regex.IsMatch(phone, "^[7][0-9]{10}$");

        public async Task GenerateSmsCodeAsync(string phone)
        {
            int code = rnd.Next(1000, 9999);
            SmsCode sms = await smsCodesRepository.FirstOrDefaultAsync(filter: s => s.Phone == phone);

            if (sms == null)
            {
                await smsCodesRepository.CreateAsync(new SmsCode() { Phone = phone, Code = code, TimeStamp = DateTime.Now });
            }
            else
            {
                sms.Code = code;
                sms.TimeStamp = DateTime.Now;
                await smsCodesRepository.UpdateAsync(sms);
            }

            // TODO: send SMS via some service
        }

        public async Task<User> SignUpUserAsync(UserSignUpDTO userData)
        {
            SmsCode sms = await smsCodesRepository.FirstOrDefaultAsync(filter: x => x.Code == userData.SmsCode && x.Phone == userData.Phone);
            if (sms == null)
            {
                return null;
            }

            var user = new User { Phone = userData.Phone, DateOfRegistration = DateTime.Now, Roles = new string[] { "client", "worker" } };
            await usersRepository.CreateAsync(user);
            User createdUser = await usersRepository.FirstOrDefaultAsync(filter: u => u.Phone == userData.Phone);

            var userProfile = Mapper.Map<UserProfile>(userData);
            userProfile.Id = createdUser.Id;

            await profilesRepository.CreateAsync(userProfile);
            await smsCodesRepository.RemoveAsync(sms);

            return createdUser;
        }

        public async Task<User> SignInUserAsync(UserSignInDTO userData)
        {
            SmsCode sms = await smsCodesRepository.FirstOrDefaultAsync(filter: s => s.Code == userData.SmsCode & s.Phone == userData.Phone);
            if (sms == null)
            {
                return null;
            }

            User user = await usersRepository.FirstOrDefaultAsync(filter: u => u.Phone == userData.Phone);
            await smsCodesRepository.RemoveAsync(sms);

            return user;
        }

        public async Task<bool> UserExistsAsync(string phone)
        {
            return await usersRepository.FirstOrDefaultAsync(filter: u => u.Phone == phone) != null;
        }

        public async Task SaveUserTokenAsync(User user, string refreshToken)
        {
            user.RefreshToken = refreshToken;
            await usersRepository.UpdateAsync(user);
        }

        public async Task<User> FindUserByIdAsync(int id)
        {
            return await usersRepository.FirstOrDefaultAsync(filter: u => u.Id == id);
        }

        public async Task InvalidateTokenAsync(int userId, string refreshToken)
        {
            User user = await usersRepository.FirstOrDefaultAsync(u => u.Id == userId && u.RefreshToken == refreshToken);
            if (user == null)
            {
                Console.WriteLine("user not found");
                return;
            }

            user.RefreshToken = "";

            await usersRepository.UpdateAsync(user);
        }
    }
}
