using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Application.DTOs
{
    public class FilterParams
    {
        public string Search { get; set; }
        public string Sort { get; set; }
        public int? Page { get; set; }
        public int? Size { get; set; }
        public ulong? MinPrice { get; set; }
        public ulong? MaxPrice { get; set; }
        public int? StatusID { get; set; }
        public int CategoryID { get; set; }
        public int ProvinceID { get; set; }
        public int DistrictID { get; set; }
        public int PostTypeID { get; set; }
        public string UserID { get; set; }
    }
}