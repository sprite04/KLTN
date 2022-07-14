using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Application.DTOs
{
    public class UserFilterParams
    {
        public int? Page { get; set; }
        public int? Size { get; set; }
        public string Search { get; set; }
        public string RoleID { get; set; }
    }
}
