using System;
using System.Collections.Generic;
using System.Text;

namespace CarHelp.AppLayer
{
    public class AppException : Exception
    {
        public AppException(string message) : base(message) { }
    }
}
