using VehicleTracker.Interfaces;

namespace VehicleTracker.Services
{
    public class FileUploadService : IFileUpload
    {
        private IWebHostEnvironment env;
        public FileUploadService(IWebHostEnvironment env)
        {
            this.env = env;
        }


        public async Task<bool> UploadFile(IFormFile file)
        {
            try
            {
                var contentPath = this.env.ContentRootPath;
                var path = Path.Combine(contentPath, "wwwroot/Images");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                var ext = Path.GetExtension(file.FileName);
                var allowedExtensions = new string[] { ".jpg", ".png", ".jpeg" };
                if (allowedExtensions.Contains(ext))
                {
                    var fileWithPath = Path.Combine(path, file.FileName);
                    var stream = new FileStream(fileWithPath, FileMode.Create);
                    await file.CopyToAsync(stream);
                    stream.Close();
                    return true;
                }
                return false;

            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool DeleteFile(string imageFileName)
        {
            try
            {
                var wwwPath = this.env.WebRootPath;
                var path = Path.Combine(wwwPath, "wwwroot\\Images\\", imageFileName);
                if (System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
