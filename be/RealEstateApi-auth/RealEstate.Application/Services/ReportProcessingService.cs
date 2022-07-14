using AutoMapper;
using PagedList;
using RealEstate.Application.DTOs;
using RealEstate.Application.IServices;
using RealEstate.Domain.Entities;
using RealEstate.Domain.Infrastructure.Persistence.UoW;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Application.Services
{
    public class ReportProcessingService : IReportProcessingService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private int pageSize = 10;

        public ReportProcessingService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ReportProcessingList> GetReportProcessings(int? page, int ? size)
        {
            int pageNumber = (page ?? 1);
            pageSize = (size ?? pageSize);
            ReportProcessingList reportProcessingList = new ReportProcessingList();



            var result = await _unitOfWork.ReportProcessing.GetReportProcessingDueDate();
            if (result != null && result.Count() > 0)
            {
                var reportProcessings = result.Select(report => _mapper.Map<ReportProcessing, ReportProcessingDto>(report)).ToList();
                reportProcessingList.TotalSize = reportProcessings.Count();
                reportProcessingList.ReportProcessings = reportProcessings.ToPagedList(pageNumber, pageSize);
                reportProcessingList.PageNumber = pageNumber;

                return reportProcessingList;
            }
            reportProcessingList.TotalSize = 0;
            reportProcessingList.PageNumber = 1;
            reportProcessingList.ReportProcessings = new List<ReportProcessingDto>();
            return reportProcessingList;

        }

        public async Task<bool> UpdateStatusReportProcessing(int id, int statusId) //1: chua duoc duyet, 2: dang hien thi, 3: tu choi hien thi, 4: khoa bai dang
        {
            var result = await _unitOfWork.ReportProcessing.UpdateStatusReportProcessing(id, statusId);

            if (result == null) return false;

            try
            {
                await _unitOfWork.CompleteAsync();
                if (statusId > 1) //chuyển trạng thái của report thành đã đọc
                {
                    var reportList = await _unitOfWork.Reports.UpdateStatusReportsByPost(result.PostID);
                    await _unitOfWork.CompleteAsync();
                }
                if (statusId == 3) //trạng thái chấp nhận report -> khoá bài đăng
                {
                    var post = await _unitOfWork.Posts.GetPostById(result.PostID);
                    if (post == null) return false;
                    post.StatusID = 4;
                    await _unitOfWork.Posts.UpdatePost(post);
                    await _unitOfWork.CompleteAsync();
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> BlockAllPosts() //1: chua duoc duyet, 2: dang hien thi, 3: tu choi hien thi, 4: khoa bai dang
        {
            var result = await _unitOfWork.ReportProcessing.GetReportProcessingDueDate();

            if (result != null && result.Count() > 0)
            {
                foreach(var rp in result)
                {
                    var reportProcessing = await _unitOfWork.ReportProcessing.UpdateStatusReportProcessing(rp.ID, 3);
                    try
                    {
                        await _unitOfWork.CompleteAsync();
                        var reportList = await _unitOfWork.Reports.UpdateStatusReportsByPost(rp.PostID);
                        await _unitOfWork.CompleteAsync();


                        var post = await _unitOfWork.Posts.GetPostById(rp.PostID);
                        if (post == null) return false;
                        post.StatusID = 4;
                        await _unitOfWork.Posts.UpdatePost(post);
                        await _unitOfWork.CompleteAsync();
                        return true;
                    }
                    catch (Exception ex)
                    {
                        return false;
                    }
                }
            }

            return false;
               
        }


        public async Task<ReportProcessing> GetReportProcessingByPost(int id) //1: chua duoc duyet, 2: dang hien thi, 3: tu choi hien thi, 4: khoa bai dang
        {
            var result = await _unitOfWork.ReportProcessing.GetReportProcessingByPost(id);

            return result;
        }
    }
}
