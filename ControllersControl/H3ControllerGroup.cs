using H3_WebServeren.ControllersControl.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace H3_WebServeren.ControllersControl
{
    public abstract class H3ControllerGroup
    {
        public ControllerObjectResponse Ok(object? value)
        {
            return new ControllerObjectResponse(HttpStatusCode.OK, value);
        }

        public ControllerObjectResponse Ok()
        {
            return new ControllerObjectResponse(HttpStatusCode.OK, null);
        }

        public ControllerObjectResponse BadRequest(object? value)
        {
            return new ControllerObjectResponse(HttpStatusCode.BadRequest, value);

        }

        public ControllerObjectResponse NotFound(object? value)
        {
            return new ControllerObjectResponse(HttpStatusCode.NotFound, value);

        }

        public ControllerObjectResponse InternalError(object? value)
        {
            return new ControllerObjectResponse(HttpStatusCode.InternalServerError, value);

        }

        public ControllerObjectResponse StatusCodeResponse(HttpStatusCode code, object? value)
        {
            return new ControllerObjectResponse(code, value);
        }

    }
}
