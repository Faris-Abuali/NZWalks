using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;
using NZWalks.API.Utilities;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public class ImagesController : ControllerBase
    {
        private readonly IImageRepository imageRepository;

        public ImagesController(IImageRepository imageRepository)
        {
            this.imageRepository = imageRepository;
        }

        // POST: /api/Images/Upload
        [HttpPost]
        [Route("Upload")]
        public async Task<IActionResult> Upload([FromForm] ImageUploadRequestDto request)
        {
            ValidateFileUpload(request);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Convert the DTO to Domain Model
            var imageDomainModel = new Image
            {
                File = request.File,
                Extension = Path.GetExtension(request.File.FileName),
                SizeInBytes = request.File.Length,
                Name = request.FileName,
                Description = request.FileDescription,
            };
            // Image path will be determined later by the repository


            // Use repository to upload image
            await imageRepository.Upload(imageDomainModel);
            // No need to say: var imageDomainModel = await imageRepository.Upload(imageDomainModel);

            return Ok(imageDomainModel);
        }

        private void ValidateFileUpload(ImageUploadRequestDto request)
        {
            // 1. Check File Extension
            var allowedExtensions = new string[] { ".jpg", ".jpeg", ".png" };

            var extension = Path.GetExtension(request.File.FileName);
            if (!allowedExtensions.Contains(extension))
            {
                ModelState.AddModelError("file", "Unsupported file extension");
            }

            // 2. Check File Size
            if (request.File.Length > Constants.MaxImageUploadSize)
            {
                ModelState.AddModelError("file", "File size exceeds 10 MB");
            }
        }
    }
}
