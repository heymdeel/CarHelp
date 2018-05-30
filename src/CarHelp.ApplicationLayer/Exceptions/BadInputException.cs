using System;
using System.Collections.Generic;
using System.Text;

namespace CarHelp.AppLayer
{
    public class BadInputException : AppLayerException
    {
        public BadInputException(string message) : base(message) { }
    }
}
