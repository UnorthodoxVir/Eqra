using System;
using System.Linq;

namespace Eqra.Services
{
    public class CodeGeneratorService
    {
        public Random random = new Random();

        public string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public string GenerateOTP()
        {
            return new string(Enumerable.Repeat("123456789", 6).Select(s => s[random.Next(s.Length)]).ToArray());
        }

    }
}