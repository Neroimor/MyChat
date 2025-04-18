using Konscious.Security.Cryptography;
using System.Security.Cryptography;

namespace ChatApi.Services.RegisterServices.Realization
{
    public class Argon2PasswordHasher
    {

        public  string HashPassword(string password)
        {
            var salt = new byte[16];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }

       
            using (var argon2 = new Argon2i(salt))
            {
                argon2.DegreeOfParallelism = 8; 
                argon2.MemorySize = 65536; 
                argon2.Iterations = 4;

                var hashBytes = argon2.GetBytes(32); 

                return Convert.ToBase64String(salt) + "." + Convert.ToBase64String(hashBytes);
            }
        }

 
        public  bool VerifyPassword(string hashedPassword, string password)
        {

            var parts = hashedPassword.Split('.');
            var salt = Convert.FromBase64String(parts[0]);
            var storedHash = Convert.FromBase64String(parts[1]); 

            using (var argon2 = new Argon2i(salt))
            {
                argon2.DegreeOfParallelism = 8;
                argon2.MemorySize = 65536;
                argon2.Iterations = 4;

                var hashBytes = argon2.GetBytes(32);
                return CompareByteArrays(storedHash, hashBytes); 
            }
        }


        private  bool CompareByteArrays(byte[] array1, byte[] array2)
        {
            if (array1.Length != array2.Length) return false;

            for (int i = 0; i < array1.Length; i++)
            {
                if (array1[i] != array2[i])
                    return false;
            }

            return true;
        }
    }


}
