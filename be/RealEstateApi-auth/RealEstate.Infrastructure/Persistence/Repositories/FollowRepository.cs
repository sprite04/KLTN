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
    public class FollowRepository: GenericRepository<Follow>, IFollowRepository
    {
        public FollowRepository(RealEstateDbContext context, ILogger logger) : base(context, logger){}

        public async Task<Follow> CheckExist(string idFollow, string idFollowed)
        {
            var result = await _dbSet.Where(x => x.FollowID.Equals(idFollow) && x.FollowedID.Equals(idFollowed)).FirstOrDefaultAsync();
            return result;
        }

        public async Task Follow(string idFollow, string idFollowed)
        {
            var result = await CheckExist(idFollow, idFollowed);
            if (result == null)
            {
                Follow follow = new Follow();
                follow.FollowedID = idFollowed;
                follow.FollowID = idFollow;
                _dbSet.Add(follow);
            }
            else
            {
                _dbSet.Remove(result);
            }
        }

       

        public async Task<IEnumerable<User>> GetFollow(string idFollowed)
        {
            var result = await _dbSet.Where(x => x.FollowedID.Equals(idFollowed)).Include(y => y.FollowUser).Select(z=>z.FollowUser).ToListAsync();
            return result;
        }

        public async Task<IEnumerable<User>> GetFollowed(string idFollow)
        {
            var result = await _dbSet.Where(x => x.FollowID.Equals(idFollow)).Include(y => y.FollowedUser).Select(z => z.FollowedUser).ToListAsync(); 
            return result;
        }
    }
}
