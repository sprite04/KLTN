using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Application.DTOs
{
    public class PostsUserCurrent
    {
        public string Name { get; set; }
        public string ID { get; set; }
        public int FollowCount { get; set; } // đang theo dõi
        public int FollowedCount { get; set; }//được theo dõi bởi
        public IEnumerable<PostDto> PostPending { get; set; }
        public IEnumerable<PostDto> PostShowing { get; set; }
        public IEnumerable<PostDto> PostDenied { get; set; }
    }
}
