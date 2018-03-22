using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarHelp.Options
{
    // TODO: store it in configuration file
    internal class AuthOptions
    {
        public const string ISSUER = "CarHelpServer";

        public const string ACCESS_AUDIENCE = "CarHelpClient";
        public const string REFRESH_AUDIENCE = "CarHelpRefreshToken";
        const string KEY = "token_secret_key_fhsy23#4&sd*fd33";

        public const int REFRESH_LIFETIME = 180 * 24 * 60;
        public const int ACCESS_LIFETIME = 15;

        public static SymmetricSecurityKey GetSymmetricSecurityKey()
        {
            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(KEY));
        }
    }

    internal enum TokenType
    {
        Refresh = 0,
        Access = 1
    }
}
