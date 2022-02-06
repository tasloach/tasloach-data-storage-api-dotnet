using DataStorage.Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace DataStorage.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DataController : ControllerBase
    {
        private readonly IStorageService _storageService;

        public DataController(IStorageService storageService)
        {
            // Consider injecting your abstracted dependencies here
            _storageService = storageService;
        }

        [HttpPut]
        [Route("{repository}")]
        public async Task<IActionResult> UploadObjectAsync(string repository)
        {
            // Store the data somewhere
            await Task.CompletedTask;
            object result = null;

            var req = HttpContext.Request;
            var bodyStr = "";

            using (var reader = new StreamReader(req.Body, Encoding.UTF8))
            {
                bodyStr = await reader.ReadToEndAsync();
            }

            result = new { oid = "test", size = 15151 };

            return CreatedAtAction(
                "DownloadObject", // Works with or without Async suffix on DownloadObject method
                routeValues: new { repository, objectID = "some object id" },
                value: result);
        }

        [HttpGet]
        [Route("{repository}/{objectID}")]
        public IActionResult DownloadObject(string repository, string objectID)
        {
            bool foundData = false;

            // Get the data from somewhere

            if (foundData)
            {
                return new FileStreamResult(fileStream: new MemoryStream(), "application/octet-stream");
            }
            else
            {
                return NotFound();
            }
        }

        [HttpDelete]
        [Route("{repository}/{objectID}")]
        public IActionResult DeleteObject(string repository, string objectID)
        {
            bool couldDeleteObject = false;

            if (couldDeleteObject)
            {
                return Ok();
            }
            else
            {
                return NotFound();
            }
        }
    }
}
