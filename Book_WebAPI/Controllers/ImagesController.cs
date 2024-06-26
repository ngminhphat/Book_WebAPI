﻿using Book_WebAPI.Models.DTO;
using Book_WebAPI.Models.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Book_WebAPI.Models.Domain;

namespace Book_WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImagesController : ControllerBase
    {
        private readonly IImageRepository _imageRepository;

        public ImagesController(IImageRepository imageRepository)
        {
            _imageRepository = imageRepository;
        }
        [HttpPost]
        [Route("Upload")]
        public ActionResult Upload([FromForm] ImageUploadRequestDTO request)
        {
            ValidateFileUpload(request);

            if (ModelState.IsValid)
            {
                //convert DTO to Domain model
                var imageDomainModel = new Image
                {
                    File = request.File,
                    FileExtension = Path.GetExtension(request.File.FileName),
                    FileSizeInBytes = request.File.Length,
                    FileName = request.FileName,
                    FileDescription = request.FileDescription,
                };

                // User repository to upload image
                _imageRepository.Upload(imageDomainModel);

                return Ok(imageDomainModel);
            }
            return BadRequest(ModelState);
        }
        private void ValidateFileUpload(ImageUploadRequestDTO request)
        {

            var allowExtensions = new string[] { ".jpg", ".jpeg", ".png" };
            if (!allowExtensions.Contains(Path.GetExtension(request.File.FileName)))

            {
                ModelState.AddModelError("file", "Unsupported file extension");

            }

            if (request.File.Length > 1040000)
            {

            }
            ModelState.AddModelError("file", "File size too big, please upload file <10M");

        }
        [HttpGet]
        public IActionResult GetInfoImages()
        {
            var allImages = _imageRepository.GetAllInfoImages();
            return Ok(allImages);
        }
        [HttpGet]
        [Route("Download")]
        public IActionResult DownloadImage (int id)
        {
            var result = _imageRepository.DownloadFile(id);
            return File(result.Item1,result.Item2,result.Item3);
        }



    }

    







}
