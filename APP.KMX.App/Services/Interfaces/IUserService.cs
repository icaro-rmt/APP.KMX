namespace APP.KMX.App.Services.Interfaces
{
    public interface IUserService
    {
        Task<bool> ValidateUserAsync(string email, string password);
        Task<bool> RegisterUserAsync(string name, string email, string password);
        Task<bool> UserExistsAsync(string email);
        Task<string> GetUserNameAsync(string email);
    }
}
