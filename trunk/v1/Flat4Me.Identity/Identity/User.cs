using Flat4Me.Data.DTO.Auth;
using Microsoft.AspNet.Identity;
using System;

namespace Flat4Me.Identity
{
    /// <summary>
    /// ASP.NET Identity User
    /// </summary>
    public class User : UserDTO, IUser<int>
    {
        public int Id
        {
            get { return this.UserId; }
        }

        /// <summary>
        ///  This is equals to EMAIL
        /// ASP.NET Identity use UserName as Login. We user Email as Login.
        /// </summary>
        public string UserName
        {
            get
            {
                return this.Email;
            }
            set
            {
                this.Email = value;
            }
        }       
    }
}
