using Anyor.Domains.User.Models;
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
    
    [Test]
    public async Task AddAndRemoveUser()
    {
        var userRepository = new UserRepository();
        var user = new User()
        {
            Email = Guid.NewGuid().ToString(),
            Phone = Guid.NewGuid().ToString(),
            Telegram = Guid.NewGuid().ToString(),
            Token = Guid.NewGuid().ToString(),
            Username = Guid.NewGuid().ToString()
        };
        var id = await userRepository.AddUser(user);
        await userRepository.RemoveUser(id);
    }
}