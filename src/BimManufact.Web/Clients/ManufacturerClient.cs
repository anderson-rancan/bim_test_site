﻿using System.Net.Http;
using System.Threading.Tasks;

namespace BimManufact.Web.Clients
{
    public class ManufacturerClient : ClientBase, IManufacturerClient
    {
        private readonly string _getManufacturersAddress = "manufacturers";
        private readonly string _getManufacturerAddress = "manufacturers/{0}";
        private readonly string _getManufacturerLogoAddress = "manufacturers/{0}/logo";

        public async Task<HttpResponseMessage> DeleteManufacturer(int id)
        {
            using (var client = GetWebApiClient())
            {
                return await client.DeleteAsync(string.Format(_getManufacturerAddress, id));
            }
        }

        public async Task<HttpResponseMessage> GetManufacturer(int id)
        {
            using (var client = GetWebApiClient())
            {
                return await client.GetAsync(string.Format(_getManufacturerAddress, id));
            }
        }

        public async Task<HttpResponseMessage> GetManufacturerLogo(int id)
        {
            using (var client = GetWebApiClient())
            {
                return await client.GetAsync(string.Format(_getManufacturerLogoAddress, id));
            }
        }

        public async Task<HttpResponseMessage> GetManufacturers()
        {
            using (var client = GetWebApiClient())
            {
                return await client.GetAsync(_getManufacturersAddress);
            }
        }

        public async Task<HttpResponseMessage> PostManufacturer<T>(T value)
        {
            using (var client = GetWebApiClient())
            {
                return await client.PostAsJsonAsync(_getManufacturersAddress, value);
            }
        }

        public async Task<HttpResponseMessage> PutManufacturer<T>(int id, T value)
        {
            using (var client = GetWebApiClient())
            {
                return await client.PutAsJsonAsync(string.Format(_getManufacturerAddress, id), value);
            }
        }
    }
}