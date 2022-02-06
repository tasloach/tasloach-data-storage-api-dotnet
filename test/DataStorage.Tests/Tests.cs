using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using DataStorage.Api;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace DataStorage.Tests
{
    public class Tests : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly WebApplicationFactory<Startup> _factory;

        public Tests(WebApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task PutGet_SameRepoDifferentData()
        {
            HttpClient client = _factory.CreateClient();

            // PUT two different data objects in the same repo.
            ExpectedObject helloData = await PutAsync(client, "data/myRepo", "hello");
            ExpectedObject goodbyeData = await PutAsync(client, "data/myRepo", "goodbye");

            Assert.NotEqual(helloData.oid, goodbyeData.oid);
            Assert.Equal(s_charEncoding.GetByteCount("hello"), helloData.size);
            Assert.Equal(s_charEncoding.GetByteCount("goodbye"), goodbyeData.size);

            // GET the data again by object ID.
            HttpResponseMessage helloResponse = await GetAsync(client, "myRepo", helloData.oid);
            await AssertResponseAsync(helloResponse, expected: "hello", helloData.size);

            HttpResponseMessage goodbyeResponse = await GetAsync(client, "myRepo", goodbyeData.oid);
            await AssertResponseAsync(goodbyeResponse, expected: "goodbye", goodbyeData.size);
        }

        [Fact]
        public async Task Get404()
        {
            HttpClient client = _factory.CreateClient();

            // Return 404 when the data does not exist.
            HttpResponseMessage response = await client.GetAsync("data/randomRepo/randomData");
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task Delete()
        {
            HttpClient client = _factory.CreateClient();

            ExpectedObject helloData = await PutAsync(client, "data/myRepo", "hello again");
            ExpectedObject helloDataYours = await PutAsync(client, "data/yourRepo", "hello again");

            HttpResponseMessage response = await client.DeleteAsync($"data/myRepo/{helloData.oid}");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(string.Empty, await response.Content.ReadAsStringAsync());

            // Confirm that the data is no longer there after being deleted.
            response = await client.GetAsync($"data/myRepo/{helloData.oid}");
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);

            // Make sure data in another repo is not touched.
            response = await client.GetAsync($"data/yourRepo/{helloDataYours.oid}");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            await AssertResponseAsync(response, "hello again", expectedSize: helloDataYours.size);
        }

        [Fact]
        public async Task Delete404()
        {
            HttpClient client = _factory.CreateClient();

            // Return 404 when the data does not exist.
            HttpResponseMessage response = await client.DeleteAsync("data/randomRepo/1234");
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        private static async Task AssertResponseAsync(HttpResponseMessage response, string expected, long expectedSize)
        {
            byte[] bytes = await response.Content.ReadAsByteArrayAsync();
            string decoded = s_charEncoding.GetString(bytes);
            Assert.Equal(expected, decoded);
            Assert.Equal(s_charEncoding.GetByteCount(expected), expectedSize);
        }

        /// <summary>
        /// Put an object and check the response headers.
        /// </summary>
        private static async Task<ExpectedObject> PutAsync(HttpClient client, string url, string content)
        {
            HttpResponseMessage response = await client.PutAsync(url, new ByteArrayContent(s_charEncoding.GetBytes(content)));
            string responseText = await response.Content.ReadAsStringAsync();
            ExpectedObject responseData = JsonSerializer.Deserialize<ExpectedObject>(responseText);

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            Assert.Equal("application/json", response.Content.Headers.ContentType.MediaType);

            return responseData;
        }

        /// <summary>
        /// Get an object that is expected to exist and check the response headers.
        /// </summary>
        private static async Task<HttpResponseMessage> GetAsync(HttpClient client, string repo, string oid)
        {
            HttpResponseMessage response = await client.GetAsync($"data/{repo}/{oid}");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal("application/octet-stream", response.Content.Headers.ContentType.MediaType);
            return response;
        }

        /// <summary>
        /// Non-standard encoding to ensure binary data is handled appropriately
        /// </summary>
        private static readonly Encoding s_charEncoding = Encoding.BigEndianUnicode;

        private class ExpectedObject
        {
            public string oid { get; set; }

            public long size { get; set; }
        }
    }
}