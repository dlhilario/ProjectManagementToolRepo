using ProjectManagementTool.Models;
using ProjectManagementTool.PMTWebService;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using System;

namespace ProjectManagementTool.Controllers
{
    public class DataController : Controller
    {
       // public new HttpRequest Request = HttpContext.Current.Request;

        // POST: Data/Create
        public JsonResult Create(FormCollection collection)
        {
            try
            {
                UserProfile profile = new UserProfile();
                if (Request.IsAuthenticated)
                {

                    if (User.IsInRole(UserRoles.ADMIN) || User.IsInRole(UserRoles.MANAGER) || User.IsInRole(UserRoles.IT))
                    {
                        using (var client = new PGMTWebServiceClient())
                        {

                            profile = client.CreateUserProfile(collection["UserInfo." + nameof(UserInfo.UserName)], collection["UserInfo." + nameof(UserInfo.password)], collection["UserInfo." + nameof(UserInfo.emailaddress)], collection["UserInfo." + nameof(UserInfo.FirstName)], collection["UserInfo." + nameof(UserInfo.LastName)]);
                            if (profile != null)
                            {
                                return Json(profile);
                            }
                        }
                    }
                    else
                    {
                        profile.HasMessage = true;
                        profile.Message = "You are not authorize to create User Profile";
                    }

                }
                ///TempData["profile"] = Profile;
                return Json("");
            }
            catch (Exception ex)
            {
                return Json("");
            }
        }

        public async Task<JsonResult> GetUsers(int userId = 0)
        {
            List<Users> userCollection = new List<Users>();
            using (var client = new PGMTWebServiceClient())
            {
                Users[] users = await client.GetUsersAsync();
                userCollection = users.ToList();
                if (userId > 0)
                {
                    return Json(userCollection.SingleOrDefault(x => x.ID.Equals(userId)), JsonRequestBehavior.AllowGet);
                }
            }

            return Json(userCollection, JsonRequestBehavior.AllowGet);
        }

        public JsonResult EditUser(FormCollection collection)
        {
            try
            {
                if (Request.IsAuthenticated)
                {
                    bool result = false;
                    if (User.IsInRole(UserRoles.ADMIN) || User.IsInRole(UserRoles.MANAGER) || User.IsInRole(UserRoles.IT))
                    {
                        using (var client = new PGMTWebServiceClient())
                        {
                            Users usr = new Users()
                            {
                                ID = int.Parse(collection["user_id"]),
                                Active = true,
                                Email = collection[nameof(UserInfo.emailaddress)],
                                FirstName = collection[ nameof(UserInfo.FirstName)],
                                LastName = collection[ nameof(UserInfo.LastName)],
                                //Status = StatusEnum._ACTIVE_,
                                Password = collection[ nameof(UserInfo.password)],
                                //UserProfile = "",
                                //UserProfileID = "",
                                UserName = collection[ nameof(UserInfo.UserName)],
                                //UserRole = null,
                                //UserRoleId = null,

                            };
                            result = client.UpdateUser(usr);
                            if (result)
                            {
                                return Json(result);
                            }
                        }
                    }


                }

            }
            catch(Exception ex)
            {
             //   return Content(HttpStatusCode.BadRequest, ex.Message);
            }
            return Json("");
        }

        //// POST: Data/Edit/5
        //public IHttpActionResult Edit(int id, FormCollection collection)
        //{
        //    return Ok();
        //}


        //// GET: Data/Delete/5
        //public IHttpActionResult Delete(int id)
        //{
        //    return Ok();
        //}

        //// POST: Data/Delete/5

        //public IHttpActionResult Delete(int id, FormCollection collection)
        //{
        //    return Ok();
        //}
    }
}
