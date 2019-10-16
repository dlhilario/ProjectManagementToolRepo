using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjectManagementTool.Models.ViewModels
{
    public class ProjectAttachementModel : BaseProject
    {
        public List<Attachment> Attachments { get; set; }
    }
    public class Attachment 
    {
        public string FileName { get; set; }
        public string Extention { get; set; }
        public string FileType { get; set; }
        public string CreatedDate { get; set; }
        public string Projects_ID { get; set; }
        public string Document { get; set; }
        public string ProjectID { get; set; }
    }
}