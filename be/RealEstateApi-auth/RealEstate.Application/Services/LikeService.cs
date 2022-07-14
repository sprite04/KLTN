using AutoMapper;
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
    public class LikeService:ILikeService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public LikeService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<bool> Like(string idUser, int idPost)
        {
            try
            {
                await _unitOfWork.FavoritePosts.Like(idUser, idPost);
                await _unitOfWork.CompleteAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        

        public async Task<IEnumerable<PostDto>> GetLike(string idUser)
        {
            var result = await _unitOfWork.FavoritePosts.GetLike(idUser);
            var posts = result.Select(post => _mapper.Map<Post, PostDto>(post)).ToList();
            foreach (var post in posts)
                post.Like = true;
           
            return posts;
        }

        
    }
}
