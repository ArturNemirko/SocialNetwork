using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SocialNetwork.Models.Entities
{
    public class Comment : ICloneable
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public virtual ApplicationUser Creator { get; set; }
        public virtual Description Description { get; set; }

        public object Clone()
        {
            return new Comment()
            {
                Id = this.Id,
                Creator = this.Creator,
                Description = this.Description
            };
        }
    }
}