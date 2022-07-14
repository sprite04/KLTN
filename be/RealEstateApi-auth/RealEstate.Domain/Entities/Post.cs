using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Domain.Entities
{
    [Table("Post")]
    public class Post
    {
        [Key]
        public int ID { get; set; }

        [Required(ErrorMessage = "Title is required")]
        public string Title { get; set; }
        public int? ProvinceID { get; set; }
        public int? DistrictID { get; set; }
        public int? WardID { get; set; }
        public string Address { get; set; }
        public int? StatusID { get; set; }
        public float? Area { get; set; }
        public ulong? Price { get; set; }
        public int? Bedrooms { get; set; }
        public int? Bathrooms { get; set; }
        public int? DirectionID { get; set; }
        public string Details { get; set; }
        public int? PaperID { get; set; }
        public DateTime? CreatedDate { get; set; }
        public bool? IsSold { get; set; } //de trong dto de qua fe xu ly hien thi da ban
        public bool? IsDeleted { get; set; } //khong co trong dto
        public string CreatorID { get; set; }
        public int PostTypeID { get; set; }
        public int CategoryID { get; set; }
        public virtual User Creator { get; set; }
        public virtual PostType PostType { get; set; }
        public virtual Category Category { get; set; }
        public virtual ICollection<Image> Images { get; set; }
        public virtual ICollection<FavoritePost> FavoritePosts { get; set; }
        public virtual ICollection<Report> Reports { get; set; }
        public virtual ICollection<ReportProcessing> ReportProcessings { get; set; }
    }
}
