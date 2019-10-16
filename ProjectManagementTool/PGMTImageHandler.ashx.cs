using ProjectManagementTool.PMTWebService;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace ProjectManagementTool
{
    /// <summary>
    /// Summary description for PGMTImageHandler
    /// </summary>
    [Authorize]
    public class PGMTImageHandler : IHttpHandler, System.Web.SessionState.IReadOnlySessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            var identity = (ClaimsPrincipal)Thread.CurrentPrincipal;
            string sid = identity.Claims.SingleOrDefault(x => x.Type == ClaimTypes.PrimarySid).Value;

            int imageID = default(int);
            bool thumbNail = false;
            int thumbNailSize = default(int);

            if (int.TryParse(context.Request.QueryString["PictureId"], out imageID))
            {
                List<Attachments> attachments = new List<Attachments>();
             
                if (context.Session["Attachments"] != null)
                    attachments = (List<Attachments>)context.Session["Attachments"];

                Attachments attachment = new Attachments();
                using (var client = new PGMTWebServiceClient())
                {
                    attachment = attachments.SingleOrDefault(x => x.ID == imageID);
                    if (attachment == null)
                        attachment = client.GetAttachmentById(imageID);
                }

                byte[] imageBytes = (attachment != null) ? attachment.Document : null;

                if (imageBytes == null)
                {
                    context.Response.ContentType = "text/plain";
                    context.Response.Write("Missing Image");
                    return;
                }
                context.Response.ContentType = attachment.FileType;

                MemoryStream mem = new MemoryStream(imageBytes);
                Image image = Image.FromStream(mem);

                if (bool.TryParse(context.Request["thumbnail"], out thumbNail))
                {
                    int.TryParse(context.Request["thumbNailSize"], out thumbNailSize);
                    Image thumb = image.GetThumbnailImage(thumbNailSize, thumbNailSize, () => false, IntPtr.Zero);
                    switch (attachment.FileType)
                    {
                        case "image/png":
                            thumb.Save(context.Response.OutputStream, ImageFormat.Png);
                            break;
                        case "image/jpg":
                        case "image/jpeg":
                            thumb.Save(context.Response.OutputStream, ImageFormat.Jpeg);
                            break;
                        case "image/Icon":
                            thumb.Save(context.Response.OutputStream, ImageFormat.Icon);
                            break;
                        case "image/Gif":
                            thumb.Save(context.Response.OutputStream, ImageFormat.Gif);
                            break;
                        default:
                            break;
                    }
                    return;
                }
                switch (attachment.FileType)
                {
                    case "image/png":
                        image.Save(HttpContext.Current.Response.OutputStream, ImageFormat.Png);
                        break;
                    case "image/jpg":
                    case "image/jpeg":
                        image.Save(HttpContext.Current.Response.OutputStream, ImageFormat.Jpeg);
                        break;
                    case "image/Icon":
                        image.Save(HttpContext.Current.Response.OutputStream, ImageFormat.Icon);
                        break;
                    case "image/Gif":
                        image.Save(HttpContext.Current.Response.OutputStream, ImageFormat.Gif);
                        break;
                    default:
                        break;
                }

            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}