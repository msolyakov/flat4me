using System;
using System.Net;
using System.Security.Cryptography;
using System.Text;

namespace Flat4Me.Identity.Identity
{
    internal static class Rfc6238AuthenticationService
    {
        private readonly static DateTime _unixEpoch;

        private readonly static TimeSpan _timestep;

        private readonly static Encoding _encoding;

        static Rfc6238AuthenticationService()
        {
            Rfc6238AuthenticationService._unixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            Rfc6238AuthenticationService._timestep = TimeSpan.FromMinutes(3);
            Rfc6238AuthenticationService._encoding = new UTF8Encoding(false, true);
        }

        private static byte[] ApplyModifier(byte[] input, string modifier)
        {
            if (string.IsNullOrEmpty(modifier))
            {
                return input;
            }
            byte[] bytes = Rfc6238AuthenticationService._encoding.GetBytes(modifier);
            byte[] numArray = new byte[checked((int)input.Length + (int)bytes.Length)];
            Buffer.BlockCopy(input, 0, numArray, 0, (int)input.Length);
            Buffer.BlockCopy(bytes, 0, numArray, (int)input.Length, (int)bytes.Length);
            return numArray;
        }

        private static int ComputeTotp(HashAlgorithm hashAlgorithm, ulong timestepNumber, string modifier)
        {
            byte[] bytes = BitConverter.GetBytes(IPAddress.HostToNetworkOrder((long)timestepNumber));
            byte[] numArray = hashAlgorithm.ComputeHash(Rfc6238AuthenticationService.ApplyModifier(bytes, modifier));
            int num = numArray[(int)numArray.Length - 1] & 15;
            int num1 = (numArray[num] & 127) << 24 | (numArray[num + 1] & 255) << 16 | (numArray[num + 2] & 255) << 8 | numArray[num + 3] & 255;
            return num1 % 1000000;
        }

        public static int GenerateCode(SecurityToken securityToken, string modifier = null)
        {
            int num;
            if (securityToken == null)
            {
                throw new ArgumentNullException("securityToken");
            }
            ulong currentTimeStepNumber = Rfc6238AuthenticationService.GetCurrentTimeStepNumber();
            using (HMACSHA1 hMACSHA1 = new HMACSHA1(securityToken.GetDataNoClone()))
            {
                num = Rfc6238AuthenticationService.ComputeTotp(hMACSHA1, currentTimeStepNumber, modifier);
            }
            return num;
        }

        private static ulong GetCurrentTimeStepNumber()
        {
            TimeSpan utcNow = DateTime.UtcNow - Rfc6238AuthenticationService._unixEpoch;
            return (ulong)(utcNow.Ticks / Rfc6238AuthenticationService._timestep.Ticks);
        }

        public static bool ValidateCode(SecurityToken securityToken, int code, string modifier = null)
        {
            bool flag;
            if (securityToken == null)
            {
                throw new ArgumentNullException("securityToken");
            }
            ulong currentTimeStepNumber = Rfc6238AuthenticationService.GetCurrentTimeStepNumber();
            using (HMACSHA1 hMACSHA1 = new HMACSHA1(securityToken.GetDataNoClone()))
            {
                int num = -2;
                while (num <= 2)
                {
                    if (Rfc6238AuthenticationService.ComputeTotp(hMACSHA1, currentTimeStepNumber + (ulong)num, modifier) != code)
                    {
                        num++;
                    }
                    else
                    {
                        flag = true;
                        return flag;
                    }
                }
                return false;
            }
        }
    }
}
