using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjectManagementTool.Models
{
    public class BaseProject
    {
        private bool HasErrorField;
        private int ProjectIdField;
        private string ErrorMessageField;

        public int ProjectId { get { return ProjectIdField; } set { ProjectIdField = value; } }
        public bool HasError { get { return HasErrorField; } set { HasErrorField = value; } }
        public string ErrorMessage { get { return ErrorMessageField; } set { ErrorMessageField = value; } }

    }
}