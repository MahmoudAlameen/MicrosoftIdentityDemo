using AM_Supplement.Contracts.ResultModel;
using MicrosoftIdentityDemo.Core.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicrosoftIdentityDemo.Core.Services.Implementation.Interfaces
{
    public interface IUserService
    {
        public Task<List<UserListDTO>> GetUsers();
        public Task<UserDetailsDTO?> GetUserDetails(string email);
        Task<ResultModel<bool>> AddUser(RegisterDTO model);
    }
}
