using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Domain.Entities
{
    public class Keyword
    {
        [Key]
        public int ID { get; set; }
        public string UserID { get; set; }
        public virtual User User { get; set; }
        public string StrKeywords { get; set; }
    }
}
