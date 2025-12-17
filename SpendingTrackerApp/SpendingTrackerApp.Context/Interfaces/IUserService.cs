using SpendingTrackerApp.Contracts.Dtos.Requests;

namespace SpendingTrackerApp.Infrastructure.Interfaces
{
    public interface IUserService
    {
        Task<(int StatusCode, string Content)> GetUser(UserRequest request);

        Task<(int StatusCode, string Content)> CreateUser(UserRequest request);

        Task<(int StatusCode, string Content)> ResetPassword(UserEditRequest request);
    }
}
