using System;
using System.Collections.Generic;
using System.Text;

namespace CarHelp.AppLayer
{
    public class AccessRefusedException : AppLayerException
    {
        public AccessRefusedException(string message) : base(message) { }
    }
}
