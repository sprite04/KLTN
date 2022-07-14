using RealEstate.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Domain.Infrastructure.Persistence.Repositories
{
    public interface IPostRepository
    {
        Task<IEnumerable<Post>> GetAllPosts(string sort, ulong? min, ulong? max);
        Task<IEnumerable<Post>> GetPostsByUser(string id);
        Task<IEnumerable<Post>> GetPostsByAdmin(string sort, ulong? min, ulong? max);
        Task<IEnumerable<Post>> Search(string search, string sort, ulong? minPrice, ulong? maxPrice); //xem lai co can ham nay o day hay khong
        Task<int> GetPostTypeNumber(int index);
        Task<Post> GetPostById(int id);
        Task<Post> AddPost(Post post);
        Task<Post> UpdatePost(Post post);
        bool DeletePost(int id);
        bool DeleteImagesOfPost(int id);
        Task<bool> Sold(int id, string userId);
        Task<IEnumerable<Post>> GetPostsWithReport(); // lấy danh sách các bài viết bị báo cáo, nhưng bài viết đó chưa bị khoá, chức năng sắp xếp với những bài viết có report chưa được đọc nằm phía trên -> cần được bổ sung sau
    }
}
