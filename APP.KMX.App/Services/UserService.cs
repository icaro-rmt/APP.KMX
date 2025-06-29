using APP.KMX.App.Services.Interfaces;
using System.Security.Cryptography;
using System.Text;

namespace APP.KMX.App.Services
{
    public class UserService : IUserService
    {
        // In a real application, you would use a database
        // This is a simple in-memory storage for demonstration
        private static readonly Dictionary<string, (string Name, string HashedPassword)> _users = new();

        static UserService()
        {
            // Add a default test user
            var testPassword = HashPassword("Test123!");
            _users["test@kmx.com"] = ("Test User", testPassword);
        }

        public async Task<bool> ValidateUserAsync(string email, string password)
        {
            await Task.Delay(1); // Simulate async operation
            
            if (_users.TryGetValue(email.ToLower(), out var userData))
            {
                var hashedPassword = HashPassword(password);
                return userData.HashedPassword == hashedPassword;
            }
            return false;
        }

        public async Task<bool> RegisterUserAsync(string name, string email, string password)
        {
            await Task.Delay(1); // Simulate async operation
            
            var emailLower = email.ToLower();
            if (_users.ContainsKey(emailLower))
                return false;

            var hashedPassword = HashPassword(password);
            _users[emailLower] = (name, hashedPassword);
            return true;
        }

        public async Task<bool> UserExistsAsync(string email)
        {
            await Task.Delay(1); // Simulate async operation
            return _users.ContainsKey(email.ToLower());
        }

        public async Task<string> GetUserNameAsync(string email)
        {
            await Task.Delay(1); // Simulate async operation
            
            if (_users.TryGetValue(email.ToLower(), out var userData))
            {
                return userData.Name;
            }
            return email; // Fallback to email if name not found
        }

        private static string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password + "SALT_KMX_2024"));
            return Convert.ToBase64String(hashedBytes);
        }
    }
}
