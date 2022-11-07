namespace FiorelloTaskFronToBack.Helpers
{
    public interface IFileService
    {
        Task<string> UploadAsync(IFormFile file, string webrootPath);
        void Delete(string webrootPath, string filename);
        bool CheckPhoto(IFormFile file);
        bool MaxSize(IFormFile file, int maxSize);
    }
}
