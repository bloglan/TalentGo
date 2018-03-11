using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TalentGoWebApp.Models
{
    public class FileUploadResultModel
    {
        public int Result { get; set; }

        public string Message { get; set; }

        public string FileId { get; set; }
    }
}