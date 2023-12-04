namespace VehicleTracker.Interfaces
{
    public interface IFileUpload
    {
        public Task<bool> UploadFile(IFormFile file);
        public bool DeleteFile(string image);
    }
}
