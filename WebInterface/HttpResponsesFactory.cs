using H3_WebServeren.ControllersControl.Models;
using H3_WebServeren.WebInterface.Interfaces;
using H3_WebServeren.WebInterface.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace H3_WebServeren.WebInterface
{
    public class HttpResponsesFactory : IHttpResponseFactory
    {
        public HttpResponseData BuildResponseBody(IRequestResponse responseData)
        {
            HttpResponseData response = new HttpResponseData();
            response.StatusCode = FormatStatusCode(responseData.GetStatusCode());
            object? content = responseData.GetContent();

            if(content is null)
            {
                response.Content = FormatStatusCode(responseData.GetStatusCode());
            }
            else
            {
                if (content.GetType().IsClass)
                {
                    response.Content = Serializeclass(content);
                    response.ContentType = "application/json";
                }
                else if (content.GetType().IsEnum)
                {
                    response.Content = "NOT Implemented - enum";
                    response.ContentType = "text/plain";
                }
                else
                {
                    response.Content = content.ToString();
                    response.ContentType = "text/plain";

                }
            }

            return response;
        }

        public HttpResponseData BuildInternalErrorResponse(string message)
        {

            HttpResponseData response = new HttpResponseData();
            response.StatusCode = FormatStatusCode(HttpStatusCode.InternalServerError);
            response.ContentType = "text/plain";
            response.Content = message;

            return response;
        }

        private string Serializeclass(object input)
        {
            try
            {
                return JsonConvert.SerializeObject(input);
            }
            catch (Exception ex)
            {
                return $"Error serializing return content";
            }
        }

        private string FormatStatusCode(HttpStatusCode code)
        {
            return $"{(int)code} - {code.ToString()}";
        }

        private void MethodNotImplemented()
        {

            string content = "<html><head><meta"+
                "http - equiv =\"Content-Type\" content=\"text/html;\"" +
                "charset = utf - 8\">" +
                "</ head >< body >< h2 > Atasoy Simple Web" +
                "Server </ h2 >< div > 501 - Method Not" +
                "Implemented </ div ></ body ></ html > ";
                

                //"501 Not Implemented", "text/html");
        }

    }
}
