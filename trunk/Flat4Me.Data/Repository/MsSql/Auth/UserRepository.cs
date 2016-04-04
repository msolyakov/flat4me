using Dapper;
using Flat4Me.Data.DTO.Auth;
using Flat4Me.Data.Repository.Interfaces.Auth;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flat4Me.Data.Repository.MsSql.Auth
{
    public class UserRepository<TUser> :
        BaseRepository,
        IUserRepository<TUser>
        where TUser : UserDTO
    {
        #region Stored procedures

        private async Task sp_Auth_User_Add(TUser user, SqlConnection conn, SqlTransaction tran = null)
        {
            var spName = "cmn.sp_Auth_User_Add";
            var p = new DynamicParameters();

            p.Add("@Email", user.Email);

            p.Add("@PhoneNumber", user.PhoneNumber);

            p.Add("@SecurityStamp", user.SecurityStamp);
            p.Add("@PasswordHash", user.PasswordHash);

            p.Add("@FirstName", user.FirstName);
            p.Add("@LastName", user.LastName);

            user.UserId = (await conn.QueryAsync<int>(
                                sql: spName, param: p, transaction: tran,
                                commandType: CommandType.StoredProcedure)).SingleOrDefault();
        }
        private Task sp_Auth_User_Update(TUser user, SqlConnection conn, SqlTransaction tran = null)
        {
            var spName = "cmn.sp_Auth_User_Update";
            var p = new DynamicParameters();

            p.Add("@UserId", user.UserId);

            p.Add("@Email", user.Email);
            p.Add("@EmailConfirmed", user.EmailConfirmed);

            p.Add("@PhoneNumber", user.PhoneNumber);
            p.Add("@PhoneNumberConfirmed", user.PhoneNumberConfirmed);

            p.Add("@LockoutEndDateUtc", user.LockoutEndDateUtc);
            p.Add("@LockoutEnabled", user.LockoutEnabled);
            p.Add("@AccessFailedCount", user.AccessFailedCount);

            p.Add("@SecurityStamp", user.SecurityStamp);
            p.Add("@PasswordHash", user.PasswordHash);

            p.Add("@FirstName", user.FirstName);
            p.Add("@LastName", user.LastName);

            return conn.ExecuteAsync(
                sql: spName, param: p, transaction: tran,
                commandType: CommandType.StoredProcedure);
        }
        private Task sp_Auth_User_Delete(TUser user, SqlConnection conn, SqlTransaction tran = null)
        {
            var spName = "cmn.sp_Auth_User_Delete";
            var p = new DynamicParameters();
            p.Add("@UserId", user.UserId);
            p.Add("@IsDeleted", user.IsDeleted);

            return conn.ExecuteAsync(
                sql: spName, param: p, transaction: tran,
                commandType: CommandType.StoredProcedure);
        }


        private async Task<TUser> sp_Auth_User_GetByEmail(string email, SqlConnection conn, SqlTransaction tran = null)
        {
            var spName = "cmn.sp_Auth_User_GetByEmail";
            var p = new DynamicParameters();
            p.Add("@Email", email);

            return (await conn.QueryAsync<TUser>(
                sql: spName, param: p, transaction: tran,
                commandType: CommandType.StoredProcedure)).SingleOrDefault();
        }
        private async Task<TUser> sp_Auth_User_GetByUserId(int id, SqlConnection conn, SqlTransaction tran = null)
        {
            var spName = "cmn.sp_Auth_User_GetByUserId";
            var p = new DynamicParameters();
            p.Add("@UserId", id);

            return (await conn.QueryAsync<TUser>(
                sql: spName, param: p, transaction: tran,
                commandType: CommandType.StoredProcedure)).SingleOrDefault();
        }
        private async Task<TUser> sp_Auth_User_GetByLogin(int providerId, string providerKey, SqlConnection conn, SqlTransaction tran = null)
        {
            var spName = "cmn.sp_Auth_User_GetByLogin";
            var p = new DynamicParameters();
            p.Add("@ProviderId", providerId);
            p.Add("@ProviderKey", providerKey);

            return (await conn.QueryAsync<TUser>(
                sql: spName, param: p, transaction: tran,
                commandType: CommandType.StoredProcedure)).SingleOrDefault();
        }


        private async Task<int> sp_Auth_Provider_Add(string providerName, SqlConnection conn, SqlTransaction tran = null)
        {
            var spName = "cmn.sp_Auth_Provider_Add";
            var p = new DynamicParameters();
            p.Add("@Name", providerName);

            return (await conn.QueryAsync<int>(
                sql: spName, param: p, transaction: tran,
                commandType: CommandType.StoredProcedure)).SingleOrDefault();
        }
        private Task<int?> sp_Auth_Provider_Get(string providerName, SqlConnection conn, SqlTransaction tran = null)
        {
            var spName = "cmn.sp_Auth_Provider_Get";
            var p = new DynamicParameters();
            p.Add("@Name", providerName);

            return conn.ExecuteScalarAsync<int?>(
                sql: spName, param: p, transaction: tran,
                commandType: CommandType.StoredProcedure);
        }


        private Task sp_Auth_UserLogin_Add(int providerId, TUser user, string providerKey, SqlConnection conn, SqlTransaction tran = null)
        {
            var spName = "cmn.sp_Auth_UserLogin_Add";
            var p = new DynamicParameters();
            p.Add("@UserId", user.UserId);
            p.Add("@ProviderId", providerId);
            p.Add("@ProviderKey", providerKey);

            return conn.ExecuteAsync(
                sql: spName, param: p, transaction: tran,
                commandType: CommandType.StoredProcedure);
        }
        private Task<IEnumerable<UserLoginDTO>> sp_Auth_UserLogin_GetList(TUser user, SqlConnection conn, SqlTransaction tran = null)
        {
            var spName = "cmn.sp_Auth_UserLogin_GetList";
            var p = new DynamicParameters();
            p.Add("@UserId", user.UserId);

            return conn.QueryAsync<UserLoginDTO>(
                sql: spName, param: p, transaction: tran,
                commandType: CommandType.StoredProcedure);
        }
        private Task sp_Auth_UserLogin_Remove(int providerId, TUser user, string providerKey, SqlConnection conn, SqlTransaction tran = null)
        {
            var spName = "cmn.sp_Auth_UserLogin_Remove";
            var p = new DynamicParameters();
            p.Add("@UserId", user.UserId);
            p.Add("@ProviderId", providerId);
            p.Add("@ProviderKey", providerKey);

            return conn.ExecuteAsync(
                sql: spName, param: p, transaction: tran,
                commandType: CommandType.StoredProcedure);
        }


        private Task sp_Auth_UserClaim_Add(TUser user, UserClaimDTO claim, SqlConnection conn, SqlTransaction tran = null)
        {
            var spName = "cmn.sp_Auth_UserClaim_Add";
            var p = new DynamicParameters();
            p.Add("@UserId", user.UserId);
            p.Add("@Type", claim.Type);
            p.Add("@Value", claim.Value);

            return conn.ExecuteAsync(
                sql: spName, param: p, transaction: tran,
                commandType: CommandType.StoredProcedure);
        }
        private Task sp_Auth_UserClaim_Remove(TUser user, UserClaimDTO claim, SqlConnection conn, SqlTransaction tran = null)
        {
            var spName = "cmn.sp_Auth_UserClaim_Remove";
            var p = new DynamicParameters();
            p.Add("@UserId", user.UserId);
            p.Add("@Type", claim.Type);
            p.Add("@Value", claim.Value);

            return conn.ExecuteAsync(
                sql: spName, param: p, transaction: tran,
                commandType: CommandType.StoredProcedure);
        }
        private Task<IEnumerable<UserClaimDTO>> sp_Auth_UserClaim_GetList(TUser user, SqlConnection conn, SqlTransaction tran = null)
        {
            var spName = "cmn.sp_Auth_UserClaim_GetList";
            var p = new DynamicParameters();
            p.Add("@UserId", user.UserId);

            return conn.QueryAsync<UserClaimDTO>(
                sql: spName, param: p, transaction: tran,
                commandType: CommandType.StoredProcedure);
        }


        private async Task<int?> sp_Auth_Role_Get(string name, SqlConnection conn, SqlTransaction tran = null)
        {
            var spName = "cmn.sp_Auth_Role_Get";
            var p = new DynamicParameters();
            p.Add("@Name", name);

            return (await conn.QueryAsync<int?>(
                sql: spName, param: p, transaction: tran,
                commandType: CommandType.StoredProcedure)).SingleOrDefault();
        }
        private Task sp_Auth_UserRole_Add(TUser user, int roleId, SqlConnection conn, SqlTransaction tran = null)
        {
            var spName = "cmn.sp_Auth_UserRole_Add";
            var p = new DynamicParameters();
            p.Add("@UserId", user.UserId);
            p.Add("@RoleId", roleId);

            return conn.ExecuteAsync(
                sql: spName, param: p, transaction: tran,
                commandType: CommandType.StoredProcedure);
        }
        private Task sp_Auth_UserRole_Remove(TUser user, int roleId, SqlConnection conn, SqlTransaction tran = null)
        {
            var spName = "cmn.sp_Auth_UserRole_Remove";
            var p = new DynamicParameters();
            p.Add("@UserId", user.UserId);
            p.Add("@RoleId", roleId);

            return conn.ExecuteAsync(
                sql: spName, param: p, transaction: tran,
                commandType: CommandType.StoredProcedure);
        }
        private Task<IEnumerable<string>> sp_Auth_UserRole_GetList(TUser user, SqlConnection conn, SqlTransaction tran = null)
        {
            var spName = "cmn.sp_Auth_UserRole_GetList";
            var p = new DynamicParameters();
            p.Add("@UserId", user.UserId);

            return conn.QueryAsync<string>(
                sql: spName, param: p, transaction: tran,
                commandType: CommandType.StoredProcedure);
        }


        private Task sp_Auth_UserPhone_Add(int userId, string phone, SqlConnection conn, SqlTransaction tran = null)
        {
            var spName = "cmn.sp_Auth_UserPhone_Add";
            var p = new DynamicParameters();
            p.Add("@UserId", userId);
            p.Add("@PhoneNumber", phone);

            return conn.ExecuteAsync(
                sql: spName, param: p, transaction: tran,
                commandType: CommandType.StoredProcedure);
        }
        private Task sp_Auth_UserPhone_Remove(int userId, UserPhoneDTO phone, SqlConnection conn, SqlTransaction tran = null)
        {
            var spName = "cmn.sp_Auth_UserPhone_Remove";
            var p = new DynamicParameters();
            p.Add("@UserId", userId);
            p.Add("@UserPhoneId", phone.UserPhoneId);

            return conn.ExecuteAsync(
                sql: spName, param: p, transaction: tran,
                commandType: CommandType.StoredProcedure);
        }
        private Task sp_Auth_UserPhone_Confirm(int userId, UserPhoneDTO phone, SqlConnection conn, SqlTransaction tran = null)
        {
            var spName = "cmn.sp_Auth_UserPhone_Confirm";
            var p = new DynamicParameters();
            p.Add("@UserId", userId);
            p.Add("@UserPhoneId", phone.UserPhoneId);
            p.Add("@PhoneNumberConfirmed", phone.PhoneNumberConfirmed);

            return conn.ExecuteAsync(
                sql: spName, param: p, transaction: tran,
                commandType: CommandType.StoredProcedure);
        }
        private async Task<UserPhoneDTO> sp_Auth_UserPhone_Get(int userId, string phone, SqlConnection conn, SqlTransaction tran = null)
        {
            var spName = "cmn.sp_Auth_UserPhone_Get";
            var p = new DynamicParameters();
            p.Add("@UserId", userId);
            p.Add("@PhoneNumber", phone);

            return (await conn.QueryAsync<UserPhoneDTO>(
                sql: spName, param: p, transaction: tran,
                commandType: CommandType.StoredProcedure)).SingleOrDefault();
        }
        private Task<IEnumerable<UserPhoneDTO>> sp_Auth_UserPhone_GetList(int userId, SqlConnection conn, SqlTransaction tran = null)
        {
            var spName = "cmn.sp_Auth_UserPhone_GetList";
            var p = new DynamicParameters();
            p.Add("@UserId", userId);

            return conn.QueryAsync<UserPhoneDTO>(
                sql: spName, param: p, transaction: tran,
                commandType: CommandType.StoredProcedure);
        }


        private Task sp_Auth_User_SetPhoto(TUser user, SqlConnection conn, SqlTransaction tran = null)
        {
            var spName = "cmn.sp_Auth_User_SetPhoto";
            var p = new DynamicParameters();
            p.Add("@UserId", user.UserId);
            p.Add("@PhotoSmallPath", user.PhotoSmallPath);
            p.Add("@PhotoTinyPath", user.PhotoTinyPath);

            return conn.ExecuteAsync(
                sql: spName, param: p, transaction: tran,
                commandType: CommandType.StoredProcedure);
        }

        #endregion        

        public async Task Add(TUser user)
        {
            using (var conn = new SqlConnection(base.ConnectionString))
            {
                await conn.OpenAsync();
                await sp_Auth_User_Add(user, conn);
            }
        }

        public async Task Update(TUser user)
        {
            using (var conn = new SqlConnection(base.ConnectionString))
            {
                await conn.OpenAsync();
                using (var tran = conn.BeginTransaction())
                {
                    try
                    {
                        var dbUser = await sp_Auth_User_GetByUserId(user.UserId, conn, tran);

                        if (user.HasChanged(dbUser))
                        {
                            await sp_Auth_User_Update(user, conn, tran);
                        }

                        tran.Commit();
                    }
                    catch
                    {
                        tran.RollbackSafe();
                        throw;
                    }
                }
            }
        }

        public async Task Delete(TUser user)
        {
            using (var conn = new SqlConnection(base.ConnectionString))
            {
                await conn.OpenAsync();
                await sp_Auth_User_Delete(user, conn);
            }
        }

        public async Task<TUser> FindById(int userId)
        {
            using (var conn = new SqlConnection(base.ConnectionString))
            {
                await conn.OpenAsync();
                return await sp_Auth_User_GetByUserId(userId, conn);
            }
        }


        public async Task AddClaim(TUser user, UserClaimDTO claim)
        {
            using (var conn = new SqlConnection(base.ConnectionString))
            {
                await conn.OpenAsync();
                await sp_Auth_UserClaim_Add(user, claim, conn);
            }
        }

        public async Task<IEnumerable<UserClaimDTO>> GetClaimList(TUser user)
        {
            using (var conn = new SqlConnection(base.ConnectionString))
            {
                await conn.OpenAsync();

                return await sp_Auth_UserClaim_GetList(user, conn);
            }
        }

        public async Task RemoveClaim(TUser user, UserClaimDTO claim)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            if (claim == null)
                throw new ArgumentNullException("claim");

            using (var conn = new SqlConnection(base.ConnectionString))
            {
                await conn.OpenAsync();
                await sp_Auth_UserClaim_Remove(user, claim, conn);
            }
        }



        public async Task AddLogin(TUser user, UserLoginDTO login)
        {
            using (var conn = new SqlConnection(base.ConnectionString))
            {
                await conn.OpenAsync();
                using (var tran = conn.BeginTransaction())
                {
                    try
                    {
                        var providerId = await sp_Auth_Provider_Get(login.ProviderName, conn, tran);
                        if (providerId == null)
                        {
                            providerId = await sp_Auth_Provider_Add(login.ProviderName, conn, tran);
                        }

                        await sp_Auth_UserLogin_Add(providerId.Value, user, login.ProviderKey, conn, tran);
                        tran.Commit();
                    }
                    catch
                    {
                        tran.RollbackSafe();
                        throw;
                    }
                }
            }
        }

        public async Task<TUser> Find(UserLoginDTO login)
        {
            using (var conn = new SqlConnection(base.ConnectionString))
            {
                await conn.OpenAsync();

                var providerId = await sp_Auth_Provider_Get(login.ProviderName, conn);

                if (providerId == null)
                {
                    return await Task.FromResult<TUser>(null);
                }

                return await sp_Auth_User_GetByLogin(providerId.Value, login.ProviderKey, conn);
            }
        }

        public async Task<IEnumerable<UserLoginDTO>> GetLoginList(TUser user)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            using (var conn = new SqlConnection(base.ConnectionString))
            {
                await conn.OpenAsync();

                return await sp_Auth_UserLogin_GetList(user, conn);
            }
        }

        public async Task RemoveLogin(TUser user, UserLoginDTO login)
        {
            using (var conn = new SqlConnection(base.ConnectionString))
            {
                await conn.OpenAsync();

                var providerId = await sp_Auth_Provider_Get(login.ProviderName, conn);

                if (providerId != null)
                {
                    await sp_Auth_UserLogin_Remove(providerId.Value, user, login.ProviderKey, conn);
                }
            }
        }


        public async Task AddToRole(TUser user, string roleName)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            if (string.IsNullOrWhiteSpace(roleName))
                throw new ArgumentNullException("roleName");

            using (var conn = new SqlConnection(base.ConnectionString))
            {
                var roleId = await sp_Auth_Role_Get(roleName, conn);
                if (roleId == null)
                {
                    throw new InvalidOperationException(string.Format("Role not found. RoleName is {0}", roleName));
                }

                await sp_Auth_UserRole_Add(user, roleId.Value, conn);
            }
        }

        public async Task<IEnumerable<string>> GetRoleList(TUser user)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            using (var conn = new SqlConnection(base.ConnectionString))
            {
                await conn.OpenAsync();

                return await sp_Auth_UserRole_GetList(user, conn);
            }
        }

        public async Task<bool> IsInRole(TUser user, string roleName)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            if (string.IsNullOrEmpty(roleName))
                throw new ArgumentNullException("role");

            using (var conn = new SqlConnection(base.ConnectionString))
            {
                await conn.OpenAsync();

                var roles = await sp_Auth_UserRole_GetList(user, conn);

                var isIn = roles.Any(p => p.ToUpper() == roleName.ToUpper());

                return await Task.FromResult<bool>(isIn);
            }
        }

        public async Task RemoveFromRole(TUser user, string roleName)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            if (string.IsNullOrWhiteSpace(roleName))
                throw new ArgumentNullException("roleName");

            using (var conn = new SqlConnection(base.ConnectionString))
            {
                var roleId = await sp_Auth_Role_Get(roleName, conn);
                if (roleId != null)
                {
                    await sp_Auth_UserRole_Remove(user, roleId.Value, conn);
                }
            }
        }


        public async Task<TUser> FindByEmail(string email)
        {
            using (var conn = new SqlConnection(base.ConnectionString))
            {
                await conn.OpenAsync();
                return await sp_Auth_User_GetByEmail(email, conn);
            }
        }


        public async Task AddPhoneNumber(int userId, string phone)
        {
            using (var conn = new SqlConnection(base.ConnectionString))
            {
                await conn.OpenAsync();
                await sp_Auth_UserPhone_Add(userId, phone, conn);
            }
        }

        public async Task RemovePhoneNumber(int userId, string phone)
        {
            using (var conn = new SqlConnection(base.ConnectionString))
            {
                await conn.OpenAsync();

                var userPhone = await sp_Auth_UserPhone_Get(userId, phone, conn);

                if (userPhone != null)
                {
                    await sp_Auth_UserPhone_Remove(userId, userPhone, conn);
                }
            }
        }

        public async Task SetPhoneNumberConfirmed(int userId, string phone, bool confirmed)
        {
            using (var conn = new SqlConnection(base.ConnectionString))
            {
                await conn.OpenAsync();

                var userPhone = await sp_Auth_UserPhone_Get(userId, phone, conn);

                if (userPhone != null)
                {
                    userPhone.PhoneNumberConfirmed = confirmed;
                    await sp_Auth_UserPhone_Confirm(userId, userPhone, conn);
                }
            }
        }

        public async Task<IEnumerable<UserPhoneDTO>> GetPhoneNumberList(int userId)
        {
            using (var conn = new SqlConnection(base.ConnectionString))
            {
                await conn.OpenAsync();

                return await sp_Auth_UserPhone_GetList(userId, conn);
            }
        }


        public async Task SetPhoto(TUser user)
        {
            using (var conn = new SqlConnection(base.ConnectionString))
            {
                await conn.OpenAsync();
                await sp_Auth_User_SetPhoto(user, conn);
            }
        }
    }
}
