using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Domain.Entities
{
    [Table("Notification")]
    public class Notification
    {
        [Key]
        public int ID { get; set; }
        public int PostID { get; set; }
        public virtual Post Post { get; set; } //check lại xem những field này có bị kéo lên hay không, nếu có thì làm ảnh hưởng tốc độ
        public string UserID { get; set; }
        public virtual User User { get; set; } //check lại 
        public string Content { get; set; }
        public DateTime? CreatedDate { get; set; }
        public bool? Status { get; set; } //trạng thái đã được đọc hay chưa
    }
}
