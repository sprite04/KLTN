using Azure.Storage.Blobs;
using RealEstate.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RealEstate.Domain.Values;
using System.IO;
using RealEstate.Application.IServices;
using RealEstate.Domain.Entities;
using RealEstate.Domain.Infrastructure.Persistence.UoW;
using AutoMapper;
using PagedList;
using RealEstate.Application.Models;

namespace RealEstate.Application.Services
{
    public class PostService : IPostService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private int pageSize = 10;

        public PostService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<PostList> GetPostsByAdmin(FilterParams filterParams)
        {
            int pageNumber = (filterParams.Page ?? 1);
            pageSize = (filterParams.Size ?? pageSize);
            PostList postList = new PostList();
            var result = await _unitOfWork.Posts.GetPostsByAdmin(filterParams.Sort, filterParams.MinPrice, filterParams.MaxPrice);

            if (!String.IsNullOrEmpty(filterParams.Search) && !String.IsNullOrWhiteSpace(filterParams.Search))
            {
                result = result.Where(x => x.Title.ToLower().Contains(filterParams.Search.ToLower()));
            }
            if (filterParams.CategoryID != 0)
            {
                result = result.Where(x => x.CategoryID == filterParams.CategoryID);
            }
            if (filterParams.PostTypeID != 0)
            {
                if (filterParams.PostTypeID == 1)
                    result = result.Where(x => x.PostTypeID == 1 || x.PostTypeID == 2);
                else if (filterParams.PostTypeID == 2)
                    result = result.Where(x => x.PostTypeID == 3 || x.PostTypeID == 4);
            }
            if (filterParams.ProvinceID != 0)
            {
                result = result.Where(x => x.ProvinceID == filterParams.ProvinceID);
            }
            if (filterParams.DistrictID != 0)
            {
                result = result.Where(x => x.DistrictID == filterParams.DistrictID);
            }
            if (filterParams.StatusID != null)
            {
                result = result.Where(x => x.StatusID == filterParams.StatusID);
            }
            if (filterParams.UserID != null)
            {
                result = result.Where(x => x.CreatorID.Equals(filterParams.UserID));
            }
            if (result != null && result.Count() > 0)
            {
                var posts = result.Select(post => _mapper.Map<Post, PostDto>(post)).ToList();
                
                postList.TotalSize = posts.Count();
                postList.Posts = posts.ToPagedList(pageNumber, pageSize); 
                postList.PageNumber = pageNumber;
                return postList;
            }

            postList.TotalSize = 0;
            postList.PageNumber = 1;
            postList.Posts = new List<PostDto>();
            return postList;
        }

        public async Task<Report1> GetReport1()
        {
            Report1 report1 = new Report1();
            var resultUser = await _unitOfWork.Users.GetAllUsers(null); //lấy toàn bộ kể cả admin, staff, user cho dù khoá hay k
            if(resultUser!=null && resultUser.Count() > 0)
            {
                var users = resultUser.Where(x => x.RegisteredDate != null && x.RegisteredDate.Value.Month == DateTime.Now.Month && x.RegisteredDate.Value.Year == DateTime.Now.Year);
                if(users!=null && users.Count() > 0)
                {
                    report1.NewUser = users.Count();
                }
                else
                {
                    report1.NewUser = 0;
                }
            }
            else
            {
                report1.NewUser = 0;
            }
            var resultTotalPosts = await _unitOfWork.Posts.GetPostsByAdmin("",0, int.MaxValue);
            if(resultTotalPosts!=null && resultTotalPosts.Count() > 0)
            {
                var totalPosts = resultTotalPosts.Where(x => x.CreatedDate != null && x.CreatedDate.Value.Month == DateTime.Now.Month && x.CreatedDate.Value.Year == DateTime.Now.Year);
                if(totalPosts!=null && totalPosts.Count() > 0)
                {
                    report1.TotalPosts = totalPosts.Count();
                    var totalPostNews = totalPosts.Where(x => x.StatusID == 1);
                    if(totalPostNews!=null && totalPostNews.Count() > 0)
                    {
                        report1.TotalPostNews = totalPostNews.Count();
                    }
                    else
                    {
                        report1.TotalPostNews = 0;
                    }
                }
                else
                {
                    report1.TotalPosts = 0;
                    report1.TotalPostNews = 0;
                }
            }
            else
            {
                report1.TotalPosts = 0;
                report1.TotalPostNews = 0;
            }
            return report1;
        }

