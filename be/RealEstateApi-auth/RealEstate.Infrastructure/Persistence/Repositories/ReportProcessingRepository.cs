using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RealEstate.Domain.Entities;
using RealEstate.Domain.Infrastructure.Persistence.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Infrastructure.Persistence.Repositories
{
    public class ReportProcessingRepository : GenericRepository<ReportProcessing>, IReportProcessingRepository
    {
        public ReportProcessingRepository(RealEstateDbContext context, ILogger logger) : base(context, logger)
        {
        }

        public async Task<ReportProcessing> AddReportProcessing(ReportProcessing reportProcessing)
        {
            var result = await _dbSet.AddAsync(reportProcessing);
            return result.Entity;
        }

        public async Task<IEnumerable<ReportProcessing>> GetReportProcessingDueDate() //lấy ra những xử lý report có ngày đến hạn vượt quá ngày hiện tại
        {
            var result = await _dbSet.Where(x => x.StatusID==2 && x.CreatedDate != null && x.CreatedDate.Value.AddDays(10) <= DateTime.Now).Include(y => y.Post).ToListAsync();
            return result;
        }

        public async Task<ReportProcessing> UpdateStatusReportProcessing(int id, int statusId)
        {
            var result = await _dbSet.Where(x => x.ID == id).FirstOrDefaultAsync();
            if (result != null)
                result.StatusID = statusId;

            return result;
        }

        public async Task<ReportProcessing> GetReportProcessingByPost(int postId)
        {
            var result = await _dbSet.Where(x => x.PostID == postId && (x.StatusID == 1 || x.StatusID == 2)).FirstOrDefaultAsync();

            return result;
        }

        public async Task<IEnumerable<ReportProcessing>> GetReportProcessings(int statusId)
        {
            var result = await _dbSet.Where(x => x.StatusID == statusId).ToListAsync();

            return result;
        }
    }
}
