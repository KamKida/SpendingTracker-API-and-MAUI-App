using SpendingTracker.Contracts.Dtos.Requests;
using SpendingTracker.Contracts.Dtos.Responses;

namespace SpendingTracker.Application.Interfaces.Services
{
    public interface IUserService
    {
        Task CreateUser(UserRequest request);

        Task<string> LoginUser(UserRequest request);

        Task ResetPassword(UserRequest request);

        Task<UserResponse> GetUserThisMonthInfo();
        Task<UserResponse> GetUserBaseData();


		Task EditUser(UserRequest request);
        Task DeleteUser();

    }
}
