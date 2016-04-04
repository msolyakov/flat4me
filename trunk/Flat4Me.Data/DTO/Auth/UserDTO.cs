using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flat4Me.Data.DTO.Auth
{
    public class UserDTO
    {
        public int UserId
        {
            get;
            set;
        }

        public bool IsDeleted { get; set; }

        /// <summary>
        /// Email. Unique
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        ///     True if the email is confirmed, default is false
        /// </summary>
        public bool EmailConfirmed { get; set; }

        /// <summary>
        ///     PhoneNumber for the user
        /// </summary>
        public string PhoneNumber { get; set; }

        /// <summary>
        ///     True if the phone number is confirmed, default is false
        /// </summary>
        public bool PhoneNumberConfirmed { get; set; }

        /// <summary>
        ///     DateTime in UTC when lockout ends, any time in the past is considered not locked out.
        /// </summary>
        public DateTime? LockoutEndDateUtc { get; set; }

        /// <summary>
        ///     Is lockout enabled for this user
        /// </summary>
        public bool LockoutEnabled { get; set; }

        /// <summary>
        ///     Used to record failures for the purposes of lockout
        /// </summary>
        public int AccessFailedCount { get; set; }
        
        /// <summary>
        ///     A random value that should change whenever a users credentials have changed (password changed, login removed)
        /// </summary>
        public string SecurityStamp { get; set; }

        /// <summary>
        ///     The salted/hashed form of the user password
        /// </summary>
        public string PasswordHash { get; set; }

        /// <summary>
        /// User display name. Not unique
        /// </summary>
        public string FirstName { get; set; }
        public string LastName { get; set; }

        /// <summary>
        /// Full path to photo. Insert/Update/Check changes this property separatly
        /// </summary>
        public string PhotoSmallPath { get; set; }

        /// <summary>
        /// Full path to photo. Insert/Update/Check changes this property separatly
        /// </summary>
        public string PhotoTinyPath { get; set; }


        public bool HasChanged(UserDTO user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            return Email != user.Email
                || EmailConfirmed != user.EmailConfirmed
                || PhoneNumber != user.PhoneNumber
                || PhoneNumberConfirmed != user.PhoneNumberConfirmed
                || LockoutEndDateUtc != user.LockoutEndDateUtc
                || LockoutEnabled != user.LockoutEnabled
                || AccessFailedCount != user.AccessFailedCount
                || SecurityStamp != user.SecurityStamp
                || PasswordHash != user.PasswordHash
                || FirstName != user.FirstName
                || LastName != user.LastName;
        }        
    }
}
