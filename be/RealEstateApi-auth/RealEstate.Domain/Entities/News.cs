using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Domain.Entities
{
    [Table("News")]
    public class News
    {
        [Key]
        public int ID { get; set; }
        public string Title { get; set; }
        public string Details { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string ImageUrl { get; set; }
        
        public bool Display { get; set; }
        public string CreatorID { get; set; }
        public virtual User Creator { get; set; }
    }
}
