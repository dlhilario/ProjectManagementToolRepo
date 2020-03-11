using ProjectManagementTool.Models;
using ProjectManagementTool.PMTWebService;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Cors;
using System.Web.Mvc;
using System.Web.Routing;

namespace ProjectManagementTool.Controllers
{
    [Authorize, EnableCors(origins: "*", headers: "*", methods: "*")]
    public class PGMToolController : Controller
    {

        private ProjectModel projectModel;
        // GET: PGMTool
        public PGMToolController()
        {

            if (projectModel == null)
                projectModel = new ProjectModel();
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

        public async Task<ActionResult> ProjectDetails(int cmp, int PrjId)
        {
            LoadLeftSideMenu();
            if (PrjId > 0)
            {
                ProjectDetailsModel model = new ProjectDetailsModel(Request);
                projectModel.Project = await model.GetProjectstAsync(cmp, PrjId);

                if (projectModel.Project != null && projectModel.Project.CommentList.Length > 0)
                {
                    StringBuilder builder = new StringBuilder();
                    foreach (var comment in projectModel.Project.CommentList)
                    { //<p>@comment.Time_Stamp (@Model.ProjectModel.GetUserName((int)@comment.UpdatedByUserID)): <blockquote>@comment.Comment</blockquote></p>
                        builder.AppendFormat("<p>{0} ({1}): <blockquote>{2}</blockquote></p>", comment.Time_Stamp, projectModel.GetUserName(comment.UpdatedByUserID), comment.Comment);
                        builder.AppendLine(@"<hr class='fa-flip - horizontal' />");
                    }
                    projectModel.Comments = new Comments();
                    projectModel.Comments.Comment = MvcHtmlString.Create(builder.ToString()).ToString();
                }

            };

            return View(projectModel);
        }
        //Overview
        public async Task<ActionResult> OverView(string partial)
        {
            LoadLeftSideMenu();
            projectModel.OverviewDetails = await OverViewModel.GetOvervieDetails((Request["cmp"] != null) ? int.Parse(Request["cmp"]) : 0);
            return View(projectModel);
        }

        public ActionResult ManageUsers()
        {
            return View(projectModel);
        }

        public ActionResult Details()
        {
            LoadLeftSideMenu();
            ProjectDetailsModel model = new ProjectDetailsModel(Request);
            projectModel.Projects = Task.Run(() => model.GetProjectsListAsync()).Result;
            return View(projectModel);
        }

        public ActionResult CreateNewProject(string cmp)
        {
            LoadLeftSideMenu();
            projectModel.Project = new PMTWebService.Projects
            {
                CompanyID = int.Parse(cmp)
            };

            return View(projectModel);
        }

        public async Task<ActionResult> EditProject(int cmp, int PrjId, bool EditPrj)
        {
            LoadLeftSideMenu();
            if (PrjId > 0)
            {
                ProjectDetailsModel model = new ProjectDetailsModel(Request);
                projectModel.Project = await model.GetProjectstAsync(cmp, PrjId);
                ViewBag.Editproject = true;
            };

            return RedirectToAction("ProjectDetails", new RouteValueDictionary(new { Controller = "PGMTool", action = "ProjectDetails", cmp = cmp, PrjID = PrjId, Editproject = true }));
        }

        public async Task<ActionResult> CancelEditProject(int cmp, int PrjId)
        {
            LoadLeftSideMenu();
            if (PrjId > 0)
            {
                ProjectDetailsModel model = new ProjectDetailsModel(Request);
                projectModel.Project = await model.GetProjectstAsync(cmp, PrjId);
                ViewBag.Editproject = false;
                Session.Abandon();
            };

            return RedirectToAction("ProjectDetails", new RouteValueDictionary(new { Controller = "PGMTool", action = "ProjectDetails", cmp = cmp, PrjID = PrjId }));
        }

        public ActionResult PrintProject(int cmp, int PrjId)
        {
            ProcessStartInfo psi = new ProcessStartInfo
            {
                Verb = "print",
                FileName = Request.RawUrl,
                RedirectStandardOutput = false,
                WindowStyle = ProcessWindowStyle.Hidden,
                UseShellExecute = true
            };
            Process p = new Process
            {
                StartInfo = psi
            };
            p.Start();
            p.WaitForInputIdle();
            Thread.Sleep(3000);
            if (p.CloseMainWindow() == false)
            {
                p.Kill();
            }
            return PartialView("_PrintView", projectModel);
        }

        [HttpPost]
        public ActionResult CreateProject(FormCollection form, ProjectModel projectModel, int cmp)
        {
            LoadLeftSideMenu();
            var identity = (ClaimsPrincipal)Thread.CurrentPrincipal;
            string sid = identity.Claims.SingleOrDefault(x => x.Type == ClaimTypes.PrimarySid).Value;
            int userId = default(int);
            int projectId = default(int);
            try
            {
                if (int.TryParse(sid, out userId))
                {
                    using (var client = new PMTWebService.PGMTWebServiceClient())
                    {
                        Projects project = new Projects();
                        ProjectModel model = new ProjectModel();
                        project.ProjectName = projectModel.Project.ProjectName;
                        #region comments

                        List<Comments> comments = new List<Comments>();

                        comments.Add(new Comments()
                        {
                            Comment = projectModel.Comments.Comment,
                            UpdatedByUserID = int.Parse(sid),
                            Time_Stamp = DateTime.Now
                        });
                        project.CommentList = comments.ToArray();

                        #endregion

                        project.StartDate = projectModel.Project.StartDate;
                        project.EstimatedCompletionDate = projectModel.Project.EstimatedCompletionDate;
                        project.Status = projectModel.Project.Status;
                        project.ProjectScope = projectModel.Project.ProjectScope;
                        project.StreetNumber = projectModel.Project.StreetNumber;
                        project.StreetAddress = projectModel.Project.StreetAddress;
                        project.City = projectModel.Project.City;
                        project.State = projectModel.Project.State;
                        project.ZipCode = projectModel.Project.ZipCode;
                        project.Lot = projectModel.Project.Lot;
                        project.Zone = projectModel.Project.Zone;
                        project.CompanyID = cmp;
                        project.UpdatedByUserID = int.Parse(sid);
                        project.CreatedDate = DateTime.Now;
                        project.IsDeleted = false;


                        List<MaterialList> materialList = new List<MaterialList>();
                        foreach (var material in projectModel.MaterialList)
                        {
                            materialList.Add(material);
                            project.MaterialList = materialList.ToArray();
                        };

                        ProjectResponse response = client.AddProject(int.Parse(sid), project);

                        if (response.Success)
                        {
                            projectId = response.ProjectID;
                            foreach (var attachment in projectModel.Attachments)
                            {
                                attachment.ProjectID = projectId;
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


            return RedirectToAction("ProjectDetails", new RouteValueDictionary(new { Controller = "PGMTool", action = "ProjectDetails", cmp = form["CompanyId"], PrjID = projectId }));
        }

        [HttpPost]
        public async Task<ActionResult> ProjectUpdate(FormCollection form, ProjectModel projectModel, int cmp, int PrjID)
        {

            LoadLeftSideMenu();
            var identity = (ClaimsPrincipal)Thread.CurrentPrincipal;
            string sid = identity.Claims.SingleOrDefault(x => x.Type == ClaimTypes.PrimarySid).Value;
            int userId = default(int);
            int projectId = PrjID;
            try
            {
                if (int.TryParse(sid, out userId))
                {
                    using (var client = new PMTWebService.PGMTWebServiceClient())
                    {
                        Projects project = new Projects
                        {
                            ID = PrjID,
                            CompanyID = cmp,
                            ProjectName = projectModel.Project.ProjectName
                        };
                        #region comments
                        if (projectModel.Comments != null && !string.IsNullOrEmpty(projectModel.Comments.Comment))
                        {
                            List<Comments> comments = new List<Comments>
                        {
                            new Comments()
                            {
                                Comment = projectModel.Comments.Comment,
                                UpdatedByUserID = int.Parse(sid),
                                ProjectID = PrjID,
                                Time_Stamp = DateTime.Now
                            }
                        };
                            project.CommentList = comments.ToArray();
                        }


                        #endregion

                        project.StartDate = projectModel.Project.StartDate;
                        project.EstimatedCompletionDate = projectModel.Project.EstimatedCompletionDate;
                        project.Status = projectModel.Project.Status;
                        project.ProjectScope = projectModel.Project.ProjectScope;
                        project.CostEstimate = projectModel.Project.CostEstimate;
                        project.StreetNumber = projectModel.Project.StreetNumber;
                        project.StreetAddress = projectModel.Project.StreetAddress;
                        project.City = projectModel.Project.City;
                        project.State = projectModel.Project.State;
                        project.ZipCode = projectModel.Project.ZipCode;
                        project.Lot = projectModel.Project.Lot;
                        project.Zone = projectModel.Project.Zone;
                        project.UpdatedByUserID = int.Parse(sid);
                        project.ModifiedDate = DateTime.Now;
                        project.IsDeleted = false;

                        List<MaterialList> materialList = new List<MaterialList>();
                        foreach (var material in projectModel.MaterialList)
                        {
                            material.ProjectID = projectId;
                            material.ModifiedDate = DateTime.Now;
                            material.ModifiedBy = userId;
                            materialList.Add(material);
                            project.MaterialList = materialList.ToArray();
                        };

                        ProjectResponse response = await client.UpdateProjectAsync(userId, project);

                        if (response.Success)
                        {
                            foreach (var attachment in projectModel.Attachments)
                            {
                                attachment.ProjectID = projectId;
                                attachment.ModifiedBy = userId;
                                attachment.ModifiedDate = DateTime.Now;
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
                projectModel.HasError = true;
                projectModel.ErrorMessage = ex.Message;
            }

            return RedirectToAction("ProjectDetails", new { cmp = cmp, prjId = PrjID });
        }

        [HttpPost]
        public ActionResult MaterialList(MaterialList material)
        {
            material.ID = projectModel.MaterialList.Count;
            projectModel.MaterialList.Add(material);

            Session["Material"] = projectModel.MaterialList;
            return PartialView("_MaterialListView", projectModel);
        }

        [HttpPost]
        public ActionResult RemoveMaterial(int id)
        {
            MaterialList material = projectModel.MaterialList.FirstOrDefault(x => x.ID.Equals(id));
            projectModel.MaterialList.Remove(material);
            Session["Material"] = projectModel.MaterialList;
            return PartialView("_MaterialListView", projectModel);
        }

        [HttpPost]
        public ActionResult Attachments(FormCollection form)
        {
            try
            {
                Attachments attachments = new PMTWebService.Attachments
                {
                    ID = projectModel.Attachments.Count,

                    Description = form["Description"],
                    FileName = form["FileName"],
                    FileSize = int.Parse(form["FileSize"]),
                    FileType = form["FileType"],
                    CreatedDate = DateTime.Now
                };

                string base64 = form["data"];

                byte[] byteArray = Convert.FromBase64String(form["Data"]);

                attachments.Document = byteArray;

                projectModel.Attachments.Add(attachments);

                Session["Attachments"] = projectModel.Attachments;

            }
            catch (Exception ex)
            {

                //
            }

            return PartialView("_AttachmentstView", projectModel);
        }

        [HttpPost]
        public ActionResult RemoveFile(int id)
        {
            Attachments attachment = projectModel.Attachments.FirstOrDefault(x => x.ID.Equals(id));
            projectModel.Attachments.Remove(attachment);
            Session["Attachments"] = projectModel.Attachments;
            return PartialView("_AttachmentstView", projectModel);
        }


        private void LoadLeftSideMenu()
        {
            SideMenuBar sideMenuBar = new SideMenuBar();
            projectModel.Companies = Task.Run(() => sideMenuBar.MenuBar()).Result;
        }



    }
}