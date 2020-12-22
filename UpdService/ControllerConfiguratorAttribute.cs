using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Formatting;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using Newtonsoft.Json.Serialization;

namespace UpdService
{
    public class CustomJsonAttribute : Attribute, IControllerConfiguration
    {
        public void Initialize(HttpControllerSettings controllerSettings, HttpControllerDescriptor controllerDescriptor)
        {
            var formatter = controllerSettings.Formatters.JsonFormatter;

            controllerSettings.Formatters.Remove(formatter);

            formatter = new JsonMediaTypeFormatter
            {
                SerializerSettings =
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                }
            };

            controllerSettings.Formatters.Insert(0, formatter);
        }
    }
}
