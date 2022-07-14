using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Domain.Entities
{
    [Table("Report")]
    public class Report
    {
        [Key]
        public int ID { get; set; }
        public int PostID { get; set; }
        public virtual Post Post { get; set; }
        public string UserID { get; set; }
        public virtual User User { get; set; }
        public string Details { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime? CreatedDate { get; set; }
        public bool? Status { get; set; } //true la da duyet qua, false la chua duyet qua
    }
}
