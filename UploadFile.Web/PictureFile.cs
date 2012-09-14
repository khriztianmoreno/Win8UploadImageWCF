using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace UploadFile.Web
{
    [DataContract]
    public class PictureFile 
    {
        [DataMember]
        public string PictureName { get; set; }

        [DataMember]
        public byte[] PictureStream { get; set; }

    }
}