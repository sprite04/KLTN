using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Application.Models
{
    public class Report4
    {
        public List<float> HousePrice { get; set; }
        public List<float> LandPrice { get; set; }
        public List<float> OfficePrice { get; set; }
        public List<float> AppartmentPrice { get; set; }
        public float AvgApartmentPrice { get; set; }
        public float AvgOfficePrice { get; set; }
        public float AvgLandPrice { get; set; }
        public float AvgHousePrice { get; set; }
    }
}
