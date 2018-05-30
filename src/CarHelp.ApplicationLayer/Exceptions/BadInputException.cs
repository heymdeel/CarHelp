using System;
using System.Collections.Generic;
using System.Text;

namespace CarHelp.AppLayer
{
    public class BadInputException : AppException
    {
        public BadInputException(string message) : base(message) { }
    }
}
