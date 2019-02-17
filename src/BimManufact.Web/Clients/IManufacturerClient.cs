﻿using System.Net.Http;
using System.Threading.Tasks;

namespace BimManufact.Web.Clients
{
    public interface IManufacturerClient
    {
        Task<HttpResponseMessage> DeleteManufacturer(int id);
        Task<HttpResponseMessage> GetManufacturer(int id);
        Task<HttpResponseMessage> GetManufacturers();
        Task<HttpResponseMessage> PostManufacturer<T>(T value);
        Task<HttpResponseMessage> PutManufacturer<T>(int id, T value);
    }
}