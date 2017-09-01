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

                if((user.Readable == null || user.Readable.Count == 0) && 
                    (user.MyPosts == null || user.MyPosts.Count == 0))
                {
                    return View();
                }  
  
                var posts = new LinkedList<PostViewModel>();

                foreach(var item in user.MyPosts)
                {
                    var post = GeneratePVM(item);

                    posts.AddLast(post);
                }

                foreach (var item in user.Readable)
                {
                    foreach (var p in item.MyPosts)
                    {
                        var post = GeneratePVM(p);

                        posts.AddLast(post);
                    }
                }
                return View(posts.OrderByDescending(u => u.DatePublish));
            }
        }

        [HttpPost]
        public ActionResult Like(PostViewModel postVM)
        {
            using(ApplicationDbContext context = new ApplicationDbContext())
            {
                var post = context.Posts.First(u => u.Id == postVM.Id);

                string idCrntUser = User.Identity.GetUserId();
                var user = context.Users.FirstOrDefault(u => u.Id == idCrntUser);

                if (!postVM.Like)
                {
                    post.Likes.Add(user);
                }
                else
                {
                    post.Likes.Remove(user);
                }

                postVM = GeneratePVM(post, !postVM.Like);
                
                context.Entry(post).State = EntityState.Modified;
                context.SaveChanges();

                return PartialView(postVM);
            }
        }

        private PostViewModel GeneratePVM(Post p, bool? likePost = null)
        {
            using(ApplicationDbContext context = new ApplicationDbContext())
            {
                string idCrntUser = User.Identity.GetUserId();
                var user = context.Users.FirstOrDefault(u => u.Id == idCrntUser);

                var post = new PostViewModel()
                {
                    Id = p.Id,
                    Creator = p.Creator,
                    DatePublish = p.DatePublish,
                    Image = p.Image,
                    Description = p.Description
                };

                post.CountLike = p.Likes.Count;

                if (likePost != null)
                {
                    post.Like = (bool)likePost;
                }
                else
                {
                    foreach (var like in p.Likes)
                    {
                        if (like.Id == user.Id)
                        {
                            post.Like = true;
                            break;
                        }
                    }
                }

                return post;
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
        public ActionResult Subscribe(Guid id)
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

                return View("UserPage", user_sub.Clone());

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