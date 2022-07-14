using RealEstate.Application.DTOs;
using RealEstate.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Application.IServices
{
    public interface INewsService
    {
        public Task<bool> AddNews(NewsDto newsDto);
        public Task<bool> UpdateNews(NewsDto newsDto);
        public Task<bool> DeleteNews(int id);
        public Task<NewsDto> GetNewById(int id);
        public Task<bool> UpdateStatusNews(int id, bool display);
        public Task<NewsList> GetNews(int? page, int? size, bool? display, string? search);
    }
}
