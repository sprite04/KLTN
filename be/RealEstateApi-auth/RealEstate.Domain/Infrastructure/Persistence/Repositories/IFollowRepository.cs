using RealEstate.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Domain.Infrastructure.Persistence.Repositories
{
    public interface IFollowRepository
    {
        Task<Follow> CheckExist(string idFollow, string idFollowed);
        Task Follow(string idFollow, string idFollowed);
        
        Task<IEnumerable<User>> GetFollow(string idFollowed);
        Task<IEnumerable<User>> GetFollowed(string idFollow);
    }
}
