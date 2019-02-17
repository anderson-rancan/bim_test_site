using BimManufact.Web.Clients;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BimManufact.Web.Controllers
{
    public class ManufacturerController : Controller
    {
        private readonly IManufacturerClient _manufacturerClient;
        private readonly IProductClient _productClient;

        public ManufacturerController(IProductClient productClient, IManufacturerClient manufacturerClient)
        {
            _manufacturerClient = manufacturerClient;
            _productClient = productClient;
        }

        public ActionResult Index(int manufacturerId)
        {
            return View();
        }

        public ActionResult Create(int manufacturerId)
        {
            return View();
        }

        public ActionResult Update(int manufacturerId)
        {
            return View();
        }
    }
}