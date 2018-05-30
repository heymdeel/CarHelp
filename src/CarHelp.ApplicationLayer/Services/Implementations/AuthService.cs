using AutoMapper;
using CarHelp.AppLayer.Models.DTO;
using CarHelp.DAL.Entities;
using CarHelp.DAL.Repositories;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CarHelp.AppLayer.Services
{
    public class AuthService : IAuthService
    {
        private static Random rnd = new Random();

        private readonly ISmsService smsService;
        private readonly IRepository<User> usersRepository;
        private readonly IRepository<UserProfile> profilesRepository;
        private readonly AuthOptions authOptions;

        public AuthService(ISmsService smsService, IRepository<User> usersRepository, IRepository<UserProfile> profilesRepository, IOptions<AuthOptions> authOptions)
        {
            this.smsService = smsService;
            this.usersRepository = usersRepository;
            this.profilesRepository = profilesRepository;

            this.authOptions = authOptions.Value;
        }

        public async Task<User> SignUpUserAsync(SignUpInput userData)
        {
            if (!await smsService.CodeIsValidAsync(userData.Phone, userData.SmsCode))
            {
                throw new BadInputException("code is invalid");
            }

            if (await usersRepository.FirstOrDefaultAsync(u => u.Phone == userData.Phone) != null)
            {
                throw new BadInputException("user already exists");
            }

            // it would be better to move this somewhere
            var user = Mapper.Map<User>(userData);
            user.Roles = new[] { "client", "worker" };
            user.DateOfRegistration = DateTime.Now;
            user.Profile.Phone = user.Phone;

            // linq2db can't save both entities in 1 method call
            user.Profile.Id = user.Id = await usersRepository.InsertWithIdAsync(user);
            await profilesRepository.InsertAsync(user.Profile);

            return user;
        }

        public async Task<User> SignInUserAsync(SignInInput userData)
        {
            if (!await smsService.CodeIsValidAsync(userData.Phone, userData.SmsCode))
            {
                throw new BadInputException("invalid code");
            }

            var user = await usersRepository.FirstOrDefaultAsync(u => u.Phone == userData.Phone);
            if (user == null)
            {
                throw new BadInputException("user was not found");
            }

            return user;
        }

        public async Task<(string refresh, string access)> GenerateAndStoreTokensAsync(User user)
        {
            ClaimsIdentity identity = GetIdentity(user);

            string refreshToken = GenerateToken(identity, TokenType.Refresh);
            string accessToken = GenerateToken(identity, TokenType.Access);

            user.RefreshToken = refreshToken;
            await usersRepository.UpdateAsync(user);

            return (refreshToken, accessToken);
        }

        public async Task<((string refresh, string access), User user)> RefreshTokenAsync(string refreshToken)
        {
            int userId = GetUserIdFromToken(refreshToken);

            var user = await usersRepository.FirstOrDefaultAsync(u => u.Id == userId && u.RefreshToken == refreshToken);
            if (user == null || !TokenIsValid(refreshToken)) // bad token format or token wasn't found in storage
            {
                throw new BadInputException("token is invalid");
            }

            var tokens = await GenerateAndStoreTokensAsync(user);

            return (tokens, user);
        }

        public async Task InvalidateTokenAsync(string refreshToken)
        {
            int userId = GetUserIdFromToken(refreshToken);

            var user = await usersRepository.FirstOrDefaultAsync(u => u.Id == userId && u.RefreshToken == refreshToken);
            if (user == null || !TokenIsValid(refreshToken)) // bad token format or token wasn't found in storage
            {
                throw new BadInputException("token is invalid");
            }

            user.RefreshToken = String.Empty;
            await usersRepository.UpdateAsync(user);
        }

        private ClaimsIdentity GetIdentity(User user)
        {
            var claims = new List<Claim>
            {
                new Claim("user_id", user.Id.ToString())
            };

            claims.AddRange(user.Roles.Select(r => new Claim(ClaimsIdentity.DefaultRoleClaimType, r)));

            return new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
        }

        private string GenerateToken(ClaimsIdentity identity, TokenType tokenType)
        {
            var jwt = new JwtSecurityToken(
                    issuer: authOptions.Issuer,
                    audience: tokenType == TokenType.Refresh ? authOptions.RefreshAudience : authOptions.AccessAudience,
                    notBefore: DateTime.Now,
                    claims: identity.Claims,
                    expires: DateTime.Now.AddMinutes(tokenType == TokenType.Refresh ? AuthOptions.REFRESH_LIFETIME : AuthOptions.ACCESS_LIFETIME),
                    signingCredentials: new SigningCredentials(authOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));

            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }

        private bool TokenIsValid(string refreshToken)
        {
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = authOptions.Issuer,

                ValidateAudience = true,
                ValidAudience = authOptions.RefreshAudience,

                ValidateLifetime = true,

                IssuerSigningKey = authOptions.GetSymmetricSecurityKey(),
                ValidateIssuerSigningKey = true,
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken validatedToken = null;
            try
            {
                tokenHandler.ValidateToken(refreshToken, validationParameters, out validatedToken);
            }
            catch (SecurityTokenException)
            {
                return false;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return false;
            }

            return true;
        }

        private int GetUserIdFromToken(string refreshToken) => int.Parse(new JwtSecurityTokenHandler().ReadJwtToken(refreshToken).Claims.FirstOrDefault(c => c.Type == "user_id").Value);

    }
}
