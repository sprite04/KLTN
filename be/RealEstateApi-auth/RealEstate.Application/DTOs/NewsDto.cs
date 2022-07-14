using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Application.DTOs
{
    public class NewsDto
    {
        public int? ID { get; set; }
        public string Title { get; set; }
        public string Details { get; set; }
        public DateTime? CreatedDate { get; set; }
        public IFormFile Image { get; set; }
        public string ImageUrl { get; set; }
        public bool Display { get; set; }
        public string CreatorID { get; set; }
        public string CreatorName { get; set; }
    }
}
