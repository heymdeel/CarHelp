using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CarHelp.AppLayer.Services
{
    public interface ISmsService
    {
        bool PhoneNumberIsValid(string phone);
        Task<bool> CodeIsValidAsync(string phone, int code);
        Task SendCodeAsync(string phone);
    }
}
