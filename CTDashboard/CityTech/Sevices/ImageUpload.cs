
namespace CityTech.Sevices
{
    public class ImageUpload
    {

        public void FileUpload(IFormFile file,string companyName, string folderName, string Image, IWebHostEnvironment hostingEnvironment)
        {

            try
            {
				string webRootPath = hostingEnvironment.WebRootPath;

				var upload = Path.Combine(webRootPath, "Companies/" + companyName + "/" + folderName);

				if (!Directory.Exists(upload))
				{
					Directory.CreateDirectory(upload);

					var parentPath = folderName.Split("/")[0];

					if (folderName.Split("/")[0] != parentPath)
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
						var FileName = Image + extension;

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
