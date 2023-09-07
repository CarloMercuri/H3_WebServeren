using H3_WebServeren.ControllersControl.Models;
using H3_WebServeren.WebInterface.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace H3_WebServeren.ControllersControl.Interfaces
{
    public interface IControllersManager
    {
        void InitializeControllers();
        IRequestResponse ProcessRequest(RequestData requestData);
    }
}
