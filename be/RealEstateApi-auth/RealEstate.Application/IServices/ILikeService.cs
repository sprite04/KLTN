using RealEstate.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Application.IServices
{
    public interface ILikeService
    {
        Task<bool> Like(string idUser, int idPost);
        Task<IEnumerable<PostDto>> GetLike(string idUser);
    }
}
