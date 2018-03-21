using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CarHelp.BLL.Services
{
    public interface IAccountService
    {
        Task GenerateSmsCode(string phone);
        bool ValidatePhone(string phone);
    }
}
