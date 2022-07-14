using RealEstate.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Application.IServices
{
    public interface IFollowService
    {
        public Task<bool> Follow(string idFollow, string idFollowed);
        public Task<bool> CheckFollow(string idUser1, string idUser2);
        public Task<IEnumerable<UserFollow>> GetFollow(string idFollowed);
        public Task<IEnumerable<UserFollow>> GetFollowed(string idFollow);
    }
}
