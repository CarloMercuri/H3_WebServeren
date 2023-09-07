using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace H3_WebServeren.ControllersControl.Attributes
{
    internal class RequestRoute : Attribute
    {
        public string Route { get; set; }

        public RequestRoute(string route)
        {
            Route = route;
        }
    }
}
