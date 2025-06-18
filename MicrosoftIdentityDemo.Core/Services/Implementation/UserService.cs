using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MicrosoftIdentityDemo.Core.Domain.IdentityEntites;
using MicrosoftIdentityDemo.Core.DTOs;
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
        private IUserStore<ApplicationUser> UserStore { get; set; }
        private IRoleStore<ApplicationRole> RoleStore { get; set; }

        public UserService(UserManager<ApplicationUser> userManager, IUserStore<ApplicationUser> userStore, IRoleStore<ApplicationRole> roleStore )
        {
            UserManager = userManager;
            UserStore = userStore;
            RoleStore = roleStore;
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
