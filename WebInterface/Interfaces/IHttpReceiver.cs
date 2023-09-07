using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace H3_WebServeren.WebInterface.Interfaces
{
    public interface IHttpReceiver
    {
        bool StartServer(IPAddress ipAddress, int port, int maxNOfCon, string contentPath);
    }
}