        public async Task<Report2> GetReport2()
        {
            Report2 report = new Report2();
            report.TotalPosts = new List<int>();
            report.TotalPostsApproved = new List<int>();
            var resultTotalPosts = await _unitOfWork.Posts.GetPostsByAdmin("", 0, int.MaxValue);
            if(resultTotalPosts!=null && resultTotalPosts.Count() > 0)
            {
                resultTotalPosts = resultTotalPosts.Where(x => x.CreatedDate != null && x.CreatedDate.Value.Year == DateTime.Now.Year);
                if(resultTotalPosts!=null && resultTotalPosts.Count() > 0)
                {
                    for (int i = 1; i <= DateTime.Now.Month; i++)
                    {
                        var month = resultTotalPosts.Where(x => x.CreatedDate != null && x.CreatedDate.Value.Month == i);
                        if(month!=null && month.Count() > 0)
                        {
                            report.TotalPosts.Add(month.Count());
                            var monthApproved = month.Where(x => x.StatusID == 2);
                            if(monthApproved!=null && monthApproved.Count() > 0)
                                report.TotalPostsApproved.Add(monthApproved.Count());
                            else
                                report.TotalPostsApproved.Add(0);
                        }
                        else
                        {
                            report.TotalPosts.Add(0);
                            report.TotalPostsApproved.Add(0);
                        }
                            
                    }
                }
                else
                {
                    for (int i = 1; i <= DateTime.Now.Month; i++)
                    {
                        report.TotalPosts.Add(0);
                        report.TotalPostsApproved.Add(0);
                    }
                }
            }
            else
            {
                
                for(int i=1; i<=DateTime.Now.Month; i++)
                {
                    report.TotalPosts.Add(0);
                    report.TotalPostsApproved.Add(0);
                }
            }
            return report;
        }

        public async Task<Report3> GetReport3()
        {
            Report3 report3 = new Report3();
            report3.TotalCurrentYear = new List<int>();
            report3.TotalLastYear = new List<int>();

            var result = await _unitOfWork.Users.GetAllUsers(null);
            if(result!=null && result.Count() > 0)
            {
                var current = result.Where(x => x.RegisteredDate != null && x.RegisteredDate.Value.Year == DateTime.Now.Year);
                var last = result.Where(x => x.RegisteredDate != null && x.RegisteredDate.Value.Year == DateTime.Now.Year-1);

                if(current!=null && current.Count() > 0)
                {
                    for (int i = 1; i <= DateTime.Now.Month; i++)
                    {
                        var monthCurrent = current.Where(x => x.RegisteredDate != null && x.RegisteredDate.Value.Month == i);
                        if(monthCurrent!=null && monthCurrent.Count()>0)
                            report3.TotalCurrentYear.Add(monthCurrent.Count());
                        else
                            report3.TotalCurrentYear.Add(0);
                    }
                }
                else
                {
                    for (int i = 1; i <= DateTime.Now.Month; i++)
                    {
                        report3.TotalCurrentYear.Add(0);
                    }
                }

                if(last!=null && last.Count() > 0)
                {
                    for (int i = 1; i <= 12; i++)
                    {
                        var monthLast = last.Where(x => x.RegisteredDate != null && x.RegisteredDate.Value.Month == i);
                        if (monthLast != null && monthLast.Count() > 0)
                            report3.TotalLastYear.Add(monthLast.Count());
                        else
                            report3.TotalLastYear.Add(0);
                    }
                }
                else
                {
                    for (int i = 1; i <= 12; i++)
                    {
                        report3.TotalLastYear.Add(0);
                    }
                }
            }
            else
            {
                for (int i = 1; i <= DateTime.Now.Month; i++)
                {
                    report3.TotalCurrentYear.Add(0);
                }

                for (int i=1; i<=12; i++)
                {
                    report3.TotalLastYear.Add(0);
                }
            }
            return report3;
        }

