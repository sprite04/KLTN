using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RealEstate.Domain.Infrastructure.Persistence.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Infrastructure.Persistence.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected RealEstateDbContext _context;
        protected DbSet<T> _dbSet;
        protected readonly ILogger _logger;
        public GenericRepository(RealEstateDbContext context,ILogger logger )
        {
            _context = context;
            _logger = logger;
            _dbSet = _context.Set<T>();

        }

        public async Task<IEnumerable<T>> GetAll()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<T> GetById(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task<bool> Add(T entity)
        {
            try
            {
                var result = await _dbSet.AddAsync(entity);
                if (result!=null)
                    return true;
                return false;
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "{Repo} Add method error", typeof(GenericRepository<T>));
                return false;
            }
        }

        public async Task<bool> AddRange(IEnumerable<T> entities)
        {
            try
            {
                await _dbSet.AddRangeAsync(entities);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Repo} Add Range method error", typeof(GenericRepository<T>));
                return false;
            }
        }

        public Task<bool> Update(T entity)
        {
            try
            {
                var result = _dbSet.Update(entity);
                if (result != null)
                    return Task.FromResult(true);
                return Task.FromResult(false);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Repo} Update method error", typeof(GenericRepository<T>));
                return Task.FromResult(false);
            }
        }

        public Task<bool> Delete(T entity)
        {
            try
            {
                _dbSet.Remove(entity);
                return Task.FromResult(true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Repo} Delete method error", typeof(GenericRepository<T>));
                return Task.FromResult(false);
            }
        }
    }
}
