using ProjectManagementTool.PMTWebService;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace ProjectManagementTool.Models
{
    public class OverViewModel
    {
        public static async Task<OverviewDetails> GetOvervieDetails(int CompanyId)
        {
            var identity = (ClaimsPrincipal)Thread.CurrentPrincipal;
            string sid = identity.Claims.SingleOrDefault(x => x.Type == ClaimTypes.PrimarySid).Value;

            OverviewDetails od = new OverviewDetails();
            using (var client = new PGMTWebServiceClient())
            {
                od = await client.GetOverViewAsync(CompanyId, int.Parse(sid));
            }
            return od;
        }

    }
}