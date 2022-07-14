using RealEstate.Application.DTOs;
using RealEstate.Application.Models;
using RealEstate.Application.Models.Request;
using RealEstate.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Application.IServices
{
    public interface IAuthService
    {
        Task<bool> CreateUser(UserDto post);
        Task<bool> Logout(string id);
        Task<User> ExistUser(string id);
        Task<Info> GetUserById(string id);
        Task<Response> Register(RegisterRequest register);
        Task<AuthenticatedUserResponse> Login(LoginRequest request);
        Task<Response> VerifyPhoneNumber(string phone);
        Task<Response> SendCodeResetPassword(string p);
        Task<Response> ResetPassword(ResetPassword model);
        Task<AuthenticatedUserResponse> Refresh(RefreshRequest refreshRequest);
        Task<Response> ChangePassword(ChangePasswordRequest changePassword, string username);
        Task<Response> ChangeInfo(ChangeInfoRequest changeInfo, string username);

    }
}
