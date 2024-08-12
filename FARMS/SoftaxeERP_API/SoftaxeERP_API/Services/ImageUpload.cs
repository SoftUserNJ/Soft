using ImageMagick;

namespace SoftaxeERP_API.Services
{
    public class ImageUpload
    {
        public void FileUpload(IFormFile file, string companyName, string folderName, string Image, IWebHostEnvironment hostingEnvironment)
        {

            try
            {
                string webRootPath = hostingEnvironment.WebRootPath;

                var upload = Path.Combine(webRootPath, "Companies/" + companyName + "/" + folderName);

                if (!Directory.Exists(upload))
                {
                    Directory.CreateDirectory(upload);

                    if (folderName.Split("/")[0] != "VchType")
                    {
                        string fileName = Image;
                        string sourcePath = Path.Combine(webRootPath, "Companies/NoImage");
                        string targetPath = upload;

                        string sourceFile = System.IO.Path.Combine(sourcePath, fileName);
                        string destFile = System.IO.Path.Combine(targetPath, fileName);

                        if (System.IO.Directory.Exists(sourcePath))
                        {
                            string[] files = System.IO.Directory.GetFiles(sourcePath);

                            foreach (string s in files)
                            {
                                fileName = System.IO.Path.GetFileName(s);
                                destFile = System.IO.Path.Combine(targetPath, fileName);
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
                        var FileName = "";

                        if (folderName == "CompanyLogo")
                        {
                            FileName = Image + extension;

                            using (var filesStream = new FileStream(Path.Combine(upload, FileName), FileMode.Create))
                            {
                                file.CopyTo(filesStream);
                            }
                        }
                        else
                        {
                            if (extension.ToLower() != ".pdf")
                            {
                                FileName = Image + ".webp";

                                using (var image = new MagickImage(file.OpenReadStream()))
                                {
                                    // Compress the image (adjust the compression level as needed)
                                    image.Quality = (int)(image.Quality * 0.5); // 50% compression

                                    // Convert to WebP format
                                    image.Format = MagickFormat.WebP;

                                    using (var filesStream = new FileStream(Path.Combine(upload, FileName), FileMode.Create))
                                    {
                                        image.Write(filesStream);
                                    }
                                }

                            }
                            else
                            {
                                FileName = Image + extension;

                                using (var filesStream = new FileStream(Path.Combine(upload, FileName), FileMode.Create))
                                {
                                    file.CopyTo(filesStream);
                                }
                            }
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