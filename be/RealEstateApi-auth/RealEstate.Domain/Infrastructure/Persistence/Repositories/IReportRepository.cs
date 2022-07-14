using RealEstate.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Domain.Infrastructure.Persistence.Repositories
{
    public interface IReportRepository
    {
        Task<Report> AddReport(Report report);
        Task<IEnumerable<Report>> GetReports(bool? status);
        Task<IEnumerable<Report>> GetReportsByPost(int postId, bool status);
        Task<IEnumerable<Report>> UpdateStatusReportsByPost(int postId); //chuyển trạng thái của report thành đã đọc (true)
    }
}
