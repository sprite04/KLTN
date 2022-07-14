using AutoMapper;
using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using RealEstate.Application.DTOs;
using RealEstate.Application.IServices;
using RealEstate.Application.Models;
using RealEstate.Application.Models.Request;
using RealEstate.Application.Tokens;
using RealEstate.Application.Tokens.Authenticators;
using RealEstate.Application.Tokens.TokenValidators;
using RealEstate.Domain.Entities;
using RealEstate.Domain.Infrastructure.Persistence.UoW;
using RealEstate.Domain.Values;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Twilio;
using Twilio.Clients;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace RealEstate.Application.Services
{
    public class AuthService: IAuthService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private UserManager<User> _userManager;
        private SignInManager<User> _signInManager;
        private RoleManager<IdentityRole> _roleManager;

        private readonly IConfiguration _configuration;

        private readonly AccessTokenGenerator _accessTokenGenerator;
        private readonly RefreshTokenGenerator _refreshTokenGenerator;
        private readonly RefreshTokenValidator _refreshTokenValidator;
        private readonly Authenticator _authenticator;
        /*private readonly string accountSid = "AC0cb61fd486a5e9b7f3b34ee1feae5840";
        private readonly string authToken = "5b85d3ccd5dd2fd9a104e60c1731f950";*/
        /*private readonly string phoneNumberTwilio = "+17407205651";*/
        private readonly string accountSid = "AC411250be92ecf83a7bf713543200a2e7";
        private readonly string authToken = "0a3f95c0edfbdbf19fff081f6cbbba57";
        private readonly string phoneNumberTwilio = "+19166341724";
        public AuthService(IUnitOfWork unitOfWork, IMapper mapper, UserManager<User> userManager, SignInManager<User> signInManager, 
            IConfiguration configuration, AccessTokenGenerator accessTokenGenerator, RefreshTokenGenerator refreshTokenGenerator, 
            RefreshTokenValidator refreshTokenValidator, Authenticator authenticator, RoleManager<IdentityRole> roleManager)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
            _accessTokenGenerator = accessTokenGenerator;
            _refreshTokenGenerator = refreshTokenGenerator;
            _refreshTokenValidator = refreshTokenValidator;
            _authenticator = authenticator;
            _roleManager = roleManager;
        }

        public async Task<User> ExistUser(string id)
        {
            var user = await _unitOfWork.Users.GetById(id);
            return user;
        }

        public async Task<bool> CreateUser(UserDto userDto)
        {
            User user = _mapper.Map<User>(userDto);

            bool isExist = await _unitOfWork.Users.CheckUserExist(user);
            if (isExist)
                return false;
            var result = await _unitOfWork.Users.Add(user);
            if (!result)
                return false;
            try
            {
                await _unitOfWork.CompleteAsync();
                return true;
            }
            catch(Exception ex)
            {
                
            }
            return false;
        }

        public async Task<AuthenticatedUserResponse> Login(LoginRequest request)
        {
            if(!await _roleManager.RoleExistsAsync("Admin"))
            {
                await _roleManager.CreateAsync(new IdentityRole("Admin"));
                await _roleManager.CreateAsync(new IdentityRole("Staff"));
                await _roleManager.CreateAsync(new IdentityRole("User"));
            }
            User user = _mapper.Map<User>(request);
            var result = await _signInManager.PasswordSignInAsync(user.UserName, request.Password, false, lockoutOnFailure: false);

            if (result.Succeeded)
            {
                var value = await _userManager.FindByNameAsync(user.UserName);
                UserDto userDto = _mapper.Map<UserDto>(value);
                var role = await _unitOfWork.Users.GetRoleByUserId(value.Id);
                if (role != null)
                {
                    userDto.RoleID = role.ID;
                    userDto.RoleName = role.Name;
                    userDto.ImageUrl = value.ImageUrl;
                }

                


                AuthenticatedUserResponse response = await _authenticator.Authenticate(userDto);

                return response;
            }
            return null;
        }

        public async Task<bool> Logout(string id)
        {
            try
            {
                await _unitOfWork.RefreshTokens.DeleteByIdUser(id);
                await _unitOfWork.CompleteAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
            
        }

        public async Task<Info> GetUserById(string id)
        {
            
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
                return null;
            Info info = _mapper.Map<User, Info>(user);
            return info;

        }

        public async Task<Response> ChangeInfo(ChangeInfoRequest changeInfo, string username)
        {
            Response response = new Response();
            var value = await _userManager.FindByNameAsync(username);
            if (value == null)
            {
                response.Succeeded = false;
                response.Errors = "Thông tin người dùng không tồn tại.";
                return response;
            }

            value.Name = changeInfo.Name;
            value.Address = changeInfo.Address;
            value.Gender = changeInfo.Gender;
            value.Birthday = changeInfo.Birthday;

            if (changeInfo.Image == null)
                value.ImageUrl = changeInfo.ImageUrl;
            else
            {
                BlobContainerClient blobContainerClient = new BlobContainerClient(AzureInfo.ConnectionString, AzureInfo.ContainerName);
                string imageName = value.Id + Guid.NewGuid().ToString();
                int indexEx = changeInfo.Image.FileName.LastIndexOf(".");
                string extension = changeInfo.Image.FileName.Substring(indexEx);
                string fileName = imageName + extension;

                BlobClient blobClient = blobContainerClient.GetBlobClient(fileName);
                var memoryStream = new MemoryStream();
                await changeInfo.Image.CopyToAsync(memoryStream);
                memoryStream.Position = 0;
                await blobClient.UploadAsync(memoryStream);

                string url = blobClient.Uri.AbsoluteUri;
                if (url != null && url != "")
                {
                    value.ImageUrl = url;
                }
            }
            var result = await _userManager.UpdateAsync(value);
            if (result.Succeeded)
            {
                response.Succeeded = true;
                response.Errors = "Thay đổi thông tin thành công.";
                return response;
            }
            response.Succeeded = false;
            response.Errors = "Thay đổi thông tin không thành công. Vui lòng thử lại.";
            return response;
        }

        public async Task<Response> ChangePassword(ChangePasswordRequest changePassword, string username)
        {
            Response response = new Response();
            if (changePassword.OldPassword.Equals(changePassword.Password))
            {
                response.Succeeded = false;
                response.Errors = "Mật khẩu mới không được trùng với mật khẩu cũ.";
                return response;
            }
            var result = await _signInManager.PasswordSignInAsync(username, changePassword.OldPassword, false, lockoutOnFailure: false);
            if (result.Succeeded)
            {
                var value = await _userManager.FindByNameAsync(username);
                var isChanged = await _userManager.ChangePasswordAsync(value, changePassword.OldPassword, changePassword.Password);
                if (isChanged.Succeeded)
                {
                    response.Succeeded = true;
                    response.Errors = "Thay đổi mật khẩu thành công.";
                    return response;
                }
                
                response.Succeeded = false;
                response.Errors = "Thay đổi mật khẩu thất bại.";
                return response;
            }
            response.Succeeded = false;
            response.Errors = "Mật khẩu không chính xác. Vui lòng kiểm tra lại.";
            return response;
        }
        
        public async Task<AuthenticatedUserResponse> Refresh(RefreshRequest refreshRequest)
        {
            bool isValidRefreshToken = _refreshTokenValidator.Validate(refreshRequest.RefreshToken);
            if (!isValidRefreshToken)
                return null;

            RefreshToken refreshTokenDTO = await _unitOfWork.RefreshTokens.GetByToken(refreshRequest.RefreshToken);
            if (refreshTokenDTO == null)
                return null;

            await _unitOfWork.RefreshTokens.DeleteById(refreshTokenDTO.ID);
            await _unitOfWork.CompleteAsync();

            User user = await _unitOfWork.Users.GetById(refreshTokenDTO.UserID);
            if (user == null)
                return null;

            UserDto userDto = _mapper.Map<UserDto>(user);
            var role = await _unitOfWork.Users.GetRoleByUserId(user.Id);
            if (role != null)
            {
                userDto.RoleID = role.ID;
                userDto.RoleName = role.Name;
            }

            AuthenticatedUserResponse response = await _authenticator.Authenticate(userDto);
            return response;
        }

        public async Task<Response> Register(RegisterRequest register)
        {
            if (!await _roleManager.RoleExistsAsync("Admin"))
            {
                await _roleManager.CreateAsync(new IdentityRole("Admin"));
                await _roleManager.CreateAsync(new IdentityRole("Staff"));
                await _roleManager.CreateAsync(new IdentityRole("User"));
            }  

            Response response = new Response();
            User user = _mapper.Map<User>(register);

            bool isExist = await _unitOfWork.Users.CheckUserExist(user);
            if(isExist)
            {
                response.Succeeded = false;
                response.Errors = "Số điện thoại này đã được đăng ký trước đó. Bạn hãy thử lại với số điện thoại khác.";
                return response;
            }
            string phone = user.PhoneNumber.Trim();
            user.UserName = phone;
            user.RegisteredDate = DateTime.Now;
        
            VerifyPhone verify = await _unitOfWork.Users.GetVerifyPhone(phone);
            if (verify == null || register.Code==null)
            {
                return await VerifyPhoneNumber(phone);
            }
            else
            {
                if((DateTime.Now- verify.CreatedDate).TotalSeconds < 600)
                {
                    if (register.Code == verify.Code)
                    {
                        try
                        {
                            var value = await _userManager.CreateAsync(user, register.Password);//dang thuc hien o day can chinh sua lai
                            

                            if (value.Succeeded)
                            {
                                await _userManager.AddToRoleAsync(user, "User");
                                response.Succeeded = true;
                                response.Errors = "Đăng ký thành công.";
                                return response;
                            }
                            response.Succeeded = false;
                            response.Errors = AddErrors(value);
                            return response;
                        }
                        catch (Exception ex)
                        {

                            throw;
                        }
                        
                    }
                    response.Succeeded = false;
                    response.Errors = "Mã xác thực không hợp lệ.";
                    return response; 
                }
                response.Succeeded = false;
                response.Errors = "Mã xác thực vượt quá thời hạn";
                return response;
            }
        }

        public async Task<Response> VerifyPhoneNumber(string p)
        {
            string phone = "+84" + p.Trim().Substring(1);
            Response response = new Response();
            VerifyPhone verify = new VerifyPhone();

            var client = new TwilioRestClient(accountSid, authToken);

            Random random = new Random();
            int otp = random.Next(111111, 999999);

            MessageResource message;
            try
            {
                message = MessageResource.Create(
                    from: new PhoneNumber(phoneNumberTwilio), // From number, must be an SMS-enabled Twilio number
                    to: new PhoneNumber(phone), // To number, if using Sandbox see note above
                                                // Message content
                    body: $"Your Real Estate App verification code is: {otp}",
                    client: client);
            }
            catch(Exception ex)
            {
                response.Succeeded = false;
                response.Errors = "Số điện thoại không hợp lệ.";
                return response;
            }
            DateTime createdDate = new DateTime();

            if (message.Status != MessageResource.StatusEnum.Failed)
            {
                verify.Code = otp;
                verify.CreatedDate= message.DateCreated ?? DateTime.Now;
                verify.PhoneNumber = p.Trim();
                await _unitOfWork.Users.AddVerifyPhone(verify);
                await _unitOfWork.CompleteAsync();
                response.Succeeded = true;
                return response;
            }

            response.Succeeded = false;
            response.Errors = "Quá trình gửi mã OTP đã thất bại";

            return response;
        }

        public async Task<Response> SendCodeResetPassword(string p)
        {
            var user = await _userManager.FindByNameAsync(p.Trim());
            Response response = new Response();
            if (user != null)
            {
                string phone = "+84" + user.PhoneNumber.Substring(1);
                VerifyPhone verify = new VerifyPhone();

                var client = new TwilioRestClient(accountSid, authToken);

                Random random = new Random();
                int otp = random.Next(111111, 999999);

                MessageResource message;
                try
                {
                    message = MessageResource.Create(
                        from: new PhoneNumber(phoneNumberTwilio), // From number, must be an SMS-enabled Twilio number
                        to: new PhoneNumber(phone), // To number, if using Sandbox see note above
                                                    // Message content
                        body: $"Your Real Estate App verification code is: {otp}",
                        client: client);
                }
                catch (Exception ex)
                {
                    response.Succeeded = false;
                    response.Errors = "Số điện thoại không hợp lệ.";
                    return response;
                }
                DateTime createdDate = new DateTime();

                if (message.Status != MessageResource.StatusEnum.Failed)
                {
                    var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                    verify.ResetToken = token;
                    verify.Code = otp;
                    verify.CreatedDate = message.DateCreated ?? DateTime.Now;
                    verify.PhoneNumber = user.PhoneNumber.Trim();
                    await _unitOfWork.Users.AddVerifyPhone(verify);
                    await _unitOfWork.CompleteAsync();
                    response.Succeeded = true;
                    return response;
                }

                response.Succeeded = false;
                response.Errors = "Quá trình gửi mã OTP đã thất bại";

                return response;
            }

            response.Succeeded = false;
            response.Errors = "Không tồn tại số điện thoại này.";
            return response;
        }

        public async Task<Response> ResetPassword(ResetPassword model)
        {
            Response response = new Response();
            if (String.IsNullOrEmpty(model.PhoneNumber))
            {
                response.Succeeded = false;
                response.Errors = "Cần điền số điện thoại";
                return response;
            }
            var user = await _userManager.FindByNameAsync(model.PhoneNumber.Trim());
            
            if(user==null)
            {
                response.Succeeded = false;
                response.Errors = "Số điện thoại chưa được đăng ký bởi tài khoản nào.";
                return response;
            }

            VerifyPhone verify =await _unitOfWork.Users.GetVerifyPhone(model.PhoneNumber);
            if (verify == null || model.Code == null)//chinh sua lai khuc nay do co them token
            {
                response.Succeeded = false;
                response.Errors = "Mã xác thực không hợp lệ.";
                return response;
            }
            else
            {
                if ((DateTime.Now - verify.CreatedDate).TotalSeconds < 600 && !String.IsNullOrEmpty(verify.ResetToken))
                {
                    if (model.Code == verify.Code)
                    {
                        var value = await _userManager.ResetPasswordAsync(user, verify.ResetToken, model.Password);
                        if (value.Succeeded)
                        {
                            response.Succeeded = true;
                            response.Errors = "Thay đổi mật khẩu thành công.";
                            return response;
                        }
                        response.Succeeded = false;
                        response.Errors = AddErrors(value);
                        return response;
                    }
                    response.Succeeded = false;
                    response.Errors = "Mã xác thực không hợp lệ.";
                    return response;
                }
                response.Succeeded = false;
                response.Errors = "Mã xác thực vượt quá thời hạn";
                return response;
            }
        }

        

        private string AddErrors(IdentityResult result)
        {
            string str = "";
            foreach (var error in result.Errors)
            {
                str += error.Description;
            }
            return str;
        }

        
     
        

        /*public string VerifyPhoneNumber(string phone)
        {
            string accountSid = "AC0cb61fd486a5e9b7f3b34ee1feae5840";
            string authToken = "5b85d3ccd5dd2fd9a104e60c1731f950";
            var client = new TwilioRestClient(accountSid, authToken);

            Random random = new Random();
            int otp = random.Next(111111, 999999);

            MessageResource message;
            try
            {
                message = MessageResource.Create(
                    from: new PhoneNumber("+17407205651"), // From number, must be an SMS-enabled Twilio number
                    to: new PhoneNumber(phone), // To number, if using Sandbox see note above
                                                // Message content
                    body: $"Your Real Estate App verification code is: {otp}",
                    client: client);
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            DateTime createdDate = new DateTime();

            if (message.Status != MessageResource.StatusEnum.Failed)
            {
                createdDate = message.DateCreated ?? DateTime.Now;
            }

            return message.AccountSid;
        }*/
    }
}
