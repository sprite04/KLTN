using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Application.DTOs
{
    public class Info
    {
        public string ID { get; set; }

        [StringLength(50)]
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        public bool? Gender { get; set; }
        public DateTime? Birthday { get; set; }
        public string Address { get; set; }
    }
}
