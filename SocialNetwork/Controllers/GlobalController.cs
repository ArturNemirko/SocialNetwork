using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using SocialNetwork.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SocialNetwork.Controllers
{
    public class GlobalController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;


        public GlobalController() { }

        public GlobalController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }
        public FileContentResult GetUserImage(Guid id)
        {
            try
            {
                ApplicationUser user = UserManager.FindById(id.ToString());
                var s = user.ProfilePicture;
                if (user.ProfilePicture == null)
                {
                    Image image = Image.FromFile(@"E:\SocialNetworkGit\SocialNetwork\Content\image\default.png");//("../Content/image/default.jpg");
                    System.IO.MemoryStream memoryStream = new System.IO.MemoryStream();
                    image.Save(memoryStream, System.Drawing.Imaging.ImageFormat.Png);
                    byte[] b = memoryStream.ToArray();

                    user.ProfilePicture = new SocialNetwork.Models.Entities.Image() { ImageBytes = b };
                    UserManager.Update(user);
                }
                return File(user.ProfilePicture.ImageBytes, ".png");
            }
            catch (NullReferenceException ex)
            {

            }
            return null;
        }
    }
}