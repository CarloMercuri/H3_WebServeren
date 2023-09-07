using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace H3_WebServeren.ControllersControl.Models
{
    public class ControllerObjectResponse : IRequestResponse
    {
        private HttpStatusCode StatusCode { get; set; }
        private object? Content { get; set; }
        public ControllerObjectResponse(HttpStatusCode code, object? content)
        {
            StatusCode = code;
            Content = content;
        }

        public HttpStatusCode GetStatusCode()
        {
            return StatusCode;
        }

        public object? GetContent()
        {
            return Content;
        }
    }
}
