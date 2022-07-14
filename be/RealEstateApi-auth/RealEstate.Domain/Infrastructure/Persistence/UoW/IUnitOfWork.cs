using RealEstate.Domain.Entities;
using RealEstate.Domain.Infrastructure.Persistence.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Domain.Infrastructure.Persistence.UoW
{
    public interface IUnitOfWork
    {
        IPostRepository Posts { get; }
        IFollowRepository Follows { get; }
        IFavoritePostRepository FavoritePosts { get; }
        //IGenericRepository<ReportType> ReportTypes { get; }
        IReportRepository Reports { get; }
        INewsRepository News { get; }
        IGenericRepository<Category> Categories { get; }
        IGenericRepository<PostType> PostTypes { get; }
        IGenericRepository<Image> Images { get; }
        //IGenericRepository<User> Users { get; }
        IUserRepository Users { get; }
        IRefreshTokenRepository RefreshTokens { get; }
        INotificationRepository Notifications { get; }
        IReportProcessingRepository ReportProcessing { get; }
        Task CompleteAsync();
    }
}
