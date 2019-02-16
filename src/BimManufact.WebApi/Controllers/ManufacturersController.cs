using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using System.Web.Http.OData;
using System.Web.Http.OData.Routing;
using BimManufact.WebApi.Models;

namespace BimManufact.WebApi.Controllers
{
    /*
    The WebApiConfig class may require additional changes to add a route for this controller. Merge these statements into the Register method of the WebApiConfig class as applicable. Note that OData URLs are case sensitive.

    using System.Web.Http.OData.Builder;
    using System.Web.Http.OData.Extensions;
    using BimManufact.WebApi.Models;
    ODataConventionModelBuilder builder = new ODataConventionModelBuilder();
    builder.EntitySet<Manufacturer>("Manufacturers");
    config.Routes.MapODataServiceRoute("odata", "odata", builder.GetEdmModel());
    */
    public class ManufacturersController : ODataController
    {
        private BimManufactWebApiContext db = new BimManufactWebApiContext();

        // GET: odata/Manufacturers
        [EnableQuery]
        public IQueryable<Manufacturer> GetManufacturers()
        {
            return db.Manufacturers;
        }

        // GET: odata/Manufacturers(5)
        [EnableQuery]
        public SingleResult<Manufacturer> GetManufacturer([FromODataUri] int key)
        {
            return SingleResult.Create(db.Manufacturers.Where(manufacturer => manufacturer.Id == key));
        }

        // PUT: odata/Manufacturers(5)
        public async Task<IHttpActionResult> Put([FromODataUri] int key, Delta<Manufacturer> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Manufacturer manufacturer = await db.Manufacturers.FindAsync(key);
            if (manufacturer == null)
            {
                return NotFound();
            }

            patch.Put(manufacturer);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ManufacturerExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(manufacturer);
        }

        // POST: odata/Manufacturers
        public async Task<IHttpActionResult> Post(Manufacturer manufacturer)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Manufacturers.Add(manufacturer);
            await db.SaveChangesAsync();

            return Created(manufacturer);
        }

        // PATCH: odata/Manufacturers(5)
        [AcceptVerbs("PATCH", "MERGE")]
        public async Task<IHttpActionResult> Patch([FromODataUri] int key, Delta<Manufacturer> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Manufacturer manufacturer = await db.Manufacturers.FindAsync(key);
            if (manufacturer == null)
            {
                return NotFound();
            }

            patch.Patch(manufacturer);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ManufacturerExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(manufacturer);
        }

        // DELETE: odata/Manufacturers(5)
        public async Task<IHttpActionResult> Delete([FromODataUri] int key)
        {
            Manufacturer manufacturer = await db.Manufacturers.FindAsync(key);
            if (manufacturer == null)
            {
                return NotFound();
            }

            db.Manufacturers.Remove(manufacturer);
            await db.SaveChangesAsync();

            return StatusCode(HttpStatusCode.NoContent);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ManufacturerExists(int key)
        {
            return db.Manufacturers.Count(e => e.Id == key) > 0;
        }
    }
}
