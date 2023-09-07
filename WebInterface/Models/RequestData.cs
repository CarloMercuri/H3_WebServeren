using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace H3_WebServeren.WebInterface.Models
{
    public class RequestData
    {
        public string HttpMethod { get; set; }
        public string RequestPath { get; set; }

        public List<RequestParameter> Parameters { get; set; } = new List<RequestParameter>();

        public RequestData()
        {

        }

        public RequestData(string _method, string _url)
        {
            HttpMethod = _method;
            RequestPath = _url;
        }
	}
}
