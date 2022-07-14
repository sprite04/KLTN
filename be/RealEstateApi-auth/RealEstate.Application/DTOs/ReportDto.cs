using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Application.DTOs
{
    public class ReportDto
    {
        public int PostID { get; set; }
        public string UserID { get; set; }
        public string Details { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
    }
}
