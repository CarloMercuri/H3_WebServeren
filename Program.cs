// See https://aka.ms/new-console-template for more information
using H3_WebServeren.ControllersControl;
using H3_WebServeren.ControllersControl.Interfaces;
using H3_WebServeren.WebInterface;
using H3_WebServeren.WebInterface.Interfaces;
using H3_WebServeren.WebInterface.Models;
using System.Net;

Console.WriteLine("Hello, World!");

// Initialize interfaces
IRequestDataExtractor _extractor = new RequestDataExtractor();
IControllersManager _controllersManager = new ControllersManager();
IHttpResponseFactory _factory = new HttpResponsesFactory();
IHttpReceiver _receiver = new RequestsReceiver(_extractor, _controllersManager, _factory);

// Load the controllers
_controllersManager.InitializeControllers();

IPAddress add = IPAddress.Parse("127.0.0.1");

try
{
    bool startSuccess = _receiver.StartServer(add, 8080, 10, "");

    if (!startSuccess)
    {
        Console.WriteLine("Server failed to start, unknown reason");
        Console.WriteLine("Press any key to stop the program.");
        Console.ReadKey();
    }
    else
    {
        Console.WriteLine("Server started. Press S to stop");

        ConsoleKeyInfo userKeyPress = Console.ReadKey();


    }
}
catch (Exception ex)
{
    Console.WriteLine($"Server failed to start, reason: {ex.Message}");
    Console.WriteLine("Press any key to stop the program.");
    Console.ReadKey();
}


