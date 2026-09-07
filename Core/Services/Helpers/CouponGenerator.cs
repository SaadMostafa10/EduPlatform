using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Services.Helpers
{
    public static class CouponGenerator
    {
        private static readonly char[] Characters = "23456789ABCDEFGHJKLMNPQRSTUVWXYZ".ToCharArray();

        public static string GenerateCode(int length = 9)
        {
            return RandomNumberGenerator.GetString(Characters, length);
        }
    }
}
