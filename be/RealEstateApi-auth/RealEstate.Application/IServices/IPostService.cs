using RealEstate.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RealEstate.Application.Models;
using RealEstate.Domain.Entities;

namespace RealEstate.Application.IServices
{
    public interface IPostService
    {
        public Task<PostList> GetAllPosts(FilterParams filterParams);
        public Task<PostsUserCurrent> GetPostsByUserCurrent(string id);
        public Task<PostList> GetPostsByAdmin(FilterParams filterParams);
        public Task<PostTypeNumber> GetPostTypeNumber();
        public Task<PostsUser> GetPostsByUser(string id, string userCurrentID);
        public Task<PostList> Search(FilterParams filterParams);
        public Task<PostDto> GetPostById(int id, string? userID);
        public Task<PostDto> ChangeStatus(PostStatus postStatus);
        public Task<bool> AddPost(PostDto post);
        public Task<bool> UpdatePost(PostDto postDto);
        public bool DeletePost(int id);
        public Task<Report1> GetReport1();
        public Task<Report2> GetReport2();
        public Task<Report3> GetReport3();
        public Task<Report4> GetReport4(int id);
        public Task<Response> Sold(int id, string userId);
        public Task<IEnumerable<Post>> GetPostsWithReport(); //lấy ra danh sách những bài post bị báo cáo -> cần xem việc sắp xếp (sắp xếp theo ngày hay sao trong trường hợp bài đó đã được xử lý, hoặc bài viết đã bị khoá, hoặc báo cáo của bài viết đó đã được xử lý) -> chưa thực hiện xong cần chỉnh
        public Task<IEnumerable<Post>> GetPostsDueToExplain(); //lấy ra danh sách những bài post đến hạn giải trình -> việc sắp xếp cũng cần được thực hiện, xếp theo ngày hay sao, cần bỏ qua những bài viết đến hạn nhưng đã được xử lý rồi thì không hiện ở đây nữa
    }
}
