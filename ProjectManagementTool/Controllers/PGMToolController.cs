using ProjectManagementTool.Models;
using ProjectManagementTool.PMTWebService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Cors;
using System.Web.Mvc;

namespace ProjectManagementTool.Controllers
{
    [Authorize, EnableCors(origins: "*", headers: "*", methods: "*")]
    public class PGMToolController : Controller
    {

        private ProjectViewModel projectvm;
        // GET: PGMTool
        public PGMToolController()
        {
            if (projectvm == null)
                projectvm = new ProjectViewModel();
        }
        public ActionResult Manage()
        {

            return View();
        }

        //Project Views
        public ActionResult Projects(string p = "0")
        {
            if (p.Equals("0"))
            {
                return RedirectToAction("OverView");
            }
            return PartialView("_PGMProjectList");
        }

        //Overview
        public async Task<ActionResult> OverView(string partial)
        {
            LoadLeftSideMenu();
            projectvm.OverviewDetails = await OverViewModel.GetOvervieDetails((Request["cmp"] != null) ? int.Parse(Request["cmp"]) : 0);
            return View(projectvm);
        }

        public ActionResult ManageUsers()
        {
            return View(projectvm);
        }

        public ActionResult Details()
        {
            LoadLeftSideMenu();
            ProjectDetailsModel model = new ProjectDetailsModel(Request);
            projectvm.ProjectModel.Projects = Task.Run(() => model.GetProjectsListAsync()).Result;
            return View(projectvm);
        }

        public ActionResult CreateNewProject(string cmp)
        {
            Task.Run(() => LoadLeftSideMenu());
            projectvm.ProjectModel.Project = new PMTWebService.Projects();
            projectvm.ProjectModel.Project.CompanyID = int.Parse(cmp);

            return View(projectvm);
        }

        [HttpPost]
        public ActionResult CreateProject(FormCollection form)
        {
            var identity = (ClaimsPrincipal)Thread.CurrentPrincipal;
            string sid = identity.Claims.SingleOrDefault(x => x.Type == ClaimTypes.PrimarySid).Value;
            int userId = default(int);

            try
            {
                if (int.TryParse(sid, out userId))
                {
                    using (var client = new PMTWebService.PGMTWebServiceClient())
                    {
                        Projects project = new Projects();
                        ProjectModel model = new ProjectModel();
                        project.ProjectName = form["ProjectModel.Project.ProjectName"];
                        #region comments

                        List<Comments> comments = new List<Comments>();

                        comments.Add(new Comments()
                        {
                            Comment = form["ProjectModel.Comments"],
                            UpdatedByUserID = int.Parse(sid),
                            Time_Stamp = DateTime.Now
                        });
                        project.CommentList = comments.ToArray();

                        #endregion

                        project.StartDate = DateTime.Parse(form["ProjectModel.Project.StartDate"]);
                        project.EstimatedCompletionDate = DateTime.Parse(form["ProjectModel.Project.EstimatedCompletionDate"]);
                        project.Status = form["ProjectModel.Project.Status"];
                        project.ProjectScope = form["ProjectModel.Project.ProjectScope"];
                        project.StreetNumber = int.Parse(form["ProjectModel.Project.StreetNumber"]);
                        project.StreetAddress = form["ProjectModel.Project.StreetAddress"];
                        project.City = form["ProjectModel.Project.City"];
                        project.State = form["ProjectModel.Project.State"];
                        project.ZipCode = int.Parse(form["ProjectModel.Project.ZipCode"]);
                        project.Lot = form["ProjectModel.Project.Lot"];
                        project.Zone = form["ProjectModel.Project.Zone"];
                        project.CompanyID = int.Parse(form["CompanyId"]);
                        project.UpdatedByUserID = int.Parse(sid);
                        project.CreatedDate = DateTime.Now;
                        project.IsDeleted = false;


                        List<MaterialList> materialList = new List<MaterialList>();
                        foreach (var material in projectvm.ProjectModel.MaterialList)
                        {
                            materialList.Add(material);
                            project.MaterialList = materialList.ToArray();
                        };

                        ProjectResponse response = client.AddProject(int.Parse(sid), project);

                        if (response.Success)
                        {
                            foreach (var attachment in projectvm.ProjectModel.Attachments)
                            {
                                attachment.ProjectID = response.ProjectID;
                                client.AddAttachments(userId, attachment);
                            };
                        }

                        Session["MaterialList"] = null;
                        Session["Attachments"] = null;
                    }
                }

            }
            catch (Exception ex)
            {
                //throw;
            }


            return View(projectvm);
        }

        [HttpPost]
        public ActionResult MaterialList(MaterialList material)
        {
            material.ID = projectvm.ProjectModel.MaterialList.Count;
            projectvm.ProjectModel.MaterialList.Add(material);

            Session["Material"] = projectvm.ProjectModel.MaterialList;
            return PartialView("_MaterialListView", projectvm);
        }

        [HttpPost]
        public ActionResult RemoveMaterial(int id)
        {
            MaterialList material = projectvm.ProjectModel.MaterialList.FirstOrDefault(x => x.ID.Equals(id));
            projectvm.ProjectModel.MaterialList.Remove(material);
            Session["Material"] = projectvm.ProjectModel.MaterialList;
            return PartialView("_MaterialListView", projectvm);
        }

        [HttpPost]
        public ActionResult Attachments(FormCollection form)
        {
            try
            {
                Attachments attachments = new PMTWebService.Attachments();

                attachments.ID = projectvm.ProjectModel.Attachments.Count;

                attachments.Description = form["Description"];
                attachments.FileName = form["FileName"];
                attachments.FileSize = int.Parse(form["FileSize"]);
                attachments.FileType = form["FileType"];
                attachments.CreatedDate = DateTime.Now;

                string base64 = form["data"];

                byte[] byteArray = Convert.FromBase64String(form["Data"]);

                attachments.Document = byteArray;

                projectvm.ProjectModel.Attachments.Add(attachments);

                Session["Attachments"] = projectvm.ProjectModel.Attachments;

            }
            catch (Exception ex)
            {

                //
            }

            return PartialView("_AttachmentstView", projectvm);
        }

        [HttpPost]
        public ActionResult RemoveAttachment(int id)
        {
            Attachments attachment = projectvm.ProjectModel.Attachments.FirstOrDefault(x => x.ID.Equals(id));
            projectvm.ProjectModel.Attachments.Remove(attachment);
            Session["Attachments"] = projectvm.ProjectModel.MaterialList;
            return PartialView("_AttachmentstView", projectvm);
        }


        private void LoadLeftSideMenu()
        {
            SideMenuBar sideMenuBar = new SideMenuBar();
            projectvm.Companies = Task.Run(() => sideMenuBar.MenuBar()).Result;
        }



    }
}