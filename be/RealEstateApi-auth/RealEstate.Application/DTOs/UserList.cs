using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Application.DTOs
{
    public class UserList
    {
        public IEnumerable<UserDto> Users { get; set; }
        public int TotalSize { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }
}
