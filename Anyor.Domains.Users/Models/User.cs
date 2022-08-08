using Anyor.Common.Abstract.Users;

namespace Anyor.Domains.User.Models;

public class User: IUser
{
    public Guid Id { get; set; }
    
    public string Email { get; set; } = "";

    public string Token { get; set; } = "";
    
    public string? Phone { get; set; }
    
    public string? Telegram { get; set; }

    public string Username { get; set; } = "";

    public string? VkId { get; set; } = "";
    
    public string? FirstName { get; set; } = "";
    
    public string? LastName { get; set; } = "";
}