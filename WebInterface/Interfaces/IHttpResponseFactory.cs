using H3_WebServeren.ControllersControl.Models;
using H3_WebServeren.WebInterface.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace H3_WebServeren.WebInterface.Interfaces
{
    public interface IHttpResponseFactory
    {
        HttpResponseData BuildResponseBody(IRequestResponse responseData);
        HttpResponseData BuildInternalErrorResponse(string message);
    }
}
