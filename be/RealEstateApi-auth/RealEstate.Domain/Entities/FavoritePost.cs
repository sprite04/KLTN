using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Domain.Entities
{
    [Table("FavoritePost")]
    public class FavoritePost
    {
        public string UserID { get; set; }
        public virtual User User { get; set; }
        public int PostID { get; set; }
        public virtual Post Post { get; set; }
    }
}
