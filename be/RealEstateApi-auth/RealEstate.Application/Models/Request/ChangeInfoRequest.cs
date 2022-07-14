using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Application.Models.Request
{
    public class ChangeInfoRequest
    {
        [StringLength(50)]
        public string Name { get; set; }
        public IFormFile Image { get; set; }
        public string ImageUrl { get; set; }
        public bool? Gender { get; set; }
        public DateTime? Birthday { get; set; }
        public string Address { get; set; }
    }
}
