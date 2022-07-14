using Microsoft.Extensions.Logging;
using RealEstate.Domain.Entities;
using RealEstate.Domain.Infrastructure.Persistence.Repositories;
using RealEstate.Domain.Infrastructure.Persistence.UoW;
using RealEstate.Infrastructure.Persistence.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Infrastructure.Persistence.UoW
{
    public class UnitOfWork:IUnitOfWork
    {
        private readonly RealEstateDbContext _context;
        private readonly ILogger _logger;
        public IPostRepository Posts { get; private set; }
        public IGenericRepository<Category> Categories { get; private set; }
        public IGenericRepository<PostType> PostTypes { get; private set; }
        //public IGenericRepository<User> Users { get; private set; }

        public IRefreshTokenRepository RefreshTokens { get; private set; }
        public IGenericRepository<Image> Images { get; private set; }
        public IReportRepository Reports { get; private set; }
        public INotificationRepository Notifications { get; private set; }
        public IUserRepository Users { get; }
        public IFollowRepository Follows { get; private set; }
        public IFavoritePostRepository FavoritePosts { get; private set; }
        public IReportProcessingRepository ReportProcessing { get; private set; }

        public INewsRepository News { get; private set; }

        public UnitOfWork(RealEstateDbContext context, ILoggerFactory loggerFactory)
        {
            _context = context;
            _context.ChangeTracker.LazyLoadingEnabled = false;
            _logger = loggerFactory.CreateLogger("logs");
            Posts = new PostRepository(_context, _logger);
            Follows = new FollowRepository(_context, _logger);
            FavoritePosts = new FavoritePostRepository(_context, _logger);
            Categories = new GenericRepository<Category>(_context, _logger);
            PostTypes = new GenericRepository<PostType>(_context, _logger);
            //ReportTypes = new GenericRepository<ReportType>(_context, _logger);
            Reports = new ReportRepository(_context, _logger);
            Users=new UserRepository(_context, _logger);
            Images = new GenericRepository<Image>(_context, _logger);
            RefreshTokens = new RefreshTokenRepository(_context, _logger);
            Notifications = new NotificationRepository(_context, _logger);
            ReportProcessing = new ReportProcessingRepository(_context, _logger);
            News = new NewsRepository(_context, _logger);
        }

        public async Task CompleteAsync()
        {
            await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.DisposeAsync();
        }
    }
}
