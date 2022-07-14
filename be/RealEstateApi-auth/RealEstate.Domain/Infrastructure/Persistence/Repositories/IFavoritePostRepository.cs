using RealEstate.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Domain.Infrastructure.Persistence.Repositories
{
    public interface IFavoritePostRepository
    {
        Task<FavoritePost> CheckExist(string idUser, int idPost);
        Task Like(string idUser, int idPost);
        Task<IEnumerable<Post>> GetLike(string idUser);
    }
}
