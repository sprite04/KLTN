using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Domain.Entities
{
    [Table("Follow")]
    public class Follow
    {
        public string FollowID { get; set; }           //người theo dõi
        public virtual User FollowUser { get; set; }
        public string FollowedID { get; set; }         //người được theo dõi
        public virtual User FollowedUser { get; set; }
    }
}
