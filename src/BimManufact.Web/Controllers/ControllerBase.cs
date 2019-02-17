using System;
using System.Configuration;
using System.Net.Http;
using System.Web.Mvc;

namespace BimManufact.Web.Controllers
{
    public abstract class ControllerBase : Controller
    {
        private readonly string _webApiConfigVariable = "webapiurl";

        protected HttpClient GetWebApiClient()
        {
            return new HttpClient
            {
                BaseAddress = new Uri(ConfigurationManager.AppSettings[_webApiConfigVariable])
            };
        }
    }
}