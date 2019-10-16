using ProjectManagementTool.Models.ViewModels;
using ProjectManagementTool.PMTWebService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.SessionState;

namespace ProjectManagementTool.Models
{
    [Serializable]
    public class ProjectModel : BaseProject
    {
        private List<MaterialList> _MaterialList;
        private List<Attachments> _Attachment;

        public Projects Project { get; set; }

        public MaterialList Material { get; set; }

        private HttpSessionState Session => HttpContext.Current.Session;

        public List<MaterialList> MaterialList
        {
            get
            {
                if (Session["Material"] != null)
                    _MaterialList = (List<MaterialList>)Session["Material"];
                if (_MaterialList == null)
                    _MaterialList = new List<PMTWebService.MaterialList>();
                return _MaterialList;
            }
            set
            {
                if (_MaterialList == null)
                    _MaterialList = new List<PMTWebService.MaterialList>();

                _MaterialList = value;
            }
        }

        public Attachments Attachment { get; set; }

        public List<Attachments> Attachments
        {
            get
            {
                if (Session["Attachments"] != null)
                    _Attachment = (List<Attachments>)Session["Attachments"];
                if (_Attachment == null)
                    _Attachment = new List<Attachments>();
                return _Attachment;
            }
            set
            {
                if (_Attachment == null)
                    _Attachment = new List<Attachments>();

                _Attachment = value;
            }
        }

        public Comments Comments { get; set; }
               
        public List<Projects> Projects { get; set; }


        public async Task<List<ProjectStatus>> ProjectStatusAsync()
        {
            List<ProjectStatus> ps = new List<PMTWebService.ProjectStatus>();
            using (var entity = new PMTWebService.PGMTWebServiceClient())
            {
                var status = await entity.GetProjectStatusesAsync();
                ps = status.ToList();
            }
            return ps;
        }

        public IEnumerable<SelectListItem> SelectProjectStatusListItem()
        {
            List<SelectListItem> listItem = new List<SelectListItem>();
            var statuses = Task.Run(() => ProjectStatusAsync()).Result;
            foreach (var item in statuses)
            {
                SelectListItem select = new SelectListItem();
                select.Value = item.Value;
                select.Disabled = item.Active;
                select.Text = item.Text;
                select.Selected = item.Selected;
                listItem.Add(select);
            }
            return listItem;
        }

        public SelectList SelectListForProjectStatus()
        {
            SelectList selectList = new SelectList(SelectProjectStatusListItem(), "Value", "Text");
            return selectList;
        }

    }



}