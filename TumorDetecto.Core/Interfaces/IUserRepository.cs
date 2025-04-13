using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TumorDetector.Core.Entities;
using TumorDetector.Core.AuthModels;
using TumorDetector.Core.ModelsDtos.AuthenticationUserDtos;
namespace TumorDetector.Core.Interfaces
{
    public interface IUserRepository
    {
        
            Task<IEnumerable<ApplicationUser>> GetAllUsersRepository();

            Task<ApplicationUser> GetUserByIdRepository(string id);

            Task<ProcessResult> DeleteUserRepository(string id);

            Task<ApplicationUser> CreateNewUserRepository(RegisterModel user);

            Task<ApplicationUser> UpdateUserRepository(string id, RegisterModel user);
        
    }
}
