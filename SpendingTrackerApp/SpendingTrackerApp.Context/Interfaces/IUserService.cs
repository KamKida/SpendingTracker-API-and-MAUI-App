using SpendingTrackerApp.Contracts.Dtos.Requests;

namespace SpendingTrackerApp.Infrastructure.Interfaces
{
    public interface IUserService
    {
        Task<HttpResponseMessage> LoginUser(UserRequest request);

        Task<HttpResponseMessage> CreateUser(UserRequest request);

        Task<HttpResponseMessage> ResetPassword(UserRequest request);

        Task<HttpResponseMessage> GetThisMonthInfo();
        Task<HttpResponseMessage> GetUserBaseData();
		Task<HttpResponseMessage> EditUser(UserRequest request);
        Task<HttpResponseMessage> DeleteUser();
    }
}
