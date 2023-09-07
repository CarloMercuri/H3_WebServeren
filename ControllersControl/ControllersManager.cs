using H3_WebServeren.ControllersControl.Attributes;
using H3_WebServeren.ControllersControl.Interfaces;
using H3_WebServeren.ControllersControl.Models;
using H3_WebServeren.WebInterface.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using static System.Collections.Specialized.BitVector32;

namespace H3_WebServeren.ControllersControl
{
    public class ControllersManager : IControllersManager
    {

        List<ControllerData> controllers = new List<ControllerData>();

        public void InitializeControllers()
        {
            foreach(var controller in GetAllTypesDerivingFrom<H3ControllerGroup>())
            {
                var c = (H3ControllerGroup)Activator.CreateInstance(controller);
                ControllerData data = new ControllerData();

                // Select a base address for a controller. If specified with a ControllerBaseAddress attribute use that one
                // Else use the name of the class without "Controller". F.ex FilesController will give a base route "Files"
                ControllerBaseAddress baseRouteAttr = (ControllerBaseAddress)controller.GetCustomAttribute(typeof(ControllerBaseAddress), true);
                if(baseRouteAttr != null)
                {
                    data.BaseRoute = baseRouteAttr.BaseAddress.ToLower();
                }
                else
                {
                    // If the attribute is not present, cut Controller(if existing) from the name and use that
                    string s = controller.Name;
                    string fixedName = s.Remove(s.IndexOf("Controller"));
                    data.BaseRoute = fixedName.ToLower();
                }

                data.controllerInstance = c;
                data.EndPoints = GetEndpointsForController(controller);
                controllers.Add(data);
            }

            Console.WriteLine();
        }

        private List<EndpointData> GetEndpointsForController(Type controllerType)
        {
            List<EndpointData> endpoints = new List<EndpointData>();

            MethodInfo[] methods = controllerType.GetMethods();
            foreach(MethodInfo method in methods)
            {
                RequestRoute routeAttr = (RequestRoute)method.GetCustomAttribute(typeof(RequestRoute), true);
                if(routeAttr != null)
                {
                    RequestMethod methodAttr = (RequestMethod)method.GetCustomAttribute(typeof(RequestMethod), true);
                   
                    if(methodAttr != null)
                    {
                        string endPoint = routeAttr.Route;
                        string methodType = methodAttr.Method;
                        EndpointData data = new EndpointData();
                        data.HttpMethod = methodType;
                        data.Route = endPoint.ToLower();
                        data.MethodInfo = method;

                        endpoints.Add(data);
                    }

                }
            }

            return endpoints;
        }

        public IRequestResponse ProcessRequest(RequestData requestData)
        {
            // ex: /Files/Get?id=45&name=Carlo
            string[] pathSplit = requestData.RequestPath.Split('/');
            string baseControllerPath = pathSplit[1];

            // Go through the controllers we loaded on startup
            foreach(ControllerData data in controllers)
            {
                // If we find a controller that matches the base route
                if(data.BaseRoute == baseControllerPath.ToLower())
                {
                    // Get the pure query(without base path).
                    // F.ex a query like "localhost:8080/Files/Get/Single becomes Get/Single
                    string query = GetQuery(pathSplit);

                    if (string.IsNullOrEmpty(query))
                    {
                        return new ControllerObjectResponse(HttpStatusCode.BadRequest, null);
                    }

                    // go through the endpoints (methods) contained in the controller
                    for (int i = 0; i < data.EndPoints.Count; i++)
                    {
                        // If we find that the controller has a corresponding query path
                        if (data.EndPoints[i].Route == query.ToLower() && data.EndPoints[i].HttpMethod == requestData.HttpMethod)
                        {
                            try
                            {
                                IRequestResponse response = (IRequestResponse)data.EndPoints[i].MethodInfo.Invoke(data.controllerInstance, null);
                                return response;                                
                            }
                            catch (Exception ex)
                            {
                                return new ControllerObjectResponse(HttpStatusCode.InternalServerError, "Internal Server Error");
                            }                    
                        }                        
                    }

                    

                }
            }
            // No matching controller/query found
            return new ControllerObjectResponse(HttpStatusCode.NotFound, null);
        }

        /// <summary>
        /// Returns the query without parameters and base route
        /// </summary>
        /// <param name="pathSplit"></param>
        /// <returns></returns>
        private string GetQuery(string[] pathSplit)
        {
            try
            {
                string query = "";

                // if it's more than 2 sub categories, ex: Files(base url)/Get/Single, then we need to join them
                // otherwise just return the 2nd (in that example, Get)
                if (pathSplit.Length > 2)
                {
                    bool first = true;
                    // Skip the base controller path
                    for (int i = 2; i < pathSplit.Length; i++)
                    {
                        if (first)
                        {
                            query += pathSplit[i];
                            first = false;
                        }
                        else
                        {
                            query += "/" + pathSplit[i];
                        }
                    }
                }
                else
                {
                    // It will be the same as the base address
                    return pathSplit[1];
                }

                return query;
            }
            catch (Exception)
            {
                return "";
            }
           
        }

        /// <summary>
        /// Returns a Type list of all the classes that implement the T class/interface
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        private IEnumerable<Type> GetAllTypesDerivingFrom<T>()
        {
            return System.Reflection.Assembly.GetExecutingAssembly()
                .GetTypes()
                .Where(type => typeof(T).IsAssignableFrom(type) && !type.IsAbstract);
        }

    }
}
