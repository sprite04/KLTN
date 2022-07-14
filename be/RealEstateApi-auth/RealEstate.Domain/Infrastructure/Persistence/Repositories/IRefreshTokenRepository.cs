using RealEstate.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Domain.Infrastructure.Persistence.Repositories
{
    public interface IRefreshTokenRepository: IGenericRepository<RefreshToken>
    {
        Task<RefreshToken> GetByToken(string token);
        Task Create(RefreshToken refreshToken);
        Task<bool> DeleteById(int id);
        Task<bool> DeleteByIdUser(string idUser);
    }
}
