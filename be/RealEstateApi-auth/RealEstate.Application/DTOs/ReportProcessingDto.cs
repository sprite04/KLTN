using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Application.DTOs
{
    public class ReportProcessingDto
    {
        public int ID { get; set; }
        public int PostID { get; set; }
        public string Title { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? StatusID { get; set; } 
    }
}
