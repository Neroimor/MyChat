using Konscious.Security.Cryptography;
using System.Security.Cryptography;
using System.Text;

namespace ChatApi.Services.RegisterServices.Realization
{
    public class Argon2PasswordHasher
    {
        public string HashPassword(string password)
        {
            // 1) Генерируем случайную соль
            var salt = new byte[16];
            using (var rng = RandomNumberGenerator.Create())
                rng.GetBytes(salt);

            // 2) Переводим пароль в байты
            var passwordBytes = Encoding.UTF8.GetBytes(password);

            // 3) Хэшируем пароль с этой солью
            using (var argon2 = new Argon2i(passwordBytes))
            {
                argon2.Salt = salt;
                argon2.DegreeOfParallelism = 8;
                argon2.MemorySize = 65536;
                argon2.Iterations = 4;

                var hashBytes = argon2.GetBytes(32);
                return Convert.ToBase64String(salt) + "." + Convert.ToBase64String(hashBytes);
            }
        }

        public bool VerifyPassword(string hashedPassword, string password)
        {
            var parts = hashedPassword.Split('.');
            if (parts.Length != 2) return false;

            // Достаём соль и сохранённый хэш
            var salt = Convert.FromBase64String(parts[0]);
            var storedHash = Convert.FromBase64String(parts[1]);

            // Переводим введённый пароль в байты
            var passwordBytes = Encoding.UTF8.GetBytes(password);

            // Хэшируем введённый пароль на той же соли
            using (var argon2 = new Argon2i(passwordBytes))
            {
                argon2.Salt = salt;
                argon2.DegreeOfParallelism = 8;
                argon2.MemorySize = 65536;
                argon2.Iterations = 4;

                var computedHash = argon2.GetBytes(32);
                return ConstantTimeEquals(storedHash, computedHash);
            }
        }

        private bool ConstantTimeEquals(byte[] a, byte[] b)
        {
            if (a.Length != b.Length) return false;
            int diff = 0;
            for (int i = 0; i < a.Length; i++)
                diff |= a[i] ^ b[i];
            return diff == 0;
        }
    }



}
