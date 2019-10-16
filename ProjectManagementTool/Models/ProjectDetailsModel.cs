﻿using ProjectManagementTool.PMTWebService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Web;

namespace ProjectManagementTool.Models
{
    public class ProjectDetailsModel
    {
        private HttpRequestBase Request;
        public ProjectDetailsModel(HttpRequestBase request)
        {
            Request = request;
        }
        public async System.Threading.Tasks.Task<List<Projects>> GetProjectsListAsync()
        {
            var identity = (ClaimsPrincipal)Thread.CurrentPrincipal;
            string sid = identity.Claims.SingleOrDefault(x => x.Type == ClaimTypes.PrimarySid).Value;
            string username = identity.Claims.SingleOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value;
            List<Projects> projectCollection = new List<Projects>();
            using (var client = new PGMTWebServiceClient())
            {
                try
                {

                    int companyId = 0;
                    int userId = 0;
                    int.TryParse(Request["cmp"], out companyId);
                    int.TryParse(sid, out userId);

                    Projects[] projects = await client.GetProjectsAsync(companyId, userId);
                    projectCollection = projects.ToList();
                }
                catch (Exception ex)
                {
                    client.ErrorLogger(new ErrorLog() { ErrorMessage = ex.Message, Method = "ProjectDetailsModel", StackTrace = ex.StackTrace, UserName = username });
                }

            }
            return projectCollection;
        }
    }
}