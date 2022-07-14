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
    public class ReportRepository : GenericRepository<Report>, IReportRepository
    {
        public ReportRepository(RealEstateDbContext context, ILogger logger) : base(context, logger)
        {
        }

        public async Task<Report> AddReport(Report report)
        {
            var result = await _dbSet.AddAsync(report);
            return result.Entity;
        }

        public async Task<IEnumerable<Report>> GetReportsByPost(int postId, bool status)
        {
            var result = await _dbSet.Where(x => x.PostID == postId && x.Status == status).Include(y => y.User).ToListAsync();
            return result;
        }

        public async Task<IEnumerable<Report>> GetReports(bool ? status)
        {
            if (status == null)
            {
                var result = await _dbSet.Include(y => y.User).ToListAsync();
                return result;
            }
            else
            {
                var result = await _dbSet.Where(x => x.Status == status).Include(y => y.User).ToListAsync();
                return result;
            }
        }

        public async Task<IEnumerable<Report>> UpdateStatusReportsByPost(int postId) //nếu false thì là chưa đọc còn true thì là đã đọc
        {
            var result = await _dbSet.Where(x => x.PostID == postId && x.Status != true).ToListAsync();

            foreach (var value in result)
            {
                value.Status = true;
            }

            return result;
        }
    }
}
