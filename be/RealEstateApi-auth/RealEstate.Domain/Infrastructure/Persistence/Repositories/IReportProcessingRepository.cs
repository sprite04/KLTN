using RealEstate.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Domain.Infrastructure.Persistence.Repositories
{
    public interface IReportProcessingRepository
    {
        Task<ReportProcessing> AddReportProcessing(ReportProcessing reportProcessing);
        Task<ReportProcessing> UpdateStatusReportProcessing(int id, int statusId);
        Task<IEnumerable<ReportProcessing>> GetReportProcessingDueDate(); // lấy ra các xử lý đã đến hạn
        Task<ReportProcessing> GetReportProcessingByPost(int postId); //lấy ra reportprocessing đang được xử lý (statusid = 1 or statusid = 2)
        Task<IEnumerable<ReportProcessing>> GetReportProcessings(int statusId);
    }
}
