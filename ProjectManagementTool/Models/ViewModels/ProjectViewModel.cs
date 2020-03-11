using ProjectManagementTool.Models.ViewModels;
using ProjectManagementTool.PMTWebService;
using System.Collections.Generic;

namespace ProjectManagementTool.Models
{
    public class ProjectViewModel
    {

        private ProjectModel _projectModel;
        
        public ProjectModel ProjectModel
        {
            get
            {
                if (_projectModel == null)
                    _projectModel = new ProjectModel();
                return _projectModel;
            }
        }


    }
}