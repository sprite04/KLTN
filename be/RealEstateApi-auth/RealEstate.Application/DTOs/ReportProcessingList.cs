using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Application.DTOs
{
    public class ReportProcessingList
    {
        public IEnumerable<ReportProcessingDto> ReportProcessings { get; set; }
        public int TotalSize { get; set; }
        public int PageNumber { get; set; }
    }
}
