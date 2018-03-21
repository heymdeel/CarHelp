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

        public AccountService(IRepository<SmsCode> smsCodesRepository)
        {
            this.smsCodesRepository = smsCodesRepository;
        }

        public bool ValidatePhone(string phone) => Regex.IsMatch(phone, "^[7-8][0-9]{10}$");

        public async Task GenerateSmsCode(string phone)
        {
            int code = rnd.Next(1000, 9999);
            var sms = await smsCodesRepository.FirstOrDefaultAsync(s => s.Phone == phone);

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
        }
    }
}
