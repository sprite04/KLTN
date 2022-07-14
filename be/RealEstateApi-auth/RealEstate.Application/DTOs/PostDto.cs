using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Application.DTOs
{
    public class PostDto
    {
        public int ID { get; set; }

        [Required(ErrorMessage = "Title is required")]
        public string Title { get; set; }
        public List<IFormFile> ImageList { get; set; }
        public List<string> ImageUrls { get; set; }
        public bool Like { get; set; }
        public int ProvinceID { get; set; }
        public int DistrictID { get; set; }
        public int WardID { get; set; }
        public string Address { get; set; }
        public int StatusID { get; set; }
        public float Area { get; set; }
        public ulong? Price { get; set; }
        public int? Bedrooms { get; set; }
        public int? Bathrooms { get; set; }
        public int DirectionID { get; set; }
        public string Details { get; set; }
        public int? PaperID { get; set; }
        public DateTime? CreatedDate { get; set; }
        public bool IsSold { get; set; }
        public string CreatorID { get; set; }
        public string CreatorName { get; set; }
        public string CreatorPhone { get; set; }
        public int PostTypeID { get; set; }
        public string PostTypeName { get; set; }
        public int CategoryID { get; set; }
        public string CategoryName { get; set; }
        
    }
}
