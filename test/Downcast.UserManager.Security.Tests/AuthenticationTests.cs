using System.Net;

using Downcast.UserManager.Client.Model;
using Downcast.UserManager.Tests.Utils;

using FluentAssertions;

using Xunit;

namespace Downcast.UserManager.Security.Tests;

public class AuthenticationTests : BaseTestClass
{
    [Fact]
    public async Task ValidateCredentials_Success()
    {
        HttpResponseMessage res = await Client.ValidateCredentials(new AuthenticationRequest()
        {
            Email = ExistingUser.Email,
            Password = ExistingUserPassword
        }).ConfigureAwait(false);

        res.IsSuccessStatusCode.Should().BeTrue();
    }

    [Fact]
    public async Task ValidateCredentials_Wrong_Password()
    {
        HttpResponseMessage res = await Client.ValidateCredentials(new AuthenticationRequest()
        {
            Email = ExistingUser.Email,
            Password = ExistingUserPassword + "1"
        }).ConfigureAwait(false);

        res.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task ValidateCredentials_Case_InsensitiveTest()
    {
        HttpResponseMessage res = await Client.ValidateCredentials(new AuthenticationRequest()
        {
            Email = ExistingUser.Email.ToLower(),
            Password = ExistingUserPassword
        }).ConfigureAwait(false);
        res.IsSuccessStatusCode.Should().BeTrue();

        res = await Client.ValidateCredentials(new AuthenticationRequest()
        {
            Email = ExistingUser.Email.ToUpper(),
            Password = ExistingUserPassword
        }).ConfigureAwait(false);
        res.IsSuccessStatusCode.Should().BeTrue();
    }
}