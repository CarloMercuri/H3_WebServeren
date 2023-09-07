using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace H3_WebServeren.WebInterface.Models
{
    public class HttpResponseData
    {
        public string StatusCode { get; set; }
        public string Content { get; set; }
        public string ContentType { get; set; }
    }
}