        public async Task<Report4> GetReport4(int id)
        {
            Report4 report = new Report4();
            report.AppartmentPrice = new List<float>();
            report.LandPrice = new List<float>();
            report.HousePrice = new List<float>();
            report.OfficePrice = new List<float>();

            for(int i=1; i<= DateTime.Now.Month; i++)
            {
                var postList = await _unitOfWork.Posts.GetAllPosts("", 0, int.MaxValue);
                var appartment = postList.Where(x => x.ProvinceID == id && x.CategoryID == 1 && x.CreatedDate!=null && x.CreatedDate.Value.Month == i && x.CreatedDate.Value.Year == DateTime.Now.Year);
                var house = postList.Where(x => x.ProvinceID == id && x.CategoryID == 2 && x.CreatedDate != null && x.CreatedDate.Value.Month == i && x.CreatedDate.Value.Year == DateTime.Now.Year);
                var land = postList.Where(x => x.ProvinceID == id && x.CategoryID == 3 && x.CreatedDate != null && x.CreatedDate.Value.Month == i && x.CreatedDate.Value.Year == DateTime.Now.Year);
                var office = postList.Where(x => x.ProvinceID == id && x.CategoryID == 4 && x.CreatedDate != null && x.CreatedDate.Value.Month == i && x.CreatedDate.Value.Year == DateTime.Now.Year);
                
                if(appartment!=null && appartment.Count() > 0)
                {
                    float price = 0;
                    float sum = 0;
                    int index = 0;
                    foreach (var a in appartment)
                    {
                        if(a.Price!=null && a.Area!=null)
                        {
                            sum += (a.Price * 1.0f ?? 1.0f) / (a.Area ?? 1);
                            index++;
                        }
                    }
                    if(index!=0)
                        price = (sum / index)/1000000;
                    report.AppartmentPrice.Add(price);
                }
                else
                {
                    report.AppartmentPrice.Add(0);
                }

                if(house != null && house.Count() > 0)
                {
                    float price = 0;
                    float sum = 0;
                    int index = 0;
                    foreach (var a in house)
                    {
                        if (a.Price != null && a.Area != null)
                        {
                            sum += (a.Price * 1.0f ?? 1.0f) / (a.Area ?? 1);
                            index++;
                        }
                    }
                    if (index != 0)
                        price = (sum / index) / 1000000;
                    report.HousePrice.Add(price);
                }
                else
                {
                    report.HousePrice.Add(0);
                }

                if (land != null && land.Count() > 0)
                {
                    float price = 0;
                    float sum = 0;
                    int index = 0;
                    foreach (var a in land)
                    {
                        if (a.Price != null && a.Area != null)
                        {
                            sum += (a.Price * 1.0f ?? 1.0f) / (a.Area ?? 1);
                            index++;
                        }
                    }
                    if (index != 0)
                        price = (sum / index) / 1000000;
                    report.LandPrice.Add(price);
                }
                else
                {
                    report.LandPrice.Add(0);
                }

                if (office != null && office.Count() > 0)
                {
                    float price = 0;
                    float sum = 0;
                    int index = 0;
                    foreach (var a in office)
                    {
                        if (a.Price != null && a.Area != null)
                        {
                            sum += (a.Price*1.0f ??1.0f) / (a.Area ?? 1);
                            index++;
                        }
                    }
                    if (index != 0)
                        price = (sum / index) / 1000000;
                    report.OfficePrice.Add(price);
                }
                else
                {
                    report.OfficePrice.Add(0);
                }
            }

            var AppartmentPrice = report.AppartmentPrice.Where(x => x > 0);
            if (AppartmentPrice != null && AppartmentPrice.Count() > 0)
                report.AvgApartmentPrice = AppartmentPrice.Sum() / AppartmentPrice.Count();
            else
                report.AvgApartmentPrice = 0;
            var HousePrice = report.HousePrice.Where(x => x > 0);
            if (HousePrice != null && HousePrice.Count() > 0)
                report.AvgHousePrice = HousePrice.Sum() / HousePrice.Count();
            else
                report.AvgHousePrice = 0;

            var LandPrice = report.LandPrice.Where(x => x > 0);
            if(LandPrice!=null && LandPrice.Count()>0)
                report.AvgLandPrice = LandPrice.Sum() / LandPrice.Count();
            else
                report.AvgLandPrice = 0;

            var OfficePrice = report.OfficePrice.Where(x => x > 0);
            if (OfficePrice != null && OfficePrice.Count() > 0)
                report.AvgOfficePrice = OfficePrice.Sum() / OfficePrice.Count();
            else
                report.AvgOfficePrice = 0;

            return report;
        }

