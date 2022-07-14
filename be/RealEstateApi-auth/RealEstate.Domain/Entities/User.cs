using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Domain.Entities
{
    [Table("User")]
    public class User:IdentityUser
    {
        /*[Key]
        public int ID { get; set; }*/

        [StringLength(50)]
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        public bool? Gender { get; set; }
        public DateTime? Birthday { get; set; }
        public string Address { get; set; }
        public DateTime? RegisteredDate { get; set; }
        public virtual ICollection<FavoritePost> FavoritePosts { get; set; }
        public virtual ICollection<Post> Posts { get; set; }
        //public virtual ICollection<Report> Reports { get; set; }
        public virtual ICollection<Follow> FollowUsers { get; set; }
        public virtual ICollection<Follow> FollowedUsers { get; set; }
        public virtual ICollection<News> News { get; set; }
        public virtual ICollection<RefreshToken> RefreshTokens { get; set; }
        public virtual Keyword Keywords { get; set; }
    }
}
