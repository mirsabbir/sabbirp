using Microsoft.PointOfService;
using System;
using System.Collections.Generic;
using System.Text;

namespace LiveMenuPrinter
{
    [Serializable]
    public class PrintingException : Exception
    {
        public PrintingException(string message, Exception ex) : base(message, ex)
        {
            
        }
    }
}
