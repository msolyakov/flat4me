using Flat4me.Core.Data.Objects.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flat4me.Core.Data.Auth
{
    public interface IUserRepository<TUser>
        where TUser : UserDTO
    {
        Task Add(TUser user);

        Task Update(TUser user);

        Task Delete(TUser user);

        Task<TUser> FindById(int userId);

        Task AddClaim(TUser user, UserClaimDTO claim);

        Task<IEnumerable<UserClaimDTO>> GetClaimList(TUser user);

        Task RemoveClaim(TUser user, UserClaimDTO claim);


        Task AddLogin(TUser user, UserLoginDTO login);

        Task<TUser> Find(UserLoginDTO login);

        Task<IEnumerable<UserLoginDTO>> GetLoginList(TUser user);

        Task RemoveLogin(TUser user, UserLoginDTO login);


        Task AddToRole(TUser user, string roleName);

        Task<IEnumerable<string>> GetRoleList(TUser user);

        Task<bool> IsInRole(TUser user, string roleName);

        Task RemoveFromRole(TUser user, string roleName);


        Task<TUser> FindByEmail(string email);


        Task AddPhoneNumber(int userId, string phone);

        Task RemovePhoneNumber(int userId, string phone);

        Task SetPhoneNumberConfirmed(int userId, string phone, bool confirmed);

        Task<IEnumerable<UserPhoneDTO>> GetPhoneNumberList(int userId);


        Task SetPhoto(TUser user);
    }
}
