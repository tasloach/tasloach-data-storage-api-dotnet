using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace DataStorage.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DataController : ControllerBase
    {
        public DataController()
        {
            // Consider injecting your abstracted dependencies here
        }

        [HttpPut]
        [Route("{repository}")]
        public async Task<IActionResult> UploadObjectAsync(string repository)
        {
            // Store the data somewhere
            await Task.CompletedTask;
            object result = null;

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
