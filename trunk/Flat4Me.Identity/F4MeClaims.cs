using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Flat4Me.Identity
{
    /// <summary>
    /// Flat 4 me Claims. Use only claim type. Claim value should be string.Empty always.
    /// </summary>
    public class F4MeClaims
    {
        public const string EmptyValue = "1";// Should have value. Because security checks for non empty
        public const string EmailConfirmed = "EmailConfirmed";
        public const string PhoneConfirmed = "PhoneConfirmed";
        public const string HotelierApproved = "HotelierApproved";
        public const string LAST_CONFIRM_SMS_DATETIME = "LAST_CONFIRM_SMS_DATETIME";

        public static Claim GetEmailConfirmed()
        {
            return new Claim(EmailConfirmed, EmptyValue);
        }

        public static Claim GetPhoneConfirmed()
        {
            return new Claim(PhoneConfirmed, EmptyValue);
        }

        public static Claim GetHotelierApproved()
        {
            return new Claim(HotelierApproved, EmptyValue);
        }

        public static Claim GetLAST_CONFIRM_SMS_DATETIME(string value)
        {
            return new Claim(LAST_CONFIRM_SMS_DATETIME, value);
        }

        public static bool Compare(Claim claim, Claim comparer)
        {
            return claim.Type == comparer.Type && claim.Value == comparer.Value;
        }
    }
}
