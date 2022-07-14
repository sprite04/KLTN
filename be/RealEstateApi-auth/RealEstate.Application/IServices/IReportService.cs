using RealEstate.Application.DTOs;
using RealEstate.Application.Models;
using RealEstate.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Application.IServices
{
    public interface IReportService
    {
        public Task<ReportList> GetReportsByPost(int postId, int? page, int? size, bool status);
        public Task<bool> AddReport(ReportDto report);
        public Task<ReportList> GetReports(int? page, int? size, bool? status);
    }
}
