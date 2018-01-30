using Microsoft.AspNet.Identity.EntityFramework;
using ProjectManagementTool.PMTWebService;

namespace ProjectManagementTool.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        
        public override string Email { get; set; }
        public bool Active { get; set; }
        public bool HasMessage { get; set; }
        public bool LoggedIn { get; set; }

        public string Message { get; set; }
        public string status { get; set; }
        public string UserId { get; set; }
        public string User_Name { get; set; }
        public string UserRole { get; set; }
    }

}