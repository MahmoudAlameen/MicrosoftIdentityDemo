using AM_Supplement.Contracts.ResultModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MicrosoftIdentityDemo.Core.Domain.IdentityEntites;
using MicrosoftIdentityDemo.Core.DTOs;
using MicrosoftIdentityDemo.Core.Enums;
using MicrosoftIdentityDemo.Core.Services.Implementation.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicrosoftIdentityDemo.Core.Services.Implementation
{
    public class UserService : IUserService
    {
        UserManager<ApplicationUser> UserManager { get; set; }
        RoleManager<ApplicationRole> RoleManager { get; set; }

        private IUserStore<ApplicationUser> UserStore { get; set; }
        private IRoleStore<ApplicationRole> RoleStore { get; set; }

        public UserService(UserManager<ApplicationUser> userManager, IUserStore<ApplicationUser> userStore, IRoleStore<ApplicationRole> roleStore, RoleManager<ApplicationRole> roleManager)
        {
            UserManager = userManager;
            UserStore = userStore;
            RoleStore = roleStore;
            RoleManager = roleManager;
        }
        public async Task<ResultModel<bool>> AddUser(RegisterDTO model)
        {
            ApplicationUser user = new ApplicationUser
            {
                FullName = model.FullName,
                Email = model.Email,
                UserName = model.Email,
                FullNameAR = model.FullNameAR,
                PhoneNumber = model.PhoneNumber,
            };
            var result = await UserManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
            {
                return new ResultModel<bool>
                {
                    IsValid = false,
                    ErrorMessages = result.Errors.Select(e => e.Description).ToList()
                };
            }

            if (!await RoleManager.Roles.AnyAsync(r => r.Name == model.Role.ToString()))
            {
                var creationRoleResult = await RoleManager.CreateAsync(new ApplicationRole { Name = model.Role.ToString()});
                if (!creationRoleResult.Succeeded)
                {
                    return new ResultModel<bool>
                    {
                        IsValid = false,
                        ErrorMessages = creationRoleResult.Errors.Select(e => e.Description).ToList()
                    };
                }

            }
            var addToRoleResult = await UserManager.AddToRolesAsync(user, new List<string> { model.Role.ToString() });
            if (!addToRoleResult.Succeeded)
            {
                return new ResultModel<bool>
                {
                    IsValid = false,
                    ErrorMessages = addToRoleResult.Errors.Select(e => e.Description).ToList()
                };
            };

            return new ResultModel<bool>()
            {
                IsValid = true,
                Model = true
            };
        }

        public async Task<List<UserListDTO>> GetUsers()
        {
            return await UserManager.Users.Select(u => new UserListDTO
            {
                Id = u.Id,
                FullName = u.FullName,
                Email = u.Email,
                PhoneNumber  = u.PhoneNumber,
                NameAr = u.FullNameAR
            }).ToListAsync();
        }

        public async Task<UserDetailsDTO?> GetUserDetails(string email)
        {
            var user = await UserManager.FindByEmailAsync(email);
            if (user == null)
                return null;

            var roles = await UserManager.GetRolesAsync(user);

            return new UserDetailsDTO
            {
                Id = user.Id,
                FullName = user.FullName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                FullNameAr = user.FullNameAR,
                Roles = roles.ToList()
            };
        }
    }
}
