using ProjectManagementTool.Models.ViewModels;
using ProjectManagementTool.PMTWebService;
using System.Collections.Generic;

namespace ProjectManagementTool.Models
{
    public class ProjectViewModel
    {

        private PGMToolVM _PGMToolVM;
        private ProjectModel _projectModel;

        public PGMToolVM PGMToolVM
        {
            get
            {
                if (_PGMToolVM == null)
                    _PGMToolVM = new PGMToolVM();

                return _PGMToolVM;
            }
            set
            {
                _PGMToolVM = value;
            }
        }

        public List<Companies> Companies
        {
            get; set;
        }

        public ProjectModel ProjectModel
        {
            get
            {
                if (_projectModel == null)
                    _projectModel = new ProjectModel();
                return _projectModel;
            }
        }

        public OverviewDetails OverviewDetails { get; set; }
    }
}