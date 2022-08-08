using Anyor.Common;
using Anyor.Domains.User.Models;
using Anyor.Domains.Users.Repos;

namespace Anyor.Tests;

public class UserTests
{
    private UserRepository _userRepository = new UserRepository(new YaDb());
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public async Task GetUsers()
    {
        
        var users = await _userRepository.GetUsers();
        Assert.IsTrue(users.Any());
        Assert.IsFalse(string.IsNullOrEmpty(users.First().Email));
    }
    
    [Test]
    public async Task AddAndRemoveUser()
    {
        var user = new User()
        {
            Email = Guid.NewGuid().ToString(),
            Phone = Guid.NewGuid().ToString(),
            Telegram = Guid.NewGuid().ToString(),
            Token = Guid.NewGuid().ToString(),
            Username = Guid.NewGuid().ToString(),
            FirstName = Guid.NewGuid().ToString(),
            LastName = Guid.NewGuid().ToString(),
            VkId = "123456678"
        };
        var id = await _userRepository.AddUser(user);
        await _userRepository.RemoveUser(id);
    }
}