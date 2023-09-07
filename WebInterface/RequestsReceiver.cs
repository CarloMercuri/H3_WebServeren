using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Unicode;
using System.Threading.Tasks;
using H3_WebServeren.ControllersControl;
using H3_WebServeren.ControllersControl.Interfaces;
using H3_WebServeren.ControllersControl.Models;
using H3_WebServeren.WebInterface.Interfaces;
using H3_WebServeren.WebInterface.Models;
using static System.Net.WebRequestMethods;
using File = System.IO.File;

namespace H3_WebServeren.WebInterface
{
    public class RequestsReceiver : IHttpReceiver
    {
        public bool running = false; // Is it running?

        private int timeout = 8; // Time limit for data transfers.
        private Encoding charEncoder = Encoding.UTF8; // To encode string
        private Socket serverSocket; // Our server socket
        private string contentPath; // Root path of our contents

        //
        private IRequestDataExtractor _extractor;
        private IControllersManager _controllersManager;
        private IHttpResponseFactory _responseFactory;

        public RequestsReceiver(IRequestDataExtractor extractor, IControllersManager manager, IHttpResponseFactory responseFactory)
        {
            _extractor = extractor;
            _controllersManager = manager;
            _responseFactory = responseFactory;
        }

        public bool StartServer(IPAddress ipAddress, int port, int maxNOfCon, string contentPath)
        {
            if (running) return false; // If it is already running, exit.

            try
            {
                serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream,
                               ProtocolType.Tcp);
                serverSocket.Bind(new IPEndPoint(ipAddress, port));
                serverSocket.Listen(maxNOfCon);
                serverSocket.ReceiveTimeout = timeout;
                serverSocket.SendTimeout = timeout;
                running = true;
                this.contentPath = contentPath;
            }
            catch
            {
                return false;
            }

            // Our thread that will listen connection requests
            // and create new threads to handle them.
            Thread requestListenerT = new Thread(() =>
            {
                while (running)
                {
                    Socket clientSocket;
                    try
                    {
                        clientSocket = serverSocket.Accept();
                        // Create new thread to handle the request and continue to listen the socket.
                        Thread requestHandler = new Thread(() =>
                        {
                            clientSocket.ReceiveTimeout = timeout;
                            clientSocket.SendTimeout = timeout;
                            try { HandleTheRequest(clientSocket); }
                            catch
                            {
                                try { clientSocket.Close(); } catch { }
                            }
                        });
                        requestHandler.Start();
                    }
                    catch { }
                }
            });
            requestListenerT.Start();

            return true;
        }

        public void stop()
        {
            if (running)
            {
                running = false;
                try { serverSocket.Close(); }
                catch { }
                serverSocket = null;
            }
        }

        private void HandleTheRequest(Socket clientSocket)
        {
            byte[] buffer = new byte[10240]; // 10 kb, just in case
            int receivedBCount = clientSocket.Receive(buffer); // Receive the request
            string strReceived = charEncoder.GetString(buffer, 0, receivedBCount);

            // Parse method of the request
            RequestData _reqData = _extractor.ExtractRequestData(strReceived);

            if (_reqData is null)
            {
                HttpResponseData errorResponse = _responseFactory.BuildInternalErrorResponse("Corrupted data");
                sendResponse(clientSocket, errorResponse.Content, errorResponse.StatusCode, errorResponse.ContentType);
                return;
            }

            // Send the request on to the controllers
            IRequestResponse responseFromController = _controllersManager.ProcessRequest(_reqData);           

            // Build a properly formatted response
            HttpResponseData builtResponse = _responseFactory.BuildResponseBody(responseFromController);

            // Send the response
            sendResponse(clientSocket, builtResponse.Content, builtResponse.StatusCode, builtResponse.ContentType);
                     
        }

        private void notImplemented(Socket clientSocket)
        {


            sendResponse(clientSocket, "", "", "");
        }   

        private void sendResponse(Socket clientSocket, string strContent, string responseCode,
                          string contentType)
        {
            byte[] bContent = charEncoder.GetBytes(strContent);
            sendResponse(clientSocket, bContent, responseCode, contentType);
        }
       
        // For byte arrays
        private void sendResponse(Socket clientSocket, byte[] bContent, string responseCode,
                                  string contentType)
        {
            try
            {
                byte[] bHeader = charEncoder.GetBytes(
                                    "HTTP/1.1 " + responseCode + "\r\n"
                                  + "Server: Atasoy Simple Web Server\r\n"
                                  + "Content-Length: " + bContent.Length.ToString() + "\r\n"
                                  + "Connection: close\r\n"
                                  + "Content-Type: " + contentType + "\r\n\r\n");
                clientSocket.Send(bHeader);
                clientSocket.Send(bContent);
                clientSocket.Close();
            }
            catch { }
        }
    }
}
