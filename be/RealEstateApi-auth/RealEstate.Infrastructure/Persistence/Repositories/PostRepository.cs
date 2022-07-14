using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RealEstate.Application.DTOs;
using RealEstate.Domain.Entities;
using RealEstate.Domain.Infrastructure.Persistence.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Infrastructure.Persistence.Repositories
{
    public class PostRepository: GenericRepository<Post>, IPostRepository
    {
        protected DbSet<Image> dbSetImage;
        public PostRepository(RealEstateDbContext context, ILogger logger):base(context, logger)
        {
            dbSetImage = context.Set<Image>();
        }

        public async Task<IEnumerable<Post>> GetAllPosts(string sort, ulong? min, ulong? max)
        {
            ulong minValue = min ?? 0;
            ulong maxValue = max ?? ulong.MaxValue;

            if (minValue > maxValue)
                return new List<Post>();
            

            var result = await _dbSet.Where(x => x.IsDeleted == false && x.Price >= minValue && x.Price <= maxValue && x.StatusID==2 && x.IsSold==false).Include(x => x.Images).Include(y => y.Creator).Include(x => x.Category).Include(z => z.PostType).OrderByDescending(x => x.ID).ToListAsync();

            if (result != null && result.Count() > 0)
                return Sort(result, sort);
            return new List<Post>();
        }

        public async Task<int> GetPostTypeNumber(int index)
        {
            if (index == 1)  //Mua ban
            {
                var posts = await _dbSet.Where(x => x.IsDeleted == false && x.StatusID == 2 &&x.IsSold==false && (x.PostTypeID == 1 || x.PostTypeID == 2)).ToListAsync();
                return posts!=null ? posts.Count():0;
            }

            if(index == 2) //Cho thue
            {
                var posts = await _dbSet.Where(x => x.IsDeleted == false && x.StatusID == 2 && x.IsSold == false && (x.PostTypeID == 3 || x.PostTypeID == 4)).ToListAsync();
                return posts != null ? posts.Count() : 0;
            }

            return 0;
        }

        public async Task<IEnumerable<Post>> GetPostsWithReport() // lấy danh sách các bài viết bị báo cáo, nhưng bài viết đó chưa bị khoá, chức năng sắp xếp với những bài viết có report chưa được đọc nằm phía trên -> cần được bổ sung sau
        {
            var result = await _dbSet.Where(x => x.IsDeleted == false && x.StatusID != 4 && x.Reports.Count() > 0).ToListAsync();
            return result;
        }

        public async Task<IEnumerable<Post>> GetPostsByAdmin(string sort, ulong? min, ulong? max)
        {
            ulong minValue = min ?? 0;
            ulong maxValue = max ?? ulong.MaxValue;

            if (minValue > maxValue)
                return new List<Post>();


            var result = await _dbSet.Where(x => x.IsDeleted == false && x.Price >= minValue && x.Price <= maxValue).Include(x => x.Images).Include(y => y.Creator).Include(x => x.Category).Include(z => z.PostType).OrderByDescending(x => x.ID).ToListAsync();

            if (result != null && result.Count() > 0)
                return result.OrderBy(x => x.CreatedDate);
            return new List<Post>();
        }

        public async Task<IEnumerable<Post>> GetPostsByUser(string id)
        {
            var result = await _dbSet.Where(x => x.IsDeleted == false && x.CreatorID.Equals(id)).Include(x => x.Images).Include(y => y.Creator).Include(x => x.Category).Include(z => z.PostType).ToListAsync();
            if (result != null && result.Count() > 0)
                return result;
            return new List<Post>();
        }

        public async Task<Post> GetPostById(int id)
        {
            return await _dbSet.Where(x => x.ID == id && x.IsDeleted == false).Include(x => x.Images).Include(y => y.Creator).Include(x=>x.Category).Include(z=>z.PostType).FirstOrDefaultAsync();
        }

        public Task<IEnumerable<Post>> Search(string search, string sort, ulong? minPrice, ulong? maxPrice)
        {
            throw new NotImplementedException();
        }

        public async Task<Post> AddPost(Post post)
        {
            try
            {
                var result = await _dbSet.AddAsync(post);
                return result.Entity;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Repo} Add method error", typeof(PostRepository));
                return null;
            }
        }

        public async Task<Post> UpdatePost(Post post)
        {
            try
            {
                var exit = await _dbSet.Where(x => x.ID == post.ID).FirstOrDefaultAsync();

                if (exit == null)
                    return null;


                exit.Address = post.Address;
                exit.Area = post.Area;
                exit.Bathrooms = post.Bathrooms;
                exit.Bedrooms = post.Bedrooms;
                exit.CategoryID = post.CategoryID;
                exit.CreatorID = post.CreatorID;
                exit.Details = post.Details;
                exit.DirectionID = post.DirectionID;
                exit.DistrictID = post.DistrictID;
                exit.ID = post.ID;
                exit.PaperID = post.PaperID;
                exit.PostTypeID = post.PostTypeID;
                exit.Price = post.Price;
                exit.ProvinceID = post.ProvinceID;
                exit.Title = post.Title;
                exit.WardID = post.WardID;
                exit.IsSold = post.IsSold;
                

                return exit;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Repo} Update method error", typeof(PostRepository));
                return null;
            }
        }


        public async Task<bool> Sold(int id, string userId)
        {
            try
            {
                var exit = await _dbSet.Where(x => x.ID == id && x.IsDeleted == false && x.CreatorID==userId).FirstOrDefaultAsync();

                if (exit == null)
                    return false;

                exit.IsSold = true;

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Repo} Delete method error", typeof(PostRepository));
                return false;
            }
        }


        public bool DeletePost(int id)
        {
            try
            {
                var exit = _dbSet.Where(x => x.ID == id && x.IsDeleted == false).FirstOrDefault();

                if (exit == null)
                    return false;

                exit.IsDeleted = true;

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Repo} Delete method error", typeof(PostRepository));
                return false;
            }
        }


        private IEnumerable<Post> Sort(IEnumerable<Post> posts, string sort)  //xem lai co sort theo ngay tao moi nhat hay khong
        {
            if (!String.IsNullOrEmpty(sort) && sort.Equals("price:desc"))
            {
                return posts.OrderByDescending(x => x.Price);
            }

            if (!String.IsNullOrEmpty(sort) && sort.Equals("price:asc"))
            {
                return posts.OrderBy(x => x.Price);
            }
            return posts;
        }

        public bool DeleteImagesOfPost(int id)
        {
            try
            {
                var images = dbSetImage.Where(x => x.PostID == id).ToList();
                if (images != null && images.Count() > 0)
                {
                    dbSetImage.RemoveRange(images);
                }
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Repo} Delete Post method error", typeof(PostRepository));
                return false;
            }
        }
    }
}
