using BimManufact.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;

namespace BimManufact.Web.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            IEnumerable<ManufacturerViewModel> members = null;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:50335/api/");

                var responseTask = client.GetAsync("manufacturers");
                responseTask.Wait();

                var result = responseTask.Result;

                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<IList<ManufacturerViewModel>>();
                    readTask.Wait();

                    members = readTask.Result;
                }
                else
                {
                    members = Enumerable.Empty<ManufacturerViewModel>();

                    ModelState.AddModelError(string.Empty, "Server error try after some time.");
                }
            }

            return View(members);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Create()
        {
            return View();
        }

        public ActionResult Update()
        {
            return View();
        }
    }
}