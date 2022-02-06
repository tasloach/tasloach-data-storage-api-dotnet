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
            var id = Guid.NewGuid();

            var body = HttpContext.Request.Body;
            byte[] contents;

            await using (var memoryStream = new MemoryStream())
            {
                await body.CopyToAsync(memoryStream);
                contents = memoryStream.ToArray();
            }

            _storageService.Put(repository, id, contents);

            var result = new { oid = id, size = contents.Length };

            return CreatedAtAction(
                "DownloadObject", // Works with or without Async suffix on DownloadObject method
                routeValues: new { repository, objectID = id },
                value: result);
        }

        [HttpGet]
        [Route("{repository}/{objectId}")]
        public IActionResult DownloadObject(string repository, string objectId)
        {
            byte[] value = null;
            var foundData = Guid.TryParse(objectId, out var id) && _storageService.TryGetValue(repository, id, out value);

            // Get the data from somewhere

            if (foundData)
                return new FileStreamResult(fileStream: new MemoryStream(value), "application/octet-stream");

            return NotFound();
        }

        [HttpDelete]
        [Route("{repository}/{objectId}")]
        public IActionResult DeleteObject(string repository, string objectId)
        {
            var couldDeleteObject = Guid.TryParse(objectId, out var id) && _storageService.Delete(repository, id);

            if (couldDeleteObject)
                return Ok();

            return NotFound();
        }
    }
}