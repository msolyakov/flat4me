using Flat4Me.Data.DTO.Auth;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flat4Me.Identity
{
    public class F4MeUserStore : UserStore<F4MeUser>
    {
        public static F4MeUserStore Create()
        {
            return new F4MeUserStore();
        }

        public Task AddPhoneNumber(int userId, string phone)
        {
            return UserRepository.AddPhoneNumber(userId, phone);
        }
        public Task RemovePhoneNumber(int userId, string phone)
        {
            return UserRepository.RemovePhoneNumber(userId, phone);
        }
        public Task SetPhoneNumberConfirmed(int userId, string phone, bool confirmed)
        {
            return UserRepository.SetPhoneNumberConfirmed(userId, phone, confirmed);
        }
        public Task<IEnumerable<UserPhoneDTO>> GetPhoneNumberList(int userId)
        {
            return UserRepository.GetPhoneNumberList(userId);
        }
    }
}
