using SpendingTrackerApp.Contracts.Dtos.Requests;

namespace SpendingTrackerApp.Infrastructure.Interfaces
{
    public interface IUserService
    {
        Task<(int StatusCode, string Content)> LoginUser(UserRequest request);

        Task<(int StatusCode, string Content)> CreateUser(UserRequest request);

        Task<(int StatusCode, string Content)> ResetPassword(UserRequest request);

        Task<(int StatusCode, string Content)> GetBaseInfo();
    }
}
