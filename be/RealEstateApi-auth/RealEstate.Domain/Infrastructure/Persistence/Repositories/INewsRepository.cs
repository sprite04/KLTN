using RealEstate.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Domain.Infrastructure.Persistence.Repositories
{
    public interface INewsRepository
    {
        Task<News> AddNews(News news);
        Task<News> UpdateNews(News news);
        Task<News> GetNewById(int id);
        Task<bool> DeleteNews(int id);
        Task<IEnumerable<News>> GetNews(bool? display, string? search);
        Task<News> UpdateStatusNews(int id, bool display);
    }
}
