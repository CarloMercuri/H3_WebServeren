using H3_WebServeren.ControllersControl;
using H3_WebServeren.ControllersControl.Attributes;
using H3_WebServeren.ControllersControl.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace H3_WebServeren.CustomControllers
{
    [ControllerBaseAddress("Files")]
    internal class FilesController : H3ControllerGroup
    {
        [RequestMethod("GET")]
        [RequestRoute("GetFile")]
        public IRequestResponse Lolo()
        {
            return Ok("GetFile text response");
        }

        [RequestMethod("GET")]
        [RequestRoute("Get/Single")]
        public IRequestResponse GetSingle()
        {
            Console.WriteLine();
            EndpointData d = new EndpointData();
            d.HttpMethod = "3030";
            d.MethodInfo = null;
            d.Route = "hello/siu";

            return Ok(d);
        }

        [RequestMethod("GET")]
        [RequestRoute("Get/Exception")]
        public IRequestResponse SendFile()
        {
            throw new Exception("Testing exceptions");
            return Ok("hi");
        }

        [RequestMethod("GET")]
        [RequestRoute("Get/ManualException")]
        public IRequestResponse ManualExceptions()
        {
            try
            {
                throw new Exception("Testing manual exceptions");
            }
            catch (Exception ex)
            {
                return InternalError(ex.Message);
            }
        }

    }
}
