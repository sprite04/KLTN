using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RealEstate.Application.Models;
using RealEstate.Domain.Entities;
using RealEstate.Domain.Infrastructure.Persistence.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Infrastructure.Persistence.Repositories
{
    public class UserRepository: GenericRepository<User>,IUserRepository
    {
        protected DbSet<VerifyPhone> _dbSetPhone;
        protected RealEstateDbContext _context;
        public UserRepository(RealEstateDbContext context, ILogger logger) : base(context, logger)
        {
            _dbSetPhone = context.Set<VerifyPhone>();
            _context = context;
        }

        public async Task<User> GetById(string id)
        {
            return await _dbSet.Where(x => x.Id.Equals(id)).Include(x => x.FollowedUsers).Include(y => y.FollowUsers).FirstOrDefaultAsync();
        }
        public Task<bool> CheckUserExist(User user)
        {
            var result= _dbSet.Where(x => x.PhoneNumber.Equals(user.PhoneNumber.Trim())).FirstOrDefault();
            if (result != null)
                return Task.FromResult(true);
            return Task.FromResult(false);
        }

        public async Task<User> CheckUsernameExist(string username)
        {
            var result = await _dbSet.Where(x => x.UserName.Equals(username.Trim())).FirstOrDefaultAsync();
            return result;
        }

        public async Task<User> CheckEmailExist(string email)
        {
            var result = await _dbSet.Where(x => x.Email.Equals(email.Trim())).FirstOrDefaultAsync();
            return result;
        }

        public Task<VerifyPhone> GetVerifyPhone(string phone)
        {
            var result = _dbSetPhone.Where(x => x.PhoneNumber.Trim().Equals(phone.Trim())).FirstOrDefault();
            return Task.FromResult(result);
        }

        public async Task<bool> AddVerifyPhone(VerifyPhone verify)
        {
            var result = await GetVerifyPhone(verify.PhoneNumber);
            try
            {
                if (result == null)
                    await _dbSetPhone.AddAsync(verify);
                else
                {
                    result.CreatedDate = verify.CreatedDate;
                    result.Code = verify.Code;
                    result.ResetToken = verify.ResetToken;
                    _dbSetPhone.Update(result);
                }
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Repo} Add Verify Phone method error", typeof(UserRepository));
                return false;
            }
            
        }

        public async Task<IEnumerable<Role>> GetRoles()
        {
            List<Role> roleList = new List<Role>();
            var roles = _context.Roles.ToList();
            if (roles.Count() > 0)
            {
                foreach (var role in roles)
                {
                    Role re = new Role();
                    re.ID = role.Id;
                    re.Name = role.Name;
                    roleList.Add(re);
                }
            }
            return roleList;
        }

        public async Task<IEnumerable<User>> GetUsers()  
        {
            var result = await _dbSet.Where(x => x.LockoutEnd == null || x.LockoutEnd < DateTime.Now).OrderBy(x=>x.RegisteredDate).ToListAsync();
            return result;
        }

        public async Task<Role> GetRoleByUserId(string id)
        {
            var userrole = await _context.UserRoles.FirstOrDefaultAsync(x => x.UserId.Equals(id));
            if (userrole != null)
            {
                var result = await _context.Roles.FirstOrDefaultAsync(x => x.Id.Equals(userrole.RoleId));
                Role role = new Role();
                role.ID = result.Id;
                role.Name = result.Name;
                return role;
            }
            return null;
        }

        public async Task<IEnumerable<User>> GetAllUsers(string? roleID)
        {
            List<User> userList = new List<User>();
            if (!string.IsNullOrEmpty(roleID))
            {
                var userRoles = _context.UserRoles.Where(x => x.RoleId.Equals(roleID)).ToList();  // chỗ này cần toList không có sẽ bị lỗi
     
                if (userRoles != null && userRoles.Count() > 0)
                {
                    foreach(var userRole in userRoles)
                    {
                        try
                        {
                            var result = _dbSet.FirstOrDefault(x => x.Id.Equals(userRole.UserId));
                            if (result != null)
                                userList.Add(result);
                        }
                        catch(Exception ex)
                        {

                        }
                        
                    }
                }
                return userList.OrderByDescending(x=>x.RegisteredDate);
            }
            userList = await _context.Users.OrderByDescending(x => x.RegisteredDate).ToListAsync();
            return userList;
        }

        public async Task<UserRole> GetUserRole(string userId)
        {
            
            var result = await _context.UserRoles.Where(x => x.UserId.Equals(userId)).FirstOrDefaultAsync();
            if (result != null)
            {
                UserRole userRole = new UserRole();
                userRole.RoleID = result.RoleId;
                userRole.UserID = result.UserId;
                return userRole;
            }
            return null;
            
        }

    }
}
