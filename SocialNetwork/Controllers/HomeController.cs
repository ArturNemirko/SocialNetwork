using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using SocialNetwork.Models;
using SocialNetwork.Models.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SocialNetwork.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        public HomeController() { }

        public HomeController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
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

        public ActionResult Index()
        {
            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                string idCrntUser = User.Identity.GetUserId();
                var user = context.Users.FirstOrDefault(u => u.Id == idCrntUser);

                if(user.Readable == null)
                {
                    return View();
                }  

                var posts = new LinkedList<Post>();

                foreach (var item in user.Readable)
                {
                    foreach (var p in item.MyPosts)
                    {
                        posts.AddLast((Post)p.Clone());
                    }
                }
                //var posts = context.Posts.Select(u => u).ToList();
                //var show_posts = new List<Post>();
                //foreach (var item in posts)
                //{
                //    show_posts.Add((Post)item.Clone());
                //}
                return View(posts.OrderByDescending(u => u.DatePublish));
            }
        }

        public ActionResult CreatePost()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreatePost(Post post)
        {
            ApplicationUser user = UserManager.FindById(User.Identity.GetUserId());
            if (user != null)
            {
                if (user.MyPosts == null)
                {
                    user.MyPosts = new List<Post>();
                }
                post.DatePublish = DateTime.Now;
                post.Creator = user;
                user.MyPosts.Add(post);
                UserManager.Update(user);
                return View("Index");
            }
            return View();
        }

        [HttpPost]
        public void Subscribe(Guid id)
        {
            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                string idCrntUser = User.Identity.GetUserId();
                var user = context.Users.FirstOrDefault(u => u.Id == idCrntUser);
                var user_sub = context.Users.FirstOrDefault(u => u.Id == id.ToString());

                //ApplicationUser user = UserManager.FindById(User.Identity.GetUserId());
                if (user.Readable == null)
                {
                    user.Readable = new List<ApplicationUser>();
                }
                user.Readable.Add(user_sub);

                context.Entry(user).State = EntityState.Modified;
                context.SaveChanges();
            }

        }

        public ActionResult SearchUser()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SearchUser(string name)
        {
            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                var models = context.Users.Where(a => a.Email.Contains(name))
                             .Select(a => a)
                             .Distinct();
                return View(models.ToList());
            }
        }


        public ActionResult AutocompleteSearch(string term)
        {
            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                var models = context.Users.Where(a => a.Email.Contains(term))
                             .Select(a => new { value = a.Email, label = a.Email })
                             .Distinct();
                return Json(models.ToList(), JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult UserPage(Guid id)
        {
            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                var user = context.Users.FirstOrDefault(u => u.Id == id.ToString());
                return View(user.Clone());
            }
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        private T CloneObject<T>(T obj)
        {
            T new_obj = obj;
            return new_obj;
        }

    }
}