using SpendingTracker.Contracts.Dtos.Requests;
using SpendingTracker.Contracts.Dtos.Responses;

namespace SpendingTracker.Application.Interfaces.Services
{
    public interface IUserService
    {
        Task CreateUser(UserRequest request);

        Task<UserResponse> LoginUser(UserRequest request);

        Task ResetPassword(UserEditRequest request);

        //Task EditUser(UserEditRequest request);

    }
}
