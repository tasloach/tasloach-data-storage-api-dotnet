using DataStorage.Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
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
            Guid id = Guid.NewGuid();
            object result = null;

            var body = HttpContext.Request.Body;
            byte[] contents;

            await using (var memoryStream = new MemoryStream())
            {
                await body.CopyToAsync(memoryStream);
                contents = memoryStream.ToArray();
            }

            _storageService.Put(repository, id, contents);
            
            result = new { oid = id, size = contents.Length };

            return CreatedAtAction(
                "DownloadObject", // Works with or without Async suffix on DownloadObject method
                routeValues: new { repository, objectID = "some object id" },
                value: result);
        }

        [HttpGet]
        [Route("{repository}/{objectID}")]
        public IActionResult DownloadObject(string repository, string objectID)
        {
            byte[] value = null;
            var foundData = Guid.TryParse(objectID, out var id) &&_storageService.TryGetValue(repository, id, out value);

            // Get the data from somewhere

            if (foundData)
            {
                return new FileStreamResult(fileStream: new MemoryStream(value), "application/octet-stream");
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
            bool couldDeleteObject = _storageService.Delete(repository, Guid.Parse(objectID));

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
