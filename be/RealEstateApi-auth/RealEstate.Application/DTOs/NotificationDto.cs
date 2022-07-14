using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Application.DTOs
{
    public class NotificationDto
    {
        public int PostID { get; set; }
        public string UserID { get; set; }
        public string Content { get; set; }
    }
}
