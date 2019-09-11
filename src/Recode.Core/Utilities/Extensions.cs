using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;

namespace Recode.Core.Utilities
{
    public static class Extensions
    {
        public static T ToEnum<T>(this string value) where T : struct
        {
            if (Enum.TryParse(value, out T result))
            {
                return result;
            }
            else
            {
                return default(T);
            }
        }

        public static string ToHMAC_SHA256(this string toHash, string key)
        {
            byte[] keyByte = Encoding.Default.GetBytes(key);
            var sha256 = new HMACSHA256(keyByte);
            //sha256.Initialize();
            Byte[] originalByte = Encoding.Default.GetBytes(toHash);
            Byte[] encodedByte = sha256.ComputeHash(originalByte);
            sha256.Clear();

            string hash = BitConverter.ToString(encodedByte).Replace("-", "").ToLower();
            return hash;
        }

        public static bool IsValidEmail(this string email)
        {
            try
            {
                MailAddress m = new MailAddress(email);

                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }

    }
}
