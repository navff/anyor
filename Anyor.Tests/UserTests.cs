using Anyor.Domains.User.Repos;

namespace Anyor.Tests;

public class UserTests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public async Task GetUsers()
    {
        var userRepository = new UserRepository();
        var users = await userRepository.GetUsers();
        Assert.IsTrue(users.Any());
        Assert.IsFalse(string.IsNullOrEmpty(users.First().Email));
    }
}