using System;
using System.ComponentModel.DataAnnotations;

namespace ProjectManagementTool.Models
{
    public class UserInfo
    {
        [Required(ErrorMessage ="Username required")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "First Name is required")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Last Name is required")]
        public string LastName { get; set; }
        [EmailAddress]
        [Required(ErrorMessage = "Email required")]
        public string emailaddress { get; set; }
        public string Telephone { get; set; }
        public string Status { get; set; }
        public bool LoggedIn { get; set; }
        public Nullable<int> CompanyId { get; set; }
        [Required(ErrorMessage ="User Role Required")]
        public Nullable<int> UserRoleId { get; set; }
        [Required]
        [MinLength(6, ErrorMessage = "Minimun Password Size 6")]
        public string password { get; set; }
        [Required]
        [Compare(nameof(password), ErrorMessage ="Password don't Match")]
        public string ConfirmPassword { get; set; }

    }
}