        public async Task<PostList> GetAllPosts(FilterParams filterParams)
        {
            int pageNumber = (filterParams.Page ?? 1);
            pageSize = (filterParams.Size ?? pageSize);
            PostList postList = new PostList();
            var result = await _unitOfWork.Posts.GetAllPosts(filterParams.Sort, filterParams.MinPrice, filterParams.MaxPrice);
            if (result != null && result.Count() > 0)
            {
                var posts = result.Select(post => _mapper.Map<Post, PostDto>(post)).ToList();
                foreach (var post in posts)
                {
                    if (filterParams.UserID != null)
                    {
                        var likeExist = await _unitOfWork.FavoritePosts.CheckExist(filterParams.UserID, post.ID);
                        if (likeExist != null)
                        {
                            post.Like = true;
                        }
                        else
                        {
                            post.Like = false;
                        }
                    }
                    else
                    {
                        post.Like = false;
                    }
                }
                postList.TotalSize = posts.Count();
                postList.Posts = posts.ToPagedList(pageNumber, pageSize); 
                postList.PageNumber = pageNumber;
                return postList;
            }

            postList.TotalSize = 0;
            postList.PageNumber = 1;
            postList.Posts = new List<PostDto>();
            return postList;
        }

        public async Task<PostDto> GetPostById(int id, string? userID)      
        {
            Post post= await _unitOfWork.Posts.GetPostById(id);
            if (post != null)
            {
                PostDto postDto = _mapper.Map<Post, PostDto>(post);
                if (userID != null)
                {
                    var likeExist = await _unitOfWork.FavoritePosts.CheckExist(userID, post.ID);
                    if (likeExist != null)
                    {
                        postDto.Like = true;
                    }
                    else
                    {
                        postDto.Like = false;
                    }
                }
                else
                {
                    postDto.Like = false;
                }
                return postDto;
            }
            return null;
        }

        public async Task<PostsUserCurrent> GetPostsByUserCurrent(string id) //lay ra tat ca cac bai dang nguoi dung dang dang nhap
        {
            PostsUserCurrent postsUser = new PostsUserCurrent();
            var result = await _unitOfWork.Posts.GetPostsByUser(id);
            var user = await _unitOfWork.Users.GetById(id);
            postsUser.ID = user.Id;
            postsUser.Name = user.Name;
            postsUser.FollowCount = user.FollowUsers.Count();
            postsUser.FollowedCount = user.FollowedUsers.Count();

            if (result != null && result.Count() > 0)
            {
                var posts = result.Select(post => _mapper.Map<Post, PostDto>(post)).ToList(); 
                foreach (var post in posts)
                {
                    if (id != null)
                    {
                        var likeExist = await _unitOfWork.FavoritePosts.CheckExist(id, post.ID);
                        if (likeExist != null)
                        {
                            post.Like = true;
                        }
                        else
                        {
                            post.Like = false;
                        }
                    }
                    else
                    {
                        post.Like = false;
                    }
                }
                postsUser.PostPending = posts.Where(x => x.StatusID == 1);
                postsUser.PostShowing = posts.Where(x => x.StatusID == 2);
                postsUser.PostDenied = posts.Where(x => x.StatusID == 3);
                
                return postsUser;
            }

            postsUser.PostPending = new List<PostDto>();
            postsUser.PostShowing = new List<PostDto>();
            postsUser.PostDenied = new List<PostDto>();
            return postsUser;
        }

