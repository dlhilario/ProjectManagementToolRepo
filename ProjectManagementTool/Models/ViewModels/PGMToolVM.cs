using ProjectManagementTool.PMTWebService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjectManagementTool.Models.ViewModels
{
    public class PGMToolVM
    {

        public UserProfile UserProfile { get; set; }
        public UserInfo UserInfo { get; set; }

        public List<Users> users { get; set; }

      
    }
}