using H3_WebServeren.WebInterface.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace H3_WebServeren.WebInterface.Models
{
    internal class RequestDataExtractor : IRequestDataExtractor
    {

        public RequestData ExtractRequestData(string input)
        {
            try
            {
                RequestData data = new RequestData();
                data.HttpMethod = GetHttpMethod(input);
                string fullQuery = GetQueryString(input, data.HttpMethod);
                string[] queryParamsSplit = fullQuery.Split("?");
                data.RequestPath = queryParamsSplit[0]; // there will always be a [0]

                if(queryParamsSplit.Length > 1)
                {
                    data.Parameters = ParseParameters(queryParamsSplit[1]);
                }

                return data;
            }
            catch (Exception ex)
            {
                return null;
            }

        }

        private List<RequestParameter> ParseParameters(string parametersString)
        {
            List<RequestParameter> returnList = new List<RequestParameter>();   

            // Split into parameters groups with the & sign.
            // So we'll have a list of strings like f.ex: [ "id=34", "name=Carlo", "date=2023-04-03" ]

            string[] paramsSplit = parametersString.Split('&');

            foreach(string paramGroup in paramsSplit)
            {
                // Split into key and value with '='
                string[] keyValueSplit = paramGroup.Split('=');
                RequestParameter rp = new RequestParameter();
                rp.Name = keyValueSplit[0];
                rp.Value = keyValueSplit[1];

                returnList.Add(rp);
            }

            return returnList;
            
        }

        private string GetHttpMethod(string input)
        {            
            return input.Substring(0, input.IndexOf(" "));
        }

        /// <summary>
        /// Get the query string, without the ip address. Ex. /Files/Get/{id}
        /// </summary>
        /// <param name="method"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        private string GetQueryString(string input, string method)
        {
            int start = input.IndexOf(method) + method.Length + 1;
            int length = input.LastIndexOf("HTTP") - start - 1;
            string requestedUrl = input.Substring(start, length); // after the address. ex: /tasks/build/1

            return requestedUrl;
        }
    }
}
