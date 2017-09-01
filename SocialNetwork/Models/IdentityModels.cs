﻿using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using SocialNetwork.Models.Entities;
using System.Collections.Generic;
using System;

namespace SocialNetwork.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser, ICloneable
    {
        public string FirstName { get; set; }
        public virtual Image ProfilePicture { get; set; }
        public virtual ICollection<Post> MyPosts { get; set; }
        public virtual ICollection<ApplicationUser> Readable { get; set; }
        public virtual ICollection<ApplicationUser> Readers { get; set; }

        public virtual ICollection<Post> LikesPosts { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }

        public object Clone()
        {
            var posts = new LinkedList<Post>();
            foreach(var item in MyPosts)
            {
                posts.AddLast((Post)item.Clone());
            }
            return new ApplicationUser()
            {
                AccessFailedCount = this.AccessFailedCount,
                Email = this.Email,
                Id = this.Id,
                ProfilePicture = this.ProfilePicture,
                MyPosts = posts
            };
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Description> Descriptions { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<Post> Posts { get; set; }

        public ApplicationDbContext()
            : base("SocialNetworkConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}