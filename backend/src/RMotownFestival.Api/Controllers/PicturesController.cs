using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RMotownFestival.Api.Common;
using System;
using System.Linq;

namespace RMotownFestival.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PicturesController : ControllerBase
    {
        private BlobUtility BlobUtility { get; }

        public PicturesController(BlobUtility blobUtility)
        {
            BlobUtility = blobUtility;
        }

        [HttpGet]
        public string[] GetAllPictureUrls()
        {
            BlobContainerClient container = BlobUtility.GetThumbsContainer();

            return container.GetBlobs()
                .Select(blob => BlobUtility.GetSasUri(container, blob.Name))
                .ToArray();
        }

        [HttpPost]
        public ActionResult PostPicture(IFormFile file)
        {
            BlobContainerClient container = BlobUtility.GetPicturesContainer();
            Response<BlobContentInfo> response = container.UploadBlob(file.FileName, file.OpenReadStream());
            return Ok(response.GetRawResponse());
        }
    }
}
