using Microsoft.AspNetCore.Mvc;

namespace FiorelloTaskFronToBack.Helpers
{
    public class FileService:IFileService
    {
        public async Task<string> UploadAsync(IFormFile file,string webrootPath)
        {
            var filename= $"{Guid.NewGuid()}_{file.FileName}";
            string path=Path.Combine(webrootPath,"assets/images",filename);
            using(FileStream fileStream=new(path, FileMode.Create, FileAccess.ReadWrite))
            {
                await file.CopyToAsync(fileStream);
            }
            return filename;
        }

        public void Delete(string webrootPath,string filename)
        {
            string path = Path.Combine(webrootPath, "assets/images", filename);
            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }

        public bool CheckPhoto(IFormFile file)
        {
            if (file.ContentType.Contains("image/"))
            {
                return true;
            }
            return false;
        }

        public bool MaxSize(IFormFile file,int maxSize)
        {
            if(file.Length/1024> maxSize)
            {
                return false;
            }
            return true;
        }
    }
}

