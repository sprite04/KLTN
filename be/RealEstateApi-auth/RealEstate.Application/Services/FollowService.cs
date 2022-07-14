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
    public class FollowService: IFollowService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public FollowService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<bool> CheckFollow(string idUser1, string idUser2)
        {
            var result = await _unitOfWork.Follows.CheckExist(idUser1, idUser2);
            if (idUser1.Equals(idUser2))
                return false;
            if (result != null)
                return true;
            return false;
        }

        public async Task<bool> Follow(string idFollow, string idFollowed)
        {
            if (idFollow.Equals(idFollowed))
                return false;
            try
            {
                await _unitOfWork.Follows.Follow(idFollow, idFollowed);
                await _unitOfWork.CompleteAsync();
                return true;
            }
            catch(Exception ex)
            {
                return false;
            }
        }

        public async Task<IEnumerable<UserFollow>> GetFollow(string idFollowed)
        {
            var result= await _unitOfWork.Follows.GetFollow(idFollowed);
            var users = result.Select(user => _mapper.Map<User, UserFollow>(user));
            return users;
        }

        public async Task<IEnumerable<UserFollow>> GetFollowed(string idFollow)
        {
            var result = await _unitOfWork.Follows.GetFollowed(idFollow); 
            var users = result.Select(user => _mapper.Map<User, UserFollow>(user));
            return users;
        }
    }
}
