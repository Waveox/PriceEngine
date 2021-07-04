using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Newtonsoft.Json;
using PriceEngine;
using PriceEngine.Models;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace PriceEngineTesting
{
    public class ClientTests
    {
        private readonly TestServer _server;
        private readonly HttpClient _client;

        public ClientTests()
        {
            _server = new TestServer(new WebHostBuilder()
               .UseStartup<Startup>());
            _client = _server.CreateClient();
        }

        [Fact]
        public async Task ProductsTest()
        {
            var response = await _client.GetAsync("/v1/products");
            response.EnsureSuccessStatusCode();

            var responseString = JsonConvert.DeserializeObject<Product[]>(await response.Content.ReadAsStringAsync());
            var product = responseString[0];

            Assert.Equal(2, responseString.Length);

            Assert.Equal("Product1", product.ProductName);
            Assert.Equal(175, product.BoxFullCost);
            Assert.Equal("1", product.ProductNumber);
            Assert.Equal(30, product.SingleUnitMarkupPercentage);
            Assert.Equal(20, product.CartonSize);

            product = responseString[1];
            Assert.Equal("Product2", product.ProductName);
            Assert.Equal(825, product.BoxFullCost);
            Assert.Equal("2", product.ProductNumber);
            Assert.Equal(30, product.SingleUnitMarkupPercentage);
            Assert.Equal(5, product.CartonSize);
        }


        [Fact]
        public async Task ProductsListPricesTest()
        {
            var response = await _client.GetAsync("/v1/Products/ListPrices/1");
            response.EnsureSuccessStatusCode();

            var responseString = JsonConvert.DeserializeObject<FullPrice[]>(await response.Content.ReadAsStringAsync());

            Assert.Equal(50, responseString.Length);

            var product = responseString[0];

            Assert.Equal("Product1", product.ProductName);
            Assert.Equal("1", product.ProductNumber);
            Assert.Equal(1, product.RequestedUnits);
            Assert.Equal(11.375m, product.TotalCost);

            product = responseString[49];

            Assert.Equal("Product1", product.ProductName);
            Assert.Equal("1", product.ProductNumber);
            Assert.Equal(50, product.RequestedUnits);
            Assert.Equal(428.750m, product.TotalCost);
        }

        [Fact]
        public async Task ProductsListPricesWrongProductTest()
        {
            var response = await _client.GetAsync("/v1/Products/ListPrices/5");

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task BrokenEndpointTest()
        {
            var response = await _client.GetAsync("/v1/Products/ListPricesz");

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task ProductsGetPriceTest()
        {
            var response = await _client.GetAsync("/v1/Products/GetPrice?productNumber=1&units=200");
            response.EnsureSuccessStatusCode();

            var product = JsonConvert.DeserializeObject<FullPrice>(await response.Content.ReadAsStringAsync());

            Assert.Equal("Product1", product.ProductName);
            Assert.Equal("1", product.ProductNumber);
            Assert.Equal(200, product.RequestedUnits);
            Assert.Equal(1575.0m, product.TotalCost);
        }

        [Fact]
        public async Task ProductsGetPriceZeroUnitsTest()
        {
            var response = await _client.GetAsync("/v1/Products/GetPrice?productNumber=1&units=0");
            response.EnsureSuccessStatusCode();

            var product = JsonConvert.DeserializeObject<FullPrice>(await response.Content.ReadAsStringAsync());

            Assert.Equal("Product1", product.ProductName);
            Assert.Equal("1", product.ProductNumber);
            Assert.Equal(0, product.RequestedUnits);
            Assert.Equal(0m, product.TotalCost);
        }

        [Fact]
        public async Task ProductsGetPriceNegativeUnitsTest()
        {
            var response = await _client.GetAsync("/v1/Products/GetPrice?productNumber=2&units=-100");
            response.EnsureSuccessStatusCode();

            var product = JsonConvert.DeserializeObject<FullPrice>(await response.Content.ReadAsStringAsync());

            Assert.Equal("Product2", product.ProductName);
            Assert.Equal("2", product.ProductNumber);
            Assert.Equal(-100, product.RequestedUnits);
            Assert.Equal(0m, product.TotalCost);
        }

        [Fact]
        public async Task ProductsGetPriceWrongProductTest()
        {
            var response = await _client.GetAsync("/v1/Products/GetPrice?productNumber=5&units=14");

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task ProductsGetPriceOverflowUnitsTest()
        {
            var response = await _client.GetAsync("/v1/Products/GetPrice?productNumber=2&units=99999999999999999999");

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }
    }
}