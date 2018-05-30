using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Text;

namespace CarHelp.AppLayer
{
    public class AuthOptions
    {
        public string Issuer { get; set; }

        public string AccessAudience { get; set; }
        public string RefreshAudience { get; set; }
        public string Key { get; set; }

        // TODO: Change access lifetime to 15 minutes
        public static readonly int REFRESH_LIFETIME = 180 * 24 * 60;
        public static readonly int ACCESS_LIFETIME = 180 * 24 * 60;

        public SymmetricSecurityKey GetSymmetricSecurityKey()
        {
            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Key));
        }
    }
}
