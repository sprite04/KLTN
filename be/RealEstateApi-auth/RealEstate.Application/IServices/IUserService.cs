using RealEstate.Application.DTOs;
using RealEstate.Application.Models;
using RealEstate.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Application.IServices
{
    public interface IUserService
    {
        public Task<IEnumerable<Role>> GetAllRoles();
        public Task<UserList> GetUsers(UserFilterParams userFilterParams);
        public Task<ResponseUser> CreateUser(UserRequest userRequest);
        public Task<ResponseUser> UpdateUser(UserDto userDto);
        public Task<UserDto> GetUserById(string userID);
        public Task<UserDto> LockUnlock(string userId);
    }
}
