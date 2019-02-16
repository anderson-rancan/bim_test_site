using System;
using System.Data.Entity;
using System.Threading.Tasks;
using System.Web.Http.Results;
using BimManufact.WebApi.Controllers;
using BimManufact.WebApi.Models;
using FluentAssertions;
using Moq;
using NUnit.Framework;

namespace BimManufact.WebApi.Unit.Tests.Controllers
{
    [TestFixture]
    public class ManufacturersControllerUnitTest
    {
        private ManufacturersController Controller { get; set; }

        [SetUp]
        public void SetUp()
        {
            DbSet<Manufacturer> dbSet = new TestDbSet<Manufacturer>();

            var bimManufactWebApiContextMock = new Mock<IBimManufactWebApiContext>();
            bimManufactWebApiContextMock
                .SetupGet(_ => _.Manufacturers)
                .Returns(dbSet);

            Controller = new ManufacturersController(bimManufactWebApiContextMock.Object);
        }

        [Test]
        public async Task PostManufacturer_ShouldReturn_SameManufacturer()
        {
            // ARRANGE
            var newManufacturer = GetManufacturerExample();

            // ACT
            var result = await Controller.PostManufacturer(newManufacturer) as CreatedAtRouteNegotiatedContentResult<Manufacturer>;

            // ASSERT
            result.Content.Should().BeEquivalentTo(newManufacturer);
        }

        private Manufacturer GetManufacturerExample()
        {
            return new Manufacturer
            {
                Id = 1,
                Name = "First Manufacturer"
            };
        }
    }
}