        public async Task<PostsUser> GetPostsByUser(string id,string userCurrentID) //chi lay ra cac bai dang da duoc duyet
        {
            PostsUser postsUser = new PostsUser();
            var result = await _unitOfWork.Posts.GetPostsByUser(id);
            var user = await _unitOfWork.Users.GetById(id);
            postsUser.ID = user.Id;
            postsUser.Name = user.Name;
            postsUser.Avatar = user.ImageUrl;
            postsUser.FollowCount = user.FollowUsers.Count();
            postsUser.FollowedCount = user.FollowedUsers.Count();

            if (result != null && result.Count() > 0)
            {
                var posts = result.Where(x=>x.StatusID==2).Select(post => _mapper.Map<Post, PostDto>(post)).ToList();
                foreach (var post in posts)
                {
                    if (id != null)
                    {
                        var likeExist = await _unitOfWork.FavoritePosts.CheckExist(userCurrentID, post.ID);
                        if (likeExist != null)
                        {
                            post.Like = true;
                        }
                        else
                        {
                            post.Like = false;
                        }
                    }
                    else
                    {
                        post.Like = false;
                    }
                }
                postsUser.Posts = posts; //them thu vien cho phan trang
                return postsUser;
            }

            postsUser.Posts = new List<PostDto>();
            return postsUser;
        }

        public async Task<PostDto> ChangeStatus(PostStatus postStatus)
        {
            Response response = new Response();
            Post post = await _unitOfWork.Posts.GetPostById(postStatus.ID);
            if (post == null)
                return null;
            try
            {
                post.StatusID = postStatus.StatusID;
                var result = await _unitOfWork.Posts.UpdatePost(post);
                await _unitOfWork.CompleteAsync();

                PostDto postDto = _mapper.Map<Post, PostDto>(post);
                
                return postDto;
            }
            catch (Exception)
            {
                
                return null;
            }


            /*if (post == null)
            {
                response.Succeeded = false;
                response.Errors = "Không tồn tại bài đăng.";
                return response;
            }
            if(!(postStatus.StatusID>=1&& postStatus.StatusID <= 3))
            {
                response.Succeeded = false;
                response.Errors = "Không có trạng thái phù hợp.";
                return response;
            }
            try
            {
                post.StatusID = postStatus.StatusID;
                var result = await _unitOfWork.Posts.UpdatePost(post);
                await _unitOfWork.CompleteAsync();
                response.Succeeded = true;
                response.Errors = "Quá trình thực hiện thành công.";
                return response;
            }
            catch (Exception)
            {
                response.Succeeded = false;
                response.Errors = "Quá trình thực hiện thất bại.";
                return response;
            }*/
        }



        public async Task<bool> AddPost(PostDto postDto)
        {
            Post post = _mapper.Map<Post>(postDto);
            post.StatusID = 1;
            post.CreatedDate = DateTime.Now;

            bool isValid = await CheckPost(post);
            if (!isValid)
                return false;

            var result= await _unitOfWork.Posts.AddPost(post);
            await _unitOfWork.CompleteAsync();
            if (result is null)
                return false;

            if (postDto.ImageList != null && postDto.ImageList.Count() > 0)
            {
                BlobContainerClient blobContainerClient = new BlobContainerClient(AzureInfo.ConnectionString, AzureInfo.ContainerName);

                List<Image> images = new List<Image>();
                postDto.ImageUrls = new List<string>();

                foreach (var image in postDto.ImageList)
                {
                    string imageName = post.CreatorID + Guid.NewGuid().ToString();
                    int indexEx = image.FileName.LastIndexOf(".");
                    string extension = image.FileName.Substring(indexEx);
                    string fileName = imageName + extension;



                    BlobClient blobClient = blobContainerClient.GetBlobClient(fileName);
                    var memoryStream = new MemoryStream();
                    await image.CopyToAsync(memoryStream);
                    memoryStream.Position = 0;
                    await blobClient.UploadAsync(memoryStream);

                    string url = blobClient.Uri.AbsoluteUri;
                    if (url != null && url != "")
                    {
                        Image img = new Image();
                        img.PostID = result.ID;
                        img.ImageUrl = url;
                        images.Add(img);
                    }
                }

                if (images.Count > 0)
                    await _unitOfWork.Images.AddRange(images);

                await _unitOfWork.CompleteAsync();
            }
            
            return true;
        }

