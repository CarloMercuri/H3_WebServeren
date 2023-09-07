using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace H3_WebServeren.ControllersControl.Models
{
    public class EndpointData
    {
        public string Route { get; set; }
        public string HttpMethod { get; set; }
        public MethodInfo MethodInfo { get; set; }

    }
}
