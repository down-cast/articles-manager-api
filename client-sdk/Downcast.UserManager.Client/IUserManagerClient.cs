using Downcast.UserManager.Client.Model;

using Refit;

namespace Downcast.UserManager.Client;

public interface IUserManagerClient
{
    [Post("/api/v1/users/validate-credentials")]
    Task<HttpResponseMessage> ValidateCredentials([Body] AuthenticationRequest request);

    [Put("/api/v1/users/{userId}/password")]
    Task<HttpResponseMessage> UpdatePassword(string userId, [Body] UpdatePasswordInput request);

    [Get("/api/v1/users/{userId}")]
    Task<ApiResponse<User>> GetUser(string userId);

    [Get("/api/v1/users/email/{email}")]
    Task<ApiResponse<User>> GetByEmail(string email);

    [Delete("/api/v1/users/{userId}")]
    Task<HttpResponseMessage> DeleteUser(string userId);

    [Post("/api/v1/users")]
    Task<ApiResponse<User>> CreateUser([Body] CreateUserInputModel request);

    [Put("/api/v1/users/{userId}")]
    Task<HttpResponseMessage> UpdateUser(string userId, [Body] UpdateUserInputModel request);

    [Put("/api/v1/users/{userId}/roles")]
    Task<HttpResponseMessage> AddRoles(string userId, [Body] IEnumerable<string> roles);

    [Delete("/api/v1/users/{userId}/roles")]
    Task<HttpResponseMessage> RemoveRoles(string userId, [Body] IEnumerable<string> roles);
}