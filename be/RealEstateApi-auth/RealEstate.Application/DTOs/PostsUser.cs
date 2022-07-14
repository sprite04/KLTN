using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Application.DTOs
{
    public class PostsUser
    {
        public string Name { get; set; }
        public string Avatar { get; set; }
        public string ID { get; set; }
        public int FollowCount { get; set; } // đang theo dõi
        public int FollowedCount { get; set; }//được theo dõi bởi
        public IEnumerable<PostDto> Posts { get; set; }
    }
}
