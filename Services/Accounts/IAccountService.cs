using Persistance.DTOs;
using Persistance.DTOs.Accounts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistance.Services.Accounts
{
    public interface IAccountService
    {
        Task<ServiceResultDTO<ApplicationUserDTO>> RegisterAsync(RegisterDTO registerDTO);
        Task<ServiceResultDTO<string>> LoginAsync(LoginDTO loginDTO);
    }
}