        public async Task<IEnumerable<Post>> GetPostsWithReport()
        {
            //kiểm tra:
            //kiểm tra xem bài viết đã bị khoá hay chưa, bài viết có bị xoá hay không
            //cần sắp xếp lại các bài đăng nào có report chưa đọc lên trên
            var result = await _unitOfWork.Posts.GetPostsWithReport();
            return result;
        }

        public async Task<IEnumerable<Post>> GetPostsDueToExplain()
        {
            HashSet<Post> posts = new HashSet<Post>();

            var result = await _unitOfWork.ReportProcessing.GetReportProcessingDueDate();

            foreach(var report in result)
            {
                if (report.StatusID <3 && report.StatusID > 0)
                {
                    var post = await _unitOfWork.Posts.GetPostById(report.PostID);
                    if (post != null)
                        posts.Add(post);
                }    
            }

            return posts.OrderBy(x => x.CreatedDate).ToList(); //cần check lại ở đây xem thứ tự sắp xếp đúng hay chưa
        }

        public async Task<bool> UpdatePost(PostDto postDto)
        {
            Post post = _mapper.Map<Post>(postDto);

            var exit = await _unitOfWork.Posts.GetPostById(post.ID);
            if (exit is null)
                return false;

            bool isValid = await CheckPost(post);
            if (!isValid)
                return false;

            var result = await _unitOfWork.Posts.UpdatePost(post);
            try
            {
                await _unitOfWork.CompleteAsync();
            }
            catch(Exception ex)
            {
                return false;
            }

            if (result is null)
                return false;

            _unitOfWork.Posts.DeleteImagesOfPost(post.ID);
            await _unitOfWork.CompleteAsync();

            if(postDto.ImageUrls!=null && postDto.ImageUrls.Count() > 0)
            {
                List<Image> images = new List<Image>();
                foreach (var image in postDto.ImageUrls)
                {
                    if (image != null)
                    {
                        Image img = new Image();
                        img.PostID = result.ID;
                        img.ImageUrl = image;
                        images.Add(img);
                    }
                        
                }
                if (images.Count > 0)
                {
                    try
                    {
                        await _unitOfWork.Images.AddRange(images);
                        await _unitOfWork.CompleteAsync();
                    }
                    catch (Exception ex)
                    {

                    }
                }
            }
            
            if(postDto.ImageList!=null && postDto.ImageList.Count() > 0)
            {
                BlobContainerClient blobContainerClient = new BlobContainerClient(AzureInfo.ConnectionString, AzureInfo.ContainerName);
                List<Image> images = new List<Image>();

                foreach (var image in postDto.ImageList)
                {
                    string imageName = post.CreatorID + Guid.NewGuid().ToString();
                    int indexEx = image.FileName.LastIndexOf(".");
                    string extension = image.FileName.Substring(indexEx);
                    string fileName = imageName + extension;

                    BlobClient blobClient = blobContainerClient.GetBlobClient(fileName);
                    var memoryStream = new MemoryStream();
                    await image.CopyToAsync(memoryStream);

                    memoryStream.Position = 0;
                    await blobClient.UploadAsync(memoryStream);
                    string url = blobClient.Uri.AbsoluteUri;
                    if (url != null && url != "")
                    {
                        Image img = new Image();
                        img.PostID = result.ID;
                        img.ImageUrl = url;
                        images.Add(img);

                        
                    }
                }

                try
                {
                    await _unitOfWork.Images.AddRange(images);
                    await _unitOfWork.CompleteAsync();
                }
                catch (Exception ex)
                {

                }


            }
            
            return true;
        }

