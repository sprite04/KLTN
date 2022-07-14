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
    public class RefreshTokenRepository: GenericRepository<RefreshToken>, IRefreshTokenRepository
    {
        public RefreshTokenRepository(RealEstateDbContext context, ILogger logger) : base(context, logger)
        {
        }

        public async Task Create (RefreshToken refreshToken)
        {
            await _dbSet.AddAsync(refreshToken);
        }

        public Task<RefreshToken> GetByToken(string token)
        {
            var result = _dbSet.FirstOrDefault(r => r.Token.Equals(token));
            return Task.FromResult(result);
        }

        public Task<bool> DeleteById(int id)
        {
            var result = _dbSet.FirstOrDefault(x => x.ID == id);
            if (result == null)
                return Task.FromResult(false);

            _dbSet.Remove(result);
            return Task.FromResult(true);

        }

        public Task<bool> DeleteByIdUser(string idUser)
        {
            var result = _dbSet.Where(x => x.UserID.Equals(idUser));
            if (result == null)
                return Task.FromResult(false);

            _dbSet.RemoveRange(result);
            return Task.FromResult(true);
        }

    }
}
