using AutoMapper;
using Azure.Storage.Blobs;
using IdentityModel;
using Microsoft.AspNetCore.Identity;
using PagedList;
using RealEstate.Application.DTOs;
using RealEstate.Application.IServices;
using RealEstate.Application.Models;
using RealEstate.Domain.Entities;
using RealEstate.Domain.Infrastructure.Persistence.UoW;
using RealEstate.Domain.Values;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using MimeKit;
using MailKit.Security;
using System.Text.RegularExpressions;

namespace RealEstate.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private UserManager<User> _userManager;
        private RoleManager<IdentityRole> _roleManager;
        private int pageSize = 10;

        public UserService(IUnitOfWork unitOfWork, IMapper mapper, UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<IEnumerable<Role>> GetAllRoles()
        {
            
            var result = await _unitOfWork.Users.GetRoles();
            return result;
        }

        public async Task<UserList> GetUsers(UserFilterParams filterParams)
        {
            int pageNumber = (filterParams.Page ?? 1);
            pageSize = (filterParams.Size ?? pageSize);
            UserList userList = new UserList();
            var result = await _unitOfWork.Users.GetAllUsers(filterParams.RoleID);


            if (result != null && result.Count() > 0)
            {
                if (!String.IsNullOrEmpty(filterParams.Search) && !String.IsNullOrWhiteSpace(filterParams.Search))
                {
                    List<User> value = new List<User>();
                    foreach(var r in result)
                    {
                        if (r.Name.ToLower().Contains(filterParams.Search.ToLower())|| (r.Email != null && r.Email.ToLower().Contains(filterParams.Search.ToLower())) || (r.PhoneNumber!=null && r.PhoneNumber.ToLower().Contains(filterParams.Search.ToLower())))
                            value.Add(r);
                    }
                    result = value;
                }

                var users = result.Select(user => _mapper.Map<User, UserDto>(user)).ToList();
                foreach(var user in users)
                {
                    var role = await _unitOfWork.Users.GetRoleByUserId(user.Id);
                    if(role!=null)
                    {
                        user.RoleID = role.ID;
                        user.RoleName = role.Name;
                    }    
                }

                userList.PageSize = pageSize;
                userList.TotalSize = users.Count();
                userList.Users = users.ToPagedList(pageNumber, pageSize); //them thu vien cho phan trang
                userList.PageNumber = pageNumber;
                return userList;
            }

            userList.PageSize = pageSize;
            userList.TotalSize = 0;
            userList.PageNumber = 1;
            userList.Users = new List<UserDto>();
            return userList;
        }

       

        public async Task<ResponseUser> CreateUser(UserRequest userRequest)  //tạo người dùng bằng mail, o day role là User hoặc Admin, Staff
        {
            ResponseUser response = new ResponseUser();

            if (String.IsNullOrEmpty(userRequest.Email))
            {
                response.Succeeded = false;
                response.Errors = "Email là trường yêu cầu.";
                response.User = null;
                return response;
            }

            var result = await _unitOfWork.Users.CheckEmailExist(userRequest.Email);
            if (result!=null)
            {
                response.Succeeded = false;
                response.Errors = "Email đã được đăng ký.";
                response.User = null;
                return response;
            }

            var role = await _roleManager.FindByIdAsync(userRequest.RoleID);
            //var role = await _roleManager.RoleExistsAsync(userRequest.RoleName);
            if (role == null)
            {
                response.Succeeded = false;
                response.Errors = "Không có role phù hợp.";
                response.User = null;
                return response;
            }

            User user = _mapper.Map<User>(userRequest);
            if (role.Name == "User")
            {
                if (String.IsNullOrEmpty(userRequest.PhoneNumber))
                {
                    response.Succeeded = false;
                    response.Errors = "Số điện thoại là trường yêu cầu với loại người dùng này.";
                    response.User = null;
                    return response;
                }

                var username = await _unitOfWork.Users.CheckUsernameExist(userRequest.PhoneNumber.Trim());
                if (username != null)
                {
                    response.Succeeded = false;
                    response.Errors = "Số điện thoại này đã được sử dụng là username cho tài khoản khác. Vui lòng sử dụng số điện thoại khác.";
                    response.User = null;
                    return response;
                }

                user.UserName = userRequest.PhoneNumber.Trim();
            }    
            else
            {
                if (String.IsNullOrEmpty(userRequest.Email))
                {
                    response.Succeeded = false;
                    response.Errors = "Email là trường yêu cầu với loại người dùng này.";
                    response.User = null;
                    return response;
                }

                var username = await _unitOfWork.Users.CheckUsernameExist(userRequest.Email.Trim());
                if (username != null)
                {
                    response.Succeeded = false;
                    response.Errors = "Email này đã được sử dụng là username cho tài khoản khác. Vui lòng sử dụng email khác.";
                    response.User = null;
                    return response;
                }

                user.UserName = userRequest.Email.Trim();
            }    
                
            user.RegisteredDate = DateTime.Now;

            if (userRequest.Image != null)
            {
                BlobContainerClient blobContainerClient = new BlobContainerClient(AzureInfo.ConnectionString, AzureInfo.ContainerName);
                string imageName =  Guid.NewGuid().ToString();
                int indexEx = userRequest.Image.FileName.LastIndexOf(".");
                string extension = userRequest.Image.FileName.Substring(indexEx);
                string fileName = imageName + extension;

                BlobClient blobClient = blobContainerClient.GetBlobClient(fileName);
                var memoryStream = new MemoryStream();
                await userRequest.Image.CopyToAsync(memoryStream);
                memoryStream.Position = 0;
                await blobClient.UploadAsync(memoryStream);

                string url = blobClient.Uri.AbsoluteUri;
                if (url != null && url != "")
                {
                    user.ImageUrl = url;
                }
            }

            PasswordOptions passwordOptions = new PasswordOptions();  //thay đổi option cho password
            passwordOptions.RequireDigit = true;
            passwordOptions.RequireLowercase = true;
            passwordOptions.RequireNonAlphanumeric = false;
            passwordOptions.RequireUppercase = true;
            passwordOptions.RequiredLength = 6;
            passwordOptions.RequiredUniqueChars = 0;

            string password = GenerateRandomPassword(passwordOptions);
            if(passwordOptions.RequiredUniqueChars==0)
                password = Regex.Replace(password, @"[^a-zA-Z0-9]", m => "9");//tạo mật khẩu tự động

            Gmail gmail = new Gmail();
            gmail.To = userRequest.Email.Trim();
            gmail.From = "testgoog96@gmail.com";
            gmail.Subject = "Cấp tài khoản đăng nhập";
            gmail.Body = "<p>Username của bạn l&agrave; <span style=\"color: #3598db;\">" + user.UserName + "</span>.</p><p>Mật khẩu đăng nhập tạm thời của bạn l&agrave; <span style=\"color: #3598db;\">" + password + "</span>. Vui l&ograve;ng thay đổi lại mật khẩu khi đăng nhập th&agrave;nh c&ocirc;ng.</p>";
            //gmail.Body = "Mật khẩu đăng nhập tạm thời của bạn là: " + password;

            try
            {
                MimeMessage message = new MimeMessage();
                MailboxAddress from = new MailboxAddress("Admin",gmail.From);
                message.From.Add(from);

                MailboxAddress to = new MailboxAddress(user.Name,gmail.To);
                message.To.Add(to);
                message.Subject = gmail.Subject;
                BodyBuilder bodyBuilder = new BodyBuilder();
                bodyBuilder.HtmlBody = gmail.Body;
                //bodyBuilder.TextBody = "Hello World!";
                message.Body = bodyBuilder.ToMessageBody();
                
                SmtpClient client = new SmtpClient();
                client.Connect("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
                client.Authenticate("testgoog96@gmail.com", "kouxdyncpvnvlthw");
                client.Send(message);

                var value = await _userManager.CreateAsync(user, password);
                if (value.Succeeded)
                {
                    var roleidentity = await _roleManager.FindByNameAsync(role.Name);
                    await _userManager.AddToRoleAsync(user, role.Name);   //o day role là User hoặc Admin
                    response.Succeeded = true;
                    response.Errors = "Tạo tài khoản thành công.";
                    UserDto userDto = _mapper.Map<UserDto>(user);
                    userDto.RoleID = roleidentity.Id;
                    userDto.RoleName = roleidentity.Name;
                    response.User = userDto;


                    return response;
                }
            }
            catch (Exception ex)
            {
                response.Succeeded = false;
                response.Errors = "Có lỗi trong quá trình thực hiện.";
                response.User = null;
                return response;
            }
            response.Succeeded = false;
            response.Errors = "Quá trình thực hiện thất bại.";
            response.User = null;
            return response;
        }

        

        public async Task<UserDto> GetUserById(string userID)
        {
            var result = await _userManager.FindByIdAsync(userID);
            UserDto user = _mapper.Map<UserDto>(result);
            if (user != null)
            {
                var role = await _unitOfWork.Users.GetRoleByUserId(result.Id);
                if (role != null)
                {
                    user.RoleID = role.ID;
                    user.RoleName = role.Name;
                }
            }
            
           
            return user;
        }

        //vướng mắc: do user dùng sdt làm username, staff và admin dùng email để làm username nên nếu chuyển loại tài khoản từ user thành staff, admin hoặc ngược lại làm username cũng phải thay đổi theo
        //phòng trường hợp quên mật khẩu nên không thay không được nhưng nếu thay thì có thể gây ra trùng nếu như email, sdt đó đã đc sử dụng làm username của tài khoản khác -> vậy không cho phép chuyển từ tài khoản user -> .. hoặc ngược lại
        //chỉ cho phép cập nhật thông tin không phải là thông tin username , không được thay đổi loại tài khoản
        public async Task<ResponseUser> UpdateUser(UserDto userDto)
        {
            ResponseUser response = new ResponseUser();

            User user = _mapper.Map<User>(userDto);
            var result = await _userManager.FindByIdAsync(userDto.Id);
            
            if (result == null)
            {
                response.Succeeded = false;
                response.Errors = "Thông tin người dùng không tồn tại.";
                return response;
            }

            var role = await _unitOfWork.Users.GetRoleByUserId(user.Id);
            if (role == null)
            {
                response.Succeeded = false;
                response.Errors = "Không có role phù hợp.";
                return response;
            }

            if (role.Name == "Admin" || role.Name == "Staff")
            {  
                if (String.IsNullOrEmpty(userDto.Email))
                {
                    response.Succeeded = false;
                    response.Errors = "Email là trường bắt buộc với loại người dùng này.";
                    response.User = null;
                    return response;
                }    

                if (!result.Email.Equals(userDto.Email.Trim()))
                {
                    var username = await _unitOfWork.Users.CheckUsernameExist(userDto.Email.Trim());
                    if (username != null)
                    {
                        response.Succeeded = false;
                        response.Errors = "Email này đã được sử dụng là username cho tài khoản khác. Vui lòng sử dụng email khác.";
                        response.User = null;
                        return response;
                    }
                    result.UserName = userDto.Email.Trim();
                    result.Email = userDto.Email;
                    result.PhoneNumber = userDto.PhoneNumber;
                }  
            }
            else if (role.Name == "User")
            {
                if (String.IsNullOrEmpty(userDto.PhoneNumber))
                {
                    response.Succeeded = false;
                    response.Errors = "Số điện thoại là trường bắt buộc với loại người dùng này.";
                    response.User = null;
                    return response;
                }

                if (!result.PhoneNumber.Equals(userDto.PhoneNumber.Trim()))
                {
                    var username = await _unitOfWork.Users.CheckUsernameExist(userDto.PhoneNumber.Trim());
                    if (username != null)
                    {
                        response.Succeeded = false;
                        response.Errors = "Số điện thoại này đã được sử dụng là username cho tài khoản khác. Vui lòng sử dụng số điện thoại khác.";
                        response.User = null;
                        return response;
                    }

                    result.UserName = userDto.PhoneNumber.Trim();
                    result.Email = userDto.Email;
                    result.PhoneNumber = userDto.PhoneNumber;
                }    
            }

            result.Address = userDto.Address;
            result.Birthday = userDto.Birthday;
            result.Gender = userDto.Gender;
            result.Name = userDto.Name;
            
            

            if (userDto.Image != null)
            {
                BlobContainerClient blobContainerClient = new BlobContainerClient(AzureInfo.ConnectionString, AzureInfo.ContainerName);
                string imageName = Guid.NewGuid().ToString();
                int indexEx = userDto.Image.FileName.LastIndexOf(".");
                string extension = userDto.Image.FileName.Substring(indexEx);
                string fileName = imageName + extension;

                BlobClient blobClient = blobContainerClient.GetBlobClient(fileName);
                var memoryStream = new MemoryStream();
                await userDto.Image.CopyToAsync(memoryStream);
                memoryStream.Position = 0;
                await blobClient.UploadAsync(memoryStream);

                string url = blobClient.Uri.AbsoluteUri;
                if (url != null && url != "")
                {
                    result.ImageUrl = url;
                }
            }
            else
            {
                result.ImageUrl = userDto.ImageUrl;
            }

            var success = await _userManager.UpdateAsync(result);
            if (success.Succeeded)
            {
                response.Succeeded = true;
                response.Errors = "Cập nhật thành công.";

                UserDto u = _mapper.Map<UserDto>(result);
                u.RoleID = role.ID;
                u.RoleName = role.Name;
                response.User = u;

                return response;
            }

            response.Succeeded = false;
            response.Errors = "Quá trình thực hiện thất bại.";
            return response;
        }


        public string GenerateRandomPassword(PasswordOptions opts = null)
        {
            if (opts == null) opts = new PasswordOptions()
            {
                RequiredLength = 8,
                RequiredUniqueChars = 4,
                RequireDigit = true,
                RequireLowercase = true,
                RequireNonAlphanumeric = true,
                RequireUppercase = true
            };

            string[] randomChars = new[] {
                "ABCDEFGHJKLMNOPQRSTUVWXYZ",    // uppercase 
                "abcdefghijkmnopqrstuvwxyz",    // lowercase
                "0123456789",                   // digits
                "!@$?_-"                        // non-alphanumeric
            };
            
            CryptoRandom rand = new CryptoRandom();
            List<char> chars = new List<char>();

            if (opts.RequireUppercase)
                chars.Insert(rand.Next(0, chars.Count),
                    randomChars[0][rand.Next(0, randomChars[0].Length)]);

            if (opts.RequireLowercase)
                chars.Insert(rand.Next(0, chars.Count),
                    randomChars[1][rand.Next(0, randomChars[1].Length)]);

            if (opts.RequireDigit)
                chars.Insert(rand.Next(0, chars.Count),
                    randomChars[2][rand.Next(0, randomChars[2].Length)]);

            if (opts.RequireNonAlphanumeric)
                chars.Insert(rand.Next(0, chars.Count),
                    randomChars[3][rand.Next(0, randomChars[3].Length)]);

            for (int i = chars.Count; i < opts.RequiredLength
                || chars.Distinct().Count() < opts.RequiredUniqueChars; i++)
            {
                string rcs = randomChars[rand.Next(0, randomChars.Length)];
                chars.Insert(rand.Next(0, chars.Count),
                    rcs[rand.Next(0, rcs.Length)]);
            }

            return new string(chars.ToArray());
        }

        public async Task<UserDto> LockUnlock(string userId)
        {
            UserDto userDto = new UserDto();
            var obj = await _userManager.FindByIdAsync(userId);
            if (obj ==null)
            {
                return null;
            }
            if(obj.LockoutEnd!=null && obj.LockoutEnd > DateTime.Now) 
            {
                obj.LockoutEnd = DateTime.Now;
                userDto = _mapper.Map<UserDto>(obj);
            }
            else
            {
                obj.LockoutEnd = DateTime.Now.AddYears(1000);
                userDto = _mapper.Map<UserDto>(obj);
            }
            await _userManager.UpdateAsync(obj);
            var role = await _unitOfWork.Users.GetRoleByUserId(obj.Id);
            userDto.RoleID = role.ID;
            userDto.RoleName = role.Name;
            return userDto;
        }

    }
}
