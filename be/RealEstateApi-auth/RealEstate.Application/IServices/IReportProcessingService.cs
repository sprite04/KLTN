using RealEstate.Application.DTOs;
using RealEstate.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Application.IServices
{
    public interface IReportProcessingService
    {
        public Task<bool> UpdateStatusReportProcessing(int id, int statusId); //khi reportprocessing đã được xử lý thì chuyển tất cả các report của nó ở thời điểm đó thành đã đọc (true) khi trạng thái của processing chuyển thành chờ phàn hồi, đòng ý báo cáo, từ chối báo cáo: đã xong
        public Task<ReportProcessingList> GetReportProcessings(int? page, int? size);
        public Task<bool> BlockAllPosts();
        public Task<ReportProcessing> GetReportProcessingByPost(int id);

    }
}
