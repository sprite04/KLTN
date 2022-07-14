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
    public class FavoritePostRepository: GenericRepository<FavoritePost>, IFavoritePostRepository
    {
        public FavoritePostRepository(RealEstateDbContext context, ILogger logger) : base(context, logger) { }

        public async Task<FavoritePost> CheckExist(string idUser, int idPost)
        {
            var result = await _dbSet.Where(x =>x.UserID.Equals(idUser) && x.PostID==idPost).FirstOrDefaultAsync();
            return result;
        }

        public async Task Like(string idUser, int idPost)
        {
            var result = await CheckExist(idUser, idPost);
            if (result == null)
            {
                FavoritePost favoritePost = new FavoritePost();
                favoritePost.UserID = idUser;
                favoritePost.PostID = idPost;
                
                _dbSet.Add(favoritePost);
            }
            else
            {
                _dbSet.Remove(result);
            }
        }

        

        public async Task<IEnumerable<Post>> GetLike(string idUser)
        {
            var result= await (from p in _dbSet.Include(x => x.Post).ThenInclude(x => x.Images)
                         where p.UserID.Equals(idUser) && p.Post.IsDeleted == false
                         select p.Post).ToListAsync();
            
            return result;
        }

       
    }
}
