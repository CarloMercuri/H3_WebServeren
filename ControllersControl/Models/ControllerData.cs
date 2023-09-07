using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace H3_WebServeren.ControllersControl.Models
{
    public class ControllerData
    {
        public H3ControllerGroup controllerInstance { get; set; }
        public Dictionary<string, string> controllerEndpoints = new Dictionary<string, string>();
        public List<EndpointData> EndPoints = new List<EndpointData>();
        public string BaseRoute { get; set; }
    }
}
