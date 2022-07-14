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
    public class NewsRepository : GenericRepository<News>, INewsRepository
    {
        public NewsRepository(RealEstateDbContext context, ILogger logger) : base(context, logger)
        {
        }

        public async Task<News> AddNews(News news)
        {

            var result = await _dbSet.AddAsync(news);
            return result.Entity;
        }

        public async Task<bool> DeleteNews(int id)
        {

            var result = await _dbSet.Where(x => x.ID == id).FirstOrDefaultAsync();

            try
            {
                _dbSet.Remove(result);
                return await Task.FromResult(true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Repo} Delete method error", typeof(NewsRepository));
                return await Task.FromResult(false);
            }
        }

        public async Task<News> GetNewById(int id)
        {
            var result = await _dbSet.Where(x=>x.ID == id).Include(y => y.Creator).FirstOrDefaultAsync();
            return result;
        }

        public async Task<IEnumerable<News>> GetNews(bool? display, string? search)
        {
            List<News> result;
            
            if (search == null)
            {
                if (display == null)
                {
                    result = await _dbSet.Include(y => y.Creator).ToListAsync();
                }
                else
                {
                    result = await _dbSet.Where(x => x.Display == display).Include(y => y.Creator).ToListAsync();
                }
            }
            else
            {
                if (display == null)
                {
                    result = await _dbSet.Where(x=>x.Title.Contains(search)).Include(y => y.Creator).ToListAsync();
                }
                else
                {
                    result = await _dbSet.Where(x => x.Title.Contains(search) && x.Display == display).Include(y => y.Creator).ToListAsync();
                }
            }

            

            if (result != null && result.Count() > 0)
                return result;
            return new List<News>();
        }

        public async Task<News> UpdateStatusNews(int id, bool display)
        {
            try
            {
                var exit = await _dbSet.Where(x => x.ID == id).FirstOrDefaultAsync();

                if (exit == null)
                    return null;

                exit.Display = display;


                return exit;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Repo} Update method error", typeof(PostRepository));
                return null;
            }
        }

        public async Task<News> UpdateNews(News news)
        {
            try
            {
                var exit = await _dbSet.Where(x => x.ID == news.ID).FirstOrDefaultAsync();

                if (exit == null)
                    return null;

                exit.Title = news.Title;
                exit.Details = news.Details;
                exit.ImageUrl = news.ImageUrl;
                exit.Display = news.Display;


                return exit;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Repo} Update method error", typeof(PostRepository));
                return null;
            }
        }
    }
}
