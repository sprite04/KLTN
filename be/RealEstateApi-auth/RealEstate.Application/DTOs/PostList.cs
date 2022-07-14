using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Application.DTOs
{
    public class PostList
    {
        public IEnumerable<PostDto> Posts { get; set; }
        public int TotalSize { get; set; }
        public int PageNumber { get; set; }
    }
}
