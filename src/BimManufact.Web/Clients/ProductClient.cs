using System.Net.Http;
using System.Threading.Tasks;

namespace BimManufact.Web.Clients
{
    public class ProductClient : ClientBase, IProductClient
    {
        private readonly string _getManufacturerProductsAddress = "manufacturers/{0}/products";
        private readonly string _getManufacturerProductAddress = "manufacturers/{0}/products/{1}";

        public async Task<HttpResponseMessage> DeleteManufacturerProduct(int manufacturerId, int productId)
        {
            using (var client = GetWebApiClient())
            {
                return await client.DeleteAsync(string.Format(_getManufacturerProductAddress, manufacturerId, productId));
            }
        }

        public async Task<HttpResponseMessage> GetManufacturerProduct(int manufacturerId, int productId)
        {
            using (var client = GetWebApiClient())
            {
                return await client.GetAsync(string.Format(_getManufacturerProductAddress, manufacturerId, productId));
            }
        }

        public async Task<HttpResponseMessage> GetManufacturerProducts(int manufacturerId)
        {
            using (var client = GetWebApiClient())
            {
                return await client.GetAsync(string.Format(_getManufacturerProductsAddress, manufacturerId));
            }
        }

        public async Task<HttpResponseMessage> PostManufacturerProduct<T>(int manufacturerId, T value)
        {
            using (var client = GetWebApiClient())
            {
                return await client.PostAsJsonAsync(string.Format(_getManufacturerProductsAddress, manufacturerId), value);
            }
        }

        public async Task<HttpResponseMessage> PutManufacturerProduct<T>(int manufacturerId, int productId, T value)
        {
            using (var client = GetWebApiClient())
            {
                return await client.PutAsJsonAsync(string.Format(_getManufacturerProductAddress, manufacturerId, productId), value);
            }
        }
    }
}