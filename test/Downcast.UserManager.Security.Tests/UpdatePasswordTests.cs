using System.Net;

using Downcast.UserManager.Client.Model;
using Downcast.UserManager.Tests.Utils;
using Downcast.UserManager.Tests.Utils.DataFakers;

using FluentAssertions;

using Xunit;

namespace Downcast.UserManager.Security.Tests;

public class UpdatePasswordTests : BaseTestClass
{
    [Fact]
    public async Task UpdatePassword_Success()
    {
        // Ensure that password is valid
        HttpResponseMessage res = await Client.ValidateCredentials(new AuthenticationRequest()
        {
            Email = ExistingUser.Email.ToLower(),
            Password = ExistingUserPassword
        }).ConfigureAwait(false);
        res.IsSuccessStatusCode.Should().BeTrue();

        UpdatePasswordInput newPassword = new UpdatePasswordInputFaker().Generate();
        res = await Client.UpdatePassword(ExistingUser.Id, newPassword).ConfigureAwait(false);
        res.IsSuccessStatusCode.Should().BeTrue();

        // Ensure that old password is no longer valid
        res = await Client.ValidateCredentials(new AuthenticationRequest()
        {
            Email = ExistingUser.Email.ToLower(),
            Password = ExistingUserPassword
        }).ConfigureAwait(false);
        res.StatusCode.Should().Be(HttpStatusCode.Unauthorized);


        // Ensure that new password is valid
        res = await Client.ValidateCredentials(new AuthenticationRequest()
        {
            Email = ExistingUser.Email.ToLower(),
            Password = newPassword.Password
        }).ConfigureAwait(false);
        res.IsSuccessStatusCode.Should().BeTrue();
    }


    [Fact]
    public async Task UpdatePassword_NotFound()
    {
        UpdatePasswordInput newPassword = new UpdatePasswordInputFaker().Generate();
        HttpResponseMessage res = await Client.UpdatePassword(ExistingUser.Id + "1", newPassword).ConfigureAwait(false);
        res.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}