
namespace SoftaxeERP_API.Services
{
    public interface IFileUpload
    {
        // file, comapnyName, FolderName, fileName
        void fileUpload(IFormFile file, string companyName, string folderName, string fileName, IWebHostEnvironment hostingEnvironment);
    }

    public class FileUpload : IFileUpload
    {
        public void fileUpload(IFormFile file, string companyName, string folderName, string fileName, IWebHostEnvironment hostingEnvironment)
        {
            try
            {
                string webRootPath = hostingEnvironment.WebRootPath;

                var upload = Path.Combine(webRootPath, "Companies/" + companyName + "/" + folderName);

                if (!Directory.Exists(upload))
                {
                    Directory.CreateDirectory(upload);

                    if (!folderName.Contains("VchType"))
                    {
                        string sourcePath = "";
                        string noImg = fileName;

                        if(folderName == "CompanyLogo")
                        {
                            sourcePath = Path.Combine(webRootPath, "Companies/Logo");
                        }
                        else
                        {
                            sourcePath = Path.Combine(webRootPath, "Companies/NoImage");
                        }
                        string targetPath = upload;

                        string sourceFile = System.IO.Path.Combine(sourcePath, noImg);
                        string destFile = System.IO.Path.Combine(targetPath, noImg);

                        if (System.IO.Directory.Exists(sourcePath))
                        {
                            string[] files = System.IO.Directory.GetFiles(sourcePath);

                            foreach (string s in files)
                            {
                                noImg = System.IO.Path.GetFileName(s);
                                destFile = System.IO.Path.Combine(targetPath, noImg);
                                System.IO.File.Copy(s, destFile, true);
                            }
                        }
                    }
                }

                if (file != null)
                {
                    if (file.Length > 0)
                    {
                        var extension = Path.GetExtension(file.FileName);

                        var FileName = fileName + extension;

                        using (var filesStream = new FileStream(Path.Combine(upload, FileName), FileMode.Create))
                        {
                            file.CopyTo(filesStream);
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
