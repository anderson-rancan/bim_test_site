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
    public class ManufacturerProductsController : ApiControllerBase
    {
        public ManufacturerProductsController(IBimManufactWebApiContext bimManufactWebApiContext, IDummyUserResolver userResolver)
            : base(bimManufactWebApiContext, userResolver)
        {

        }

        [Route("api/manufacturers/{manufacturerId}/products")]
        public IQueryable<ProductResponse> GetProducts(int manufacturerId)
        {
            return WebApiContext.Products
                .Where(_ => _.ManufacturerId == manufacturerId)
                .Select(_ => new ProductResponse
                {
                    ManufacturerId = _.ManufacturerId,
                    Name = _.Name,
                    ProductId = _.ProductId
                });
        }

        [Route("api/manufacturers/{manufacturerId}/products/{productId}", Name = nameof(GetProduct))]
        [ResponseType(typeof(ProductResponse))]
        public async Task<IHttpActionResult> GetProduct(int manufacturerId, int productId)
        {
            var product = await WebApiContext.Products
                .Where(_ => _.ManufacturerId == manufacturerId)
                .Where(_ => _.ProductId == productId)
                .FirstOrDefaultAsync();

            if (product == null)
            {
                return NotFound();
            }

            var response = new ProductResponse
            {
                ManufacturerId = product.ManufacturerId,
                Name = product.Name,
                ProductId = product.ProductId
            };

            return Ok(response);
        }

        [Route("api/manufacturers/{manufacturerId}/products/{productId}/image")]
        [ResponseType(typeof(System.Drawing.Image))]
        public IHttpActionResult GetProductLogo(int manufacturerId, int productId)
        {
            using (var stream = new System.IO.MemoryStream())
            {
                Properties.Resources.no_image_info.Save(stream, System.Drawing.Imaging.ImageFormat.Png);

                var result = new System.Net.Http.HttpResponseMessage(HttpStatusCode.OK);
                result.Content = new System.Net.Http.ByteArrayContent(stream.ToArray());
                result.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("image/png");

                return ResponseMessage(result);
            }
        }

        [Route("api/manufacturers/{manufacturerId}/products/{productId}")]
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutProduct(int manufacturerId, int productId, ProductRequest productRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (manufacturerId != productRequest.ManufacturerId || productId != productRequest.ProductId)
            {
                return BadRequest();
            }

            var product = await WebApiContext.Products.FirstOrDefaultAsync(_ => _.ManufacturerId == manufacturerId && _.ProductId == productId);

            if (product != null)
            {
                product.AuditLastModifiedBy = UserResolver.CurrentUserId;
                product.AuditLastModifiedDate = DateTime.Now;
                product.Name = productRequest.Name;
            }
            else
            {
                return NotFound();
            }

            WebApiContext.Entry(product).State = EntityState.Modified;

            try
            {
                await WebApiContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(manufacturerId, productId))
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

        [Route("api/manufacturers/{manufacturerId}/products")]
        [ResponseType(typeof(ProductResponse))]
        public async Task<IHttpActionResult> PostProduct(int manufacturerId, ProductRequest productRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!await WebApiContext.Manufacturers.AnyAsync(_ => _.ManufacturerId == manufacturerId))
            {
                return BadRequest("Manufacturer does not exist.");
            }

            var now = DateTime.Now;

            var product = new Product
            {
                AuditCreatedBy = UserResolver.CurrentUserId,
                AuditCreatedDate = now,
                AuditLastModifiedBy = UserResolver.CurrentUserId,
                AuditLastModifiedDate = now,
                ManufacturerId = manufacturerId,
                Name = productRequest.Name,
                ProductId = productRequest.ProductId
            };

            WebApiContext.Products.Add(product);
            await WebApiContext.SaveChangesAsync();

            var response = new ProductResponse
            {
                ManufacturerId = product.ManufacturerId,
                Name = product.Name,
                ProductId = product.ProductId
            };

            return CreatedAtRoute(nameof(GetProduct), new { manufacturerId = response.ManufacturerId, productId = response.ProductId }, response);
        }

        [Route("api/manufacturers/{manufacturerId}/products/{productId}/image")]
        public async Task<IHttpActionResult> PostProductImage(int manufacturerId, int productId)
        {
            System.Drawing.Image image = null;
            var imageBytes = Convert.FromBase64String(await Request.Content.ReadAsStringAsync());

            using (var stream = new System.IO.MemoryStream(imageBytes))
            {
                image = System.Drawing.Image.FromStream(stream);
            }

            return Ok();
        }

        [Route("api/manufacturers/{manufacturerId}/products/{productId}")]
        [ResponseType(typeof(ProductResponse))]
        public async Task<IHttpActionResult> DeleteProduct(int manufacturerId, int productId)
        {
            var product = await WebApiContext.Products
                .Where(_ => _.ManufacturerId == manufacturerId)
                .Where(_ => _.ProductId == productId)
                .FirstOrDefaultAsync();

            if (product == null)
            {
                return NotFound();
            }

            WebApiContext.Products.Remove(product);
            await WebApiContext.SaveChangesAsync();

            var response = new ProductResponse
            {
                ManufacturerId = product.ManufacturerId,
                Name = product.Name,
                ProductId = product.ProductId
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

        private bool ProductExists(int manufacturerId, int productId)
        {
            return WebApiContext.Products.Count(_ => _.ManufacturerId == manufacturerId && _.ProductId == productId) > 0;
        }
    }
}