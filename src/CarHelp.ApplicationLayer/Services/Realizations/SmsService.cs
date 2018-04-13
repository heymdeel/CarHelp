using CarHelp.DAL.Entities;
using CarHelp.DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CarHelp.AppLayer.Services
{
    public class SmsService : ISmsService
    {
        private static Random rnd = new Random();
        private readonly IRepository<SmsCode> smsRepository;

        public SmsService(IRepository<SmsCode> smsRepository)
        {
            this.smsRepository = smsRepository;
        }

        public bool ValidatePhone(string phone) => Regex.IsMatch(phone, "^[7][0-9]{10}$");

        public async Task<bool> CodeIsValidAsync(string phone, int code)
        {
            SmsCode sms = await smsRepository.FirstOrDefaultAsync(filter: s => s.Phone == phone && s.Code == code);

            if (sms != null)
            {
                await smsRepository.RemoveAsync(sms);
                return true;
            }

            return false;
        }

        public async Task SendCodeAsync(string phone)
        {
            int code = rnd.Next(1000, 9999);
            SmsCode sms = await smsRepository.FirstOrDefaultAsync(filter: s => s.Phone == phone);

            if (sms == null)
            {
                await smsRepository.InsertAsync(new SmsCode { Phone = phone, Code = code, TimeStamp = DateTime.Now });
            }
            else
            {
                sms.Code = code;
                sms.TimeStamp = DateTime.Now;
                await smsRepository.UpdateAsync(sms);
            }

            // TODO: send SMS via some service
        }
    }
}
