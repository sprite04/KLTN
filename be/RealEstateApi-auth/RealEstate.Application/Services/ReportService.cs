using AutoMapper;
using PagedList;
using RealEstate.Application.DTOs;
using RealEstate.Application.IServices;
using RealEstate.Application.Models;
using RealEstate.Domain.Entities;
using RealEstate.Domain.Infrastructure.Persistence.UoW;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Application.Services
{
    public class ReportService: IReportService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private int pageSize = 10;

        public ReportService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<bool> AddReport(ReportDto reportDto)
        {
            var result = false;
            Report report = new Report();
            try
            {
                report.Email = reportDto.Email;
                report.Details = reportDto.Details;
                report.PhoneNumber = reportDto.PhoneNumber;
                report.PostID = reportDto.PostID;
                report.UserID = reportDto.UserID;
                report.CreatedDate = DateTime.Now;
                report.Status = false;
                await _unitOfWork.Reports.AddReport(report);
                await _unitOfWork.CompleteAsync();

                var reportProcessing = await _unitOfWork.ReportProcessing.GetReportProcessingByPost(report.PostID);
                if (reportProcessing == null)
                {
                    ReportProcessing value = new ReportProcessing();
                    value.StatusID = 1;
                    value.PostID = report.PostID;
                    value.CreatedDate = DateTime.Now;

                    await _unitOfWork.ReportProcessing.AddReportProcessing(value);
                    await _unitOfWork.CompleteAsync();
                }    
                result = true;

            }
            catch (Exception ex)
            {

            }
            return result;
        }

        public async Task<ReportList> GetReportsByPost(int postId, int ?page, int? size, bool status)
        {
            int pageNumber = (page ?? 1);
            pageSize = (size ?? pageSize);
            ReportList reportList = new ReportList();

            var result = await _unitOfWork.Reports.GetReportsByPost(postId, status);
            if (result != null && result.Count() > 0)
            {
                var reports = result.Select(report => _mapper.Map<Report, ReportResponse>(report)).ToList();
                reportList.TotalSize = reports.Count();
                reportList.Reports = reports.ToPagedList(pageNumber, pageSize);
                reportList.PageNumber = pageNumber;

                return reportList;
            }


            reportList.TotalSize = 0;
            reportList.PageNumber = 1;
            reportList.Reports = new List<ReportResponse>();
            return reportList;
        }

        public async Task<ReportList> GetReports(int ? page, int ? size, bool? status)
        {
            int pageNumber = (page ?? 1);
            pageSize = (size ?? pageSize);
            ReportList reportList = new ReportList();

            var result = await _unitOfWork.Reports.GetReports(status);
            if (result != null && result.Count() > 0)
            {
                var reports = result.Select(report => _mapper.Map<Report, ReportResponse>(report)).ToList();

                reportList.TotalSize = reports.Count();
                reportList.Reports = reports.ToPagedList(pageNumber, pageSize);
                reportList.PageNumber = pageNumber;

                return reportList;
            }


            reportList.TotalSize = 0;
            reportList.PageNumber = 1;
            reportList.Reports = new List<ReportResponse>();
            return reportList;
        }
    }
}