        public async Task<Response> Sold(int id, string userId)
        {
            Response response = new Response();
            /*var exit = await _unitOfWork.Posts.GetPostById(id);
            if (exit is null)
            {
                response.Succeeded = false;
                response.Errors = "Bài viết không tồn tại.";
                return response;
            }

            if (!exit.CreatorID.Equals(userId))
            {
                response.Succeeded = false;
                response.Errors = "Người dùng không được phép thay đổi trạng thái bài đăng.";
                return response;
            }

            if (exit.IsSold==true)
            {
                response.Succeeded = false;
                response.Errors = "Không thể thay đổi trạng thái bài đăng.";
                return response;
            }
            exit.IsSold = true;*/

            try
            {
                var result = await _unitOfWork.Posts.Sold(id,userId);
                if (result == true)
                {
                    await _unitOfWork.CompleteAsync();
                    response.Succeeded = true;
                    response.Errors = "Thay đổi trạng thái bài đăng thành công.";
                    return response;
                }
               
            }
            catch(Exception ex)
            {
                response.Succeeded = false;
                response.Errors = "Thay đổi trạng thái bài đăng thất bại.";
                return response;
            }
            response.Succeeded = false;
            response.Errors = "Thay đổi trạng thái bài đăng thất bại.";
            return response;
        }

        public bool DeletePost(int id) //----------------------------------can kiem tra nguoi ban co phai la nguoi tao ra bai dang hay khong
        {
            var result = _unitOfWork.Posts.DeletePost(id);
            if (result)
            {
                _unitOfWork.CompleteAsync();
                return true;
            }
            return false;
        }

        private async Task<bool> CheckPost(Post post)
        {
            var category = await _unitOfWork.Categories.GetById(post.CategoryID);
            if (category is null)
                return false;

            var posttype = await _unitOfWork.PostTypes.GetById(post.PostTypeID);
            if (posttype is null)
                return false;

            var user = await _unitOfWork.Users.GetById(post.CreatorID);
            if (user is null)
                return false;

            return true;
        }

        public async Task<PostTypeNumber> GetPostTypeNumber()
        {
            PostTypeNumber postTypeNumber = new PostTypeNumber();
            postTypeNumber.MuaBan = await _unitOfWork.Posts.GetPostTypeNumber(1);
            postTypeNumber.Thue = await _unitOfWork.Posts.GetPostTypeNumber(2);
            return postTypeNumber;
        }

        public async Task<PostList> Search(FilterParams filterParams)
        {
            int pageNumber = (filterParams.Page ?? 1);
            pageSize = (filterParams.Size ?? pageSize);
            PostList postList = new PostList();
            var result = await _unitOfWork.Posts.GetAllPosts(filterParams.Sort, filterParams.MinPrice, filterParams.MaxPrice);

            if (!String.IsNullOrEmpty(filterParams.Search) && !String.IsNullOrWhiteSpace(filterParams.Search))
            {
                result = result.Where(x => x.Title.ToLower().Contains(filterParams.Search.ToLower()));
            }
            if (filterParams.CategoryID != 0)
            {
                result = result.Where(x => x.CategoryID == filterParams.CategoryID);
            }
            if (filterParams.PostTypeID != 0)
            {
                if (filterParams.PostTypeID == 1)
                    result = result.Where(x => x.PostTypeID == 1 || x.PostTypeID == 2);
                else if(filterParams.PostTypeID == 2)
                    result = result.Where(x => x.PostTypeID == 3 || x.PostTypeID == 4);
            }
            if (filterParams.ProvinceID != 0)
            {
                result = result.Where(x => x.ProvinceID == filterParams.ProvinceID);
            }
            if (filterParams.DistrictID != 0)
            {
                result = result.Where(x => x.DirectionID == filterParams.DistrictID);
            }
            if (filterParams.StatusID != null)
            {
                result = result.Where(x => x.StatusID == filterParams.StatusID);
            }

            if (result != null && result.Count() > 0)
            {
                var posts = result.Select(post => _mapper.Map<Post, PostDto>(post)).ToList();
                foreach (var post in posts)
                {
                    if (filterParams.UserID != null)
                    {
                        var likeExist = await _unitOfWork.FavoritePosts.CheckExist(filterParams.UserID, post.ID);
                        if (likeExist != null)
                        {
                            post.Like = true;
                        }
                        else
                        {
                            post.Like = false;
                        }
                    }
                    else
                    {
                        post.Like = false;
                    }
                }
                postList.TotalSize = posts.Count();
                postList.Posts = posts.ToPagedList(pageNumber, pageSize); 
                postList.PageNumber = pageNumber;
                return postList;
            }
            postList.TotalSize = 0;
            postList.PageNumber = 1;
            postList.Posts = new List<PostDto>();
            return postList;
        }
    }
}
