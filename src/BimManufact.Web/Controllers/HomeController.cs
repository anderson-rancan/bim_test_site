using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Mvc;
using BimManufact.Web.Models;

namespace BimManufact.Web.Controllers
{
    public class HomeController : ControllerBase
    {
        public async Task<ActionResult> Index()
        {
            IEnumerable<ManufacturerViewModel> members = null;

            using (var client = GetWebApiClient())
            {
                var response = await client.GetAsync("manufacturers");

                if (response.IsSuccessStatusCode)
                {
                    members = await response.Content.ReadAsAsync<IList<ManufacturerViewModel>>();
                }
                else
                {
                    members = Enumerable.Empty<ManufacturerViewModel>();
                    ModelState.AddModelError(string.Empty, "Server error, please try again.");
                }
            }

            return View(members.OrderBy(_ => _.Name));
        }

        public ActionResult Create()
        {
            return View(new ManufacturerRequestViewModel());
        }

        [HttpPost]
        public async Task<ActionResult> Create(ManufacturerRequestViewModel request)
        {
            using (var client = GetWebApiClient())
            {
                var result = await client.PostAsJsonAsync($"manufacturers", request);

                if (result.IsSuccessStatusCode)
                {
                    TempData["SuccessAlert"] = $"The '{ request.Name }' manufacturer was successfully created!";
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Server error, please try again.");
                }
            }

            return View(request);
        }

        public async Task<ActionResult> Update(int id = -1)
        {
            if (id < 0)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest, "Mandatory argument was not specified");
            }

            ManufacturerRequestViewModel request = null;

            using (var client = GetWebApiClient())
            {
                var response = await client.GetAsync($"manufacturers/{id}");

                if (response.IsSuccessStatusCode)
                {
                    request = await response.Content.ReadAsAsync<ManufacturerRequestViewModel>();
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Server error, please try again.");
                }
            }

            return View(request);
        }

        [HttpPost]
        public async Task<ActionResult> Update(ManufacturerRequestViewModel request)
        {
            using (var client = GetWebApiClient())
            {
                var result = await client.PutAsJsonAsync($"manufacturers/{request.ManufacturerId}", request);

                if (result.IsSuccessStatusCode)
                {
                    TempData["SuccessAlert"] = "The manufacturer was successfully updated!";
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Server error, please try again.");
                }
            }

            return View(request);
        }
    }
}