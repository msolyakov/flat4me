using Flat4Me.Data.DTO.Auth;
using Flat4Me.Data.Repository.Interfaces.Auth;
using Flat4Me.Data.Repository.MsSql.Auth;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Flat4Me.Identity
{
    /// <summary>
    /// ASP.NET Identity UserStore Dapper Implementation
    /// </summary>
    /// <typeparam name="TUser"></typeparam>
    public class UserStore<TUser> :
        IUserStore<TUser, int>,
        IUserClaimStore<TUser, int>,
        IUserLoginStore<TUser, int>,
        IUserRoleStore<TUser, int>,
        IUserPasswordStore<TUser, int>,
        IUserSecurityStampStore<TUser, int>,
        IUserPhoneNumberStore<TUser, int>,
        IUserEmailStore<TUser, int>,
        IUserLockoutStore<TUser, int>,
        IUserTwoFactorStore<TUser, int>
        where TUser : User
    {
        protected IUserRepository<TUser> UserRepository { get; set; }

        public UserStore()
        {
            UserRepository = new UserRepository<TUser>();
        }
        
        #region IUserStore

        public Task CreateAsync(TUser user)
        {
            return UserRepository.Add(user);
        }

        public Task DeleteAsync(TUser user)
        {
            user.IsDeleted = true;

            return UserRepository.Delete(user);
        }

        public Task<TUser> FindByIdAsync(int userId)
        {
            return UserRepository.FindById(userId);
        }

        /// <summary>
        /// ASP.NET Identity use UserName as Login. We user Email as Login.
        /// </summary>
        /// <param name="userName">EMAIL</param>
        /// <returns></returns>
        public Task<TUser> FindByNameAsync(string userName)
        {
            return FindByEmailAsync(userName);
        }

        public Task UpdateAsync(TUser user)
        {
            return UserRepository.Update(user);
        }

        public void Dispose()
        {

        }

        #endregion

        #region ITUserClaimStore

        public Task AddClaimAsync(TUser user, Claim claim)
        {
            return UserRepository.AddClaim(user, new UserClaimDTO(claim.Type, claim.Value));
        }

        public async Task<IList<Claim>> GetClaimsAsync(TUser user)
        {
            var db = await UserRepository.GetClaimList(user);

            var list = db.Select(p => new Claim(p.Type, p.Value)).ToList();

            return await Task.FromResult<IList<Claim>>(list);
        }

        public Task RemoveClaimAsync(TUser user, Claim claim)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            if (claim == null)
                throw new ArgumentNullException("claim");

            return UserRepository.RemoveClaim(user, new UserClaimDTO(claim.Type, claim.Value));
        }

        #endregion

        #region ITUserLoginStore

        public Task AddLoginAsync(TUser user, Microsoft.AspNet.Identity.UserLoginInfo login)
        {
            return UserRepository.AddLogin(user, new UserLoginDTO(login.LoginProvider, login.ProviderKey));
        }

        public Task<TUser> FindAsync(Microsoft.AspNet.Identity.UserLoginInfo login)
        {
            return UserRepository.Find(new UserLoginDTO(login.LoginProvider, login.ProviderKey));
        }

        public async Task<IList<Microsoft.AspNet.Identity.UserLoginInfo>> GetLoginsAsync(TUser user)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            var db =await UserRepository.GetLoginList(user);

            var list = db
                .Select(p => new Microsoft.AspNet.Identity.UserLoginInfo(p.ProviderName, p.ProviderKey))
                .ToList();

            return await Task.FromResult<IList<Microsoft.AspNet.Identity.UserLoginInfo>>(list);
        }

        public  Task RemoveLoginAsync(TUser user, Microsoft.AspNet.Identity.UserLoginInfo login)
        {
            return UserRepository.RemoveLogin(user, new UserLoginDTO(login.LoginProvider, login.ProviderKey));
        }

        #endregion

        #region ITUserRoleStore

        public Task AddToRoleAsync(TUser user, string roleName)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            if (string.IsNullOrWhiteSpace(roleName))
                throw new ArgumentNullException("roleName");

            return UserRepository.AddToRole(user, roleName);
        }

        public async Task<IList<string>> GetRolesAsync(TUser user)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            var roles = await UserRepository.GetRoleList(user);

            return await Task.FromResult<IList<string>>(roles.ToList());
        }

        public Task<bool> IsInRoleAsync(TUser user, string roleName)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            if (string.IsNullOrEmpty(roleName))
                throw new ArgumentNullException("role");

            return UserRepository.IsInRole(user, roleName);
        }

        public Task RemoveFromRoleAsync(TUser user, string roleName)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            if (string.IsNullOrWhiteSpace(roleName))
                throw new ArgumentNullException("roleName");

            return UserRepository.RemoveFromRole(user, roleName);
        }

        #endregion

        #region ITUserPasswordStore

        public Task<string> GetPasswordHashAsync(TUser user)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            return Task.FromResult<string>(user.PasswordHash);
        }

        public Task<bool> HasPasswordAsync(TUser user)
        {
            return Task.FromResult<bool>(user.PasswordHash != null);
        }

        public Task SetPasswordHashAsync(TUser user, string passwordHash)
        {
            user.PasswordHash = passwordHash;

            return Task.FromResult<object>(null);
        }

        #endregion

        #region ITUserSecurityStampStore

        public Task<string> GetSecurityStampAsync(TUser user)
        {
            return Task.FromResult(user.SecurityStamp);
        }

        public Task SetSecurityStampAsync(TUser user, string stamp)
        {
            user.SecurityStamp = stamp;
            return Task.FromResult(0);
        }

        #endregion

        #region ITUserPhoneNumberStore

        public Task<string> GetPhoneNumberAsync(TUser user)
        {
            return Task.FromResult(user.PhoneNumber);
        }

        public Task<bool> GetPhoneNumberConfirmedAsync(TUser user)
        {
            return Task.FromResult(user.PhoneNumberConfirmed);
        }

        public Task SetPhoneNumberAsync(TUser user, string phoneNumber)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            user.PhoneNumber = phoneNumber;
            return Task.FromResult<object>(null);
        }

        public Task SetPhoneNumberConfirmedAsync(TUser user, bool confirmed)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            user.PhoneNumberConfirmed = confirmed;
            return Task.FromResult<object>(null);
        }

        #endregion

        #region ITUserEmailStore

        public Task<TUser> FindByEmailAsync(string email)
        {
            return UserRepository.FindByEmail(email);
        }

        public Task<string> GetEmailAsync(TUser user)
        {
            return Task.FromResult(user.Email);
        }

        public Task<bool> GetEmailConfirmedAsync(TUser user)
        {
            return Task.FromResult(user.EmailConfirmed);
        }

        public Task SetEmailAsync(TUser user, string email)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            user.Email = email;
            return Task.FromResult<object>(null);
        }

        public Task SetEmailConfirmedAsync(TUser user, bool confirmed)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            user.EmailConfirmed = confirmed;
            return Task.FromResult<object>(null);
        }

        #endregion

        #region IUserLockoutStore

        public Task<bool> GetLockoutEnabledAsync(TUser user)
        {
            return Task.FromResult(user.LockoutEnabled);
        }

        public Task SetLockoutEnabledAsync(TUser user, bool enabled)
        {
            user.LockoutEnabled = enabled;

            return Task.FromResult<object>(null);
        }


        public Task<DateTimeOffset> GetLockoutEndDateAsync(TUser user)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            DateTimeOffset dateTimeOffset;

            if (user.LockoutEndDateUtc.HasValue)
            {
                DateTime? lockoutEndDateUtc = user.LockoutEndDateUtc;
                dateTimeOffset = new DateTimeOffset(DateTime.SpecifyKind(lockoutEndDateUtc.Value, DateTimeKind.Utc));
            }
            else
            {
                dateTimeOffset = new DateTimeOffset();
            }
            return Task.FromResult<DateTimeOffset>(dateTimeOffset);
        }

        public Task SetLockoutEndDateAsync(TUser user, DateTimeOffset lockoutEnd)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            DateTime? nullable;

            if (lockoutEnd == DateTimeOffset.MinValue)
            {
                nullable = null;
            }
            else
            {
                nullable = new DateTime?(lockoutEnd.UtcDateTime);
            }

            user.LockoutEndDateUtc = nullable;

            return Task.FromResult<object>(null);
        }


        public Task<int> GetAccessFailedCountAsync(TUser user)
        {
            return Task.FromResult(user.AccessFailedCount);
        }

        public Task<int> IncrementAccessFailedCountAsync(TUser user)
        {
            return Task.FromResult(++user.AccessFailedCount);
        }

        public Task ResetAccessFailedCountAsync(TUser user)
        {
            user.AccessFailedCount = 0;

            return Task.FromResult<object>(null);
        }

        #endregion

        #region IUserTwoFactorStore

        public Task<bool> GetTwoFactorEnabledAsync(TUser user)
        {
            return Task.FromResult<bool>(false);
        }

        public Task SetTwoFactorEnabledAsync(TUser user, bool enabled)
        {
            return Task.FromResult<object>(null);
        }

        #endregion
    }
}
