using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace H3_WebServeren.ControllersControl.Attributes
{
    internal class RequestMethod : Attribute
    {
        public string Method { get; set; }

        public RequestMethod(string method)
        {
            Method = method;
        }
    }
}
