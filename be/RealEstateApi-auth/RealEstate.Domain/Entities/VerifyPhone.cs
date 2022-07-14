using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Domain.Entities
{
    public class VerifyPhone
    {
        [Key]
        public string PhoneNumber { get; set; }

        [Required]
        public int Code { get; set; }

        public string ResetToken { get; set; }
        public DateTime CreatedDate{get;set;}
    }
}
