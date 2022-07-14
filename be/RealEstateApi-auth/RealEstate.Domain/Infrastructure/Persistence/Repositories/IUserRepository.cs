using RealEstate.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Domain.Infrastructure.Persistence.Repositories
{
    public interface IUserRepository: IGenericRepository<User>
    {
        Task<bool> CheckUserExist(User user);
        Task<bool> AddVerifyPhone(VerifyPhone verify);
        Task<User> GetById(string id);
        Task<VerifyPhone> GetVerifyPhone(string phone);
        Task<IEnumerable<Role>> GetRoles();
        Task<Role> GetRoleByUserId(string id);
        Task<IEnumerable<User>> GetAllUsers(string? roleID);
        Task<User> CheckEmailExist(string email);
        Task<User> CheckUsernameExist(string username);
        Task<UserRole> GetUserRole(string userId);
        Task<IEnumerable<User>> GetUsers();
    }
}
