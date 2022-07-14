using AutoMapper;
using Azure.Storage.Blobs;
using PagedList;
using RealEstate.Application.DTOs;
using RealEstate.Application.IServices;
using RealEstate.Domain.Entities;
using RealEstate.Domain.Infrastructure.Persistence.UoW;
using RealEstate.Domain.Values;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Application.Services
{
    public class NewsService : INewsService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private int pageSize = 10;

        public NewsService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }


        public async Task<bool> AddNews(NewsDto newsDto)
        {
            var result = false;

            try
            {
                if (newsDto.Image != null)
                {
                    BlobContainerClient blobContainerClient = new BlobContainerClient(AzureInfo.ConnectionString, AzureInfo.ContainerName);
                    string imageName = Guid.NewGuid().ToString();
                    int indexEx = newsDto.Image.FileName.LastIndexOf(".");
                    string extension = newsDto.Image.FileName.Substring(indexEx);
                    string fileName = imageName + extension;

                    BlobClient blobClient = blobContainerClient.GetBlobClient(fileName);
                    var memoryStream = new MemoryStream();
                    await newsDto.Image.CopyToAsync(memoryStream);
                    memoryStream.Position = 0;
                    await blobClient.UploadAsync(memoryStream);

                    string url = blobClient.Uri.AbsoluteUri;
                    if (url != null && url != "")
                    {
                        newsDto.ImageUrl = url;
                    }
                }
                News news = _mapper.Map<News>(newsDto);

                await _unitOfWork.News.AddNews(news);
                await _unitOfWork.CompleteAsync();
                result = true;
            }
            catch (Exception ex)
            {

            }
            return result;
        }

        public async Task<bool> DeleteNews(int id)
        {
            var result = await _unitOfWork.News.DeleteNews(id);
            if (result)
            {
                await _unitOfWork.CompleteAsync();
                return true;
            }
            return false;
        }

        public async Task<NewsDto> GetNewById(int id)
        {
            var result = await _unitOfWork.News.GetNewById(id);
            if (result != null)
            {
                NewsDto newsDto = _mapper.Map<NewsDto>(result);
                return newsDto;
            }
            return null;
        }

        public async Task<bool> UpdateStatusNews(int id, bool display)
        {
            var result = await _unitOfWork.News.UpdateStatusNews(id, display);
            try
            {
                if (result != null)
                {
                    await _unitOfWork.CompleteAsync();
                    return true;
                }    
            }
            catch(Exception ex)
            {

            }

            return false;
        }

        public async Task<NewsList> GetNews(int? page, int? size, bool? display, string? search)
        {
            int pageNumber = (page ?? 1);
            pageSize = (size ?? pageSize);

            NewsList newsList = new NewsList();
            var result = await _unitOfWork.News.GetNews(display, search);
            if (result!=null && result.Count() > 0)
            {
                newsList.TotalSize = result.Count();
                var news = result.Select(n => _mapper.Map<News, NewsDto>(n)).ToList();
                newsList.News = news.ToPagedList(pageNumber, pageSize);
                newsList.PageNumber = pageNumber;
                return newsList;
            }

            newsList.TotalSize = 0;
            newsList.PageNumber = 1;
            newsList.News = new List<NewsDto>();
            return newsList;
        }

        public async Task<bool> UpdateNews(NewsDto newsDto)  
        {
            if (newsDto.Image != null)
            {
                BlobContainerClient blobContainerClient = new BlobContainerClient(AzureInfo.ConnectionString, AzureInfo.ContainerName);
                string imageName = Guid.NewGuid().ToString();
                int indexEx = newsDto.Image.FileName.LastIndexOf(".");
                string extension = newsDto.Image.FileName.Substring(indexEx);
                string fileName = imageName + extension;

                BlobClient blobClient = blobContainerClient.GetBlobClient(fileName);
                var memoryStream = new MemoryStream();
                await newsDto.Image.CopyToAsync(memoryStream);
                memoryStream.Position = 0;
                await blobClient.UploadAsync(memoryStream);

                string url = blobClient.Uri.AbsoluteUri;
                if (url != null && url != "")
                {
                    newsDto.ImageUrl = url;
                }
            }
            News news = _mapper.Map<News>(newsDto);

            var result = await _unitOfWork.News.UpdateNews(news); // ở đây đã có bước check lại có tồn tại new hay không rồi
            if (result != null)
            {
                await _unitOfWork.CompleteAsync();
                return true;
            }
            return false;
        }
    }
}
