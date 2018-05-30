using System;
using System.Collections.Generic;
using System.Text;

namespace CarHelp.AppLayer
{
    public class AppLayerException : Exception
    {
        public AppLayerException(string message) : base(message) { }
    }
}
