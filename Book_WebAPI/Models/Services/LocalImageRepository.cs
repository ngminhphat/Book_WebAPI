using Book_WebAPI.Data;
using Book_WebAPI.Models.Domain;
using Book_WebAPI.Models.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Book_WebAPI.Models.Services
{
    public class LocalImageRepository : IImageRepository
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly AppDbContext _dbContext;

        public LocalImageRepository(
            IWebHostEnvironment webHostEnvironment,
            IHttpContextAccessor httpContextAccessor,
            AppDbContext dbContext)
        {
            _webHostEnvironment = webHostEnvironment;
            _httpContextAccessor = httpContextAccessor;
            _dbContext = dbContext;
        }

        public Image Upload(Image image)
        {
            // Define the local file path to save the image
            var localFilePath = Path.Combine(
                _webHostEnvironment.ContentRootPath,
                "Images",
                $"{image.FileName}{image.FileExtension}"
            );

            // Ensure the directory exists
            var directoryPath = Path.Combine(_webHostEnvironment.ContentRootPath, "Images");
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            // Upload image to local path
            using (var stream = new FileStream(localFilePath, FileMode.Create))
            {
                image.File.CopyTo(stream);
            }

            // Construct the URL to access the uploaded image
            var uriFilePath = $"{_httpContextAccessor.HttpContext.Request.Scheme}://" +
                              $"{_httpContextAccessor.HttpContext.Request.Host}" +
                              $"{_httpContextAccessor.HttpContext.Request.PathBase}/Images/" +
                              $"{image.FileName}{image.FileExtension}";

            // Update the image's FilePath property
            image.FilePath = uriFilePath;

            // Add image to the Images table and save changes
            _dbContext.Images.Add(image);
            _dbContext.SaveChanges();

            return image;
        }
        public (byte[], string, string) DownloadFile(int Id)
        {
            try
            {
                var FileById = _dbContext.Images.Where(x => x.Id == Id).FirstOrDefault();
                var path = Path.Combine(_webHostEnvironment.ContentRootPath, "Images", $"{FileById.FileName}{FileById.FileExtension}");
                var stream = File.ReadAllBytes(path);
                var fileName = FileById.FileName + FileById.FileExtension;
                return (stream, "application/octet-stream", fileName);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<Image> GetAllInfoImages()
        {
            var allImage =_dbContext.Images.ToList();
            return allImage;
        }

    }
}
