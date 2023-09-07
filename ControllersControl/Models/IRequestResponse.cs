using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace H3_WebServeren.ControllersControl.Models
{
    public interface IRequestResponse
    {
        HttpStatusCode GetStatusCode();
        object? GetContent();
    }
}
