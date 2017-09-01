using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SocialNetwork.Models.Entities
{
    public class Post : ICloneable
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public DateTime DatePublish { get; set; }
        public virtual ApplicationUser Creator { get; set; }
        public virtual Image Image { get; set; }
        public virtual Description Description { get; set; }
        public virtual ICollection<ApplicationUser> Likes { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }

        public object Clone()
        {
            return new Post()
            {
                Id = this.Id,
                DatePublish = this.DatePublish,
                Creator = this.Creator,
                Image = this.Image,
                Description = this.Description,
                Likes = this.Likes,
                Comments = this.Comments
            };
        }
    }
}