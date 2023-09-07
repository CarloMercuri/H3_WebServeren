using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace H3_WebServeren.ControllersControl.Attributes
{
    internal class ControllerBaseAddress : Attribute
    {
        public string BaseAddress { get; set; }

        public ControllerBaseAddress(string baseAddress)
        {
            BaseAddress = baseAddress;  
        }
    }
}
