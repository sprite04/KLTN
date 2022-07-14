using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Application.Models
{
    public class ReportResponse
    {
        public int ID { get; set; }
        public int PostID { get; set; }
        public string UserID { get; set; }
        public string Name { get; set; }
        public string UserName { get; set; }
        public string Details { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime? CreatedDate { get; set; }
        public bool? Status { get; set; } //true la da duyet qua, false la chua duyet qua
    }
}
