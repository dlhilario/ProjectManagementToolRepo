using ProjectManagementTool.PMTWebService;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace ProjectManagementTool.Models
{
    public class SideMenuBar
    {
        public async Task<List<Companies>> MenuBar()
        {
            var identity = (ClaimsPrincipal)Thread.CurrentPrincipal;
            if (identity == null)
            {
                return new List<Companies>();
            }
            string sid = identity.Claims.SingleOrDefault(x => x.Type == ClaimTypes.PrimarySid).Value;
            List<Companies> companies = new List<Companies>();
            try
            {
                using (var client = new PGMTWebServiceClient())
                {
                    var p = client.GetProjects(3, int.Parse(sid));
                    var comp = await client.GetCompaniesAsync(int.Parse(sid));
                    foreach (Companies company in comp)
                    {
                        companies.Add(company);
                    }

                }
            }
            catch (System.Exception ex)
            {
                //
            }

            return companies;
        }

    }
}