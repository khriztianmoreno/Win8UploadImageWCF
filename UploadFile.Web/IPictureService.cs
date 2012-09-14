using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace UploadFile.Web
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IPictureService" in both code and config file together.
    [ServiceContract]
    public interface IPictureService
    {
        [OperationContract]
        bool Upload(PictureFile picture);

        [OperationContract]
        PictureFile Download(string pictureName);
    }
}
