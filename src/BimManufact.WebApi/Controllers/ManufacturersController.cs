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
using System.Web.Http.Description;
using BimManufact.WebApi.Models;

namespace BimManufact.WebApi.Controllers
{
    public class ManufacturersController : ApiController
    {
        private IBimManufactWebApiContext _bimManufactWebApiContext;

        public ManufacturersController(IBimManufactWebApiContext bimManufactWebApiContext)
        {
            _bimManufactWebApiContext = bimManufactWebApiContext;
        }

        // GET: api/Manufacturers
        public IQueryable<Manufacturer> GetManufacturers()
        {
            return _bimManufactWebApiContext.Manufacturers;
        }

        // GET: api/Manufacturers/5
        [ResponseType(typeof(Manufacturer))]
        public async Task<IHttpActionResult> GetManufacturer(int id)
        {
            Manufacturer manufacturer = await _bimManufactWebApiContext.Manufacturers.FindAsync(id);
            if (manufacturer == null)
            {
                return NotFound();
            }

            return Ok(manufacturer);
        }

        // PUT: api/Manufacturers/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutManufacturer(int id, Manufacturer manufacturer)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != manufacturer.ManufacturerId)
            {
                return BadRequest();
            }

            _bimManufactWebApiContext.Entry(manufacturer).State = EntityState.Modified;

            try
            {
                await _bimManufactWebApiContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ManufacturerExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Manufacturers
        [ResponseType(typeof(Manufacturer))]
        public async Task<IHttpActionResult> PostManufacturer(Manufacturer manufacturer)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _bimManufactWebApiContext.Manufacturers.Add(manufacturer);
            await _bimManufactWebApiContext.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = manufacturer.ManufacturerId }, manufacturer);
        }

        // DELETE: api/Manufacturers/5
        [ResponseType(typeof(Manufacturer))]
        public async Task<IHttpActionResult> DeleteManufacturer(int id)
        {
            Manufacturer manufacturer = await _bimManufactWebApiContext.Manufacturers.FindAsync(id);
            if (manufacturer == null)
            {
                return NotFound();
            }

            _bimManufactWebApiContext.Manufacturers.Remove(manufacturer);
            await _bimManufactWebApiContext.SaveChangesAsync();

            return Ok(manufacturer);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _bimManufactWebApiContext.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ManufacturerExists(int id)
        {
            return _bimManufactWebApiContext.Manufacturers.Count(e => e.ManufacturerId == id) > 0;
        }
    }
}