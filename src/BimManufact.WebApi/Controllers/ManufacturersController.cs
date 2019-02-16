using System;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using BimManufact.WebApi.Models;
using BimManufact.WebApi.Resolver;

namespace BimManufact.WebApi.Controllers
{
    public class ManufacturersController : ApiControllerBase
    {
        public ManufacturersController(IBimManufactWebApiContext bimManufactWebApiContext, IDummyUserResolver userResolver)
            : base(bimManufactWebApiContext, userResolver)
        {
            
        }

        [Route("api/manufacturers")]
        public IQueryable<ManufacturerResponse> GetManufacturers()
        {
            return WebApiContext.Manufacturers.Select(_ => new ManufacturerResponse
            {
                ManufacturerId = _.ManufacturerId,
                Name = _.Name,
                ProductsCount = WebApiContext.Products.Count(p => p.ManufacturerId == _.ManufacturerId)
            });
        }

        [Route("api/manufacturers/{manufacturerId}")]
        [ResponseType(typeof(ManufacturerResponse))]
        public async Task<IHttpActionResult> GetManufacturer(int manufacturerId)
        {
            var manufacturer = await WebApiContext.Manufacturers.FindAsync(manufacturerId);

            if (manufacturer == null)
            {
                return NotFound();
            }

            var result = new ManufacturerResponse
            {
                ManufacturerId = manufacturerId,
                Name = manufacturer.Name,
                ProductsCount = await WebApiContext.Products.CountAsync(_ => _.ManufacturerId == manufacturerId)
            };

            return Ok(result);
        }

        [Route("api/manufacturers/{manufacturerId}")]
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutManufacturer(int manufacturerId, ManufacturerRequest manufacturerRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (manufacturerId != manufacturerRequest.ManufacturerId)
            {
                return BadRequest();
            }

            var manufacturer = await WebApiContext.Manufacturers.FirstOrDefaultAsync(_ => _.ManufacturerId == manufacturerId);

            if (manufacturer != null)
            {
                manufacturer.AuditLastModifiedBy = UserResolver.CurrentUserId;
                manufacturer.AuditLastModifiedDate = DateTime.Now;
                manufacturer.Name = manufacturerRequest.Name;
            }
            else
            {
                return NotFound();
            }

            WebApiContext.Entry(manufacturer).State = EntityState.Modified;

            try
            {
                await WebApiContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ManufacturerExists(manufacturerId))
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

        [Route("api/manufacturers")]
        [ResponseType(typeof(ManufacturerResponse))]
        public async Task<IHttpActionResult> PostManufacturer(ManufacturerRequest manufacturerRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var now = DateTime.Now;

            var manufacturer = new Manufacturer
            {
                Name = manufacturerRequest.Name,
                AuditCreatedBy = UserResolver.CurrentUserId,
                AuditCreatedDate = now,
                AuditLastModifiedBy = UserResolver.CurrentUserId,
                AuditLastModifiedDate = now
            };

            WebApiContext.Manufacturers.Add(manufacturer);
            await WebApiContext.SaveChangesAsync();

            var response = new ManufacturerResponse
            {
                ManufacturerId = manufacturer.ManufacturerId,
                Name = manufacturer.Name
            };

            return CreatedAtRoute("DefaultApi", new { id = manufacturer.ManufacturerId }, response);
        }

        [Route("api/manufacturers/{manufacturerId}")]
        [ResponseType(typeof(ManufacturerResponse))]
        public async Task<IHttpActionResult> DeleteManufacturer(int manufacturerId)
        {
            var manufacturer = await WebApiContext.Manufacturers.FindAsync(manufacturerId);

            if (manufacturer == null)
            {
                return NotFound();
            }

            WebApiContext.Manufacturers.Remove(manufacturer);
            await WebApiContext.SaveChangesAsync();

            var response = new ManufacturerResponse
            {
                ManufacturerId = manufacturerId,
                Name = manufacturer.Name
            };

            return Ok(response);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                WebApiContext.Dispose();
            }

            base.Dispose(disposing);
        }

        private bool ManufacturerExists(int id)
        {
            return WebApiContext.Manufacturers.Count(e => e.ManufacturerId == id) > 0;
        }
    }
}