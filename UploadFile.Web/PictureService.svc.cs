using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.Text;
using System.Web;

namespace UploadFile.Web
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "PictureService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select PictureService.svc or PictureService.svc.cs at the Solution Explorer and start debugging.
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Required)]
    public class PictureService : IPictureService
    {
        public bool Upload(PictureFile picture)
        {
            FileStream fileStream = null;
            BinaryWriter writer = null;
            string filePath;

            try
            {
                filePath = HttpContext.Current.Server.MapPath(".") + ConfigurationManager.AppSettings["PictureUploadDirectory"] + picture.PictureName;

                if (picture.PictureName != string.Empty)
                {
                    fileStream = File.Open(filePath, FileMode.Create);
                    writer = new BinaryWriter(fileStream);
                    writer.Write(picture.PictureStream);
                }

                return true;
            }
            catch (Exception)
            {
                return false;
            }
            finally
            {
                if (fileStream != null)
                    fileStream.Close();
                if (writer != null)
                    writer.Close();
            }
        }


        public PictureFile Download(string pictureName)
        {
            FileStream fileStream = null;
            BinaryReader reader = null;
            string imagePath;
            byte[] imageBytes;

            try
            {
                imagePath = HttpContext.Current.Server.MapPath(".") + ConfigurationManager.AppSettings["PictureUploadDirectory"] + pictureName + ".jpg";
                if (File.Exists(imagePath))
                {
                    fileStream = new FileStream(imagePath, FileMode.Open, FileAccess.Read);
                    reader = new BinaryReader(fileStream);

                    imageBytes = reader.ReadBytes((int)fileStream.Length);

                    return new PictureFile() { PictureName = pictureName, PictureStream = imageBytes };
                }
                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }
    
    }
}
