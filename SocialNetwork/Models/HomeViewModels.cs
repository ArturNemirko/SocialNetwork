using SocialNetwork.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SocialNetwork.Models
{
   
    public class PostViewModel
    {
        public Guid Id { get; set; }
        public DateTime DatePublish { get; set; }
        public virtual ApplicationUser Creator { get; set; }
        public virtual Image Image { get; set; }
        public virtual Description Description { get; set; }
        public bool Like { get; set; }
        public int CountLike { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
        public int CountComment { get; set; }
    }

    public class SearchUserViewModel
    {
        public virtual ApplicationUser User { get; set; }
        public bool IsYou { get; set; }

    }
}