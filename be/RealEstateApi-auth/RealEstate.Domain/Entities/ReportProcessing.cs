using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Domain.Entities
{
    [Table("ReportProcessing")]
    public class ReportProcessing
    {
        [Key]
        public int ID { get; set; }
        public int PostID { get; set; }
        public virtual Post Post { get; set; }
        //public string UserID { get; set; } ở đây có cần user hay không
        //public virtual User User { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? StatusID { get; set; } //có 4 trạng thái: chưa xử lý, chờ phản hồi, đồng ý báo cáo, từ chối báo cáo
    }
}
