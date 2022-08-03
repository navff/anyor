namespace Anyor.Domains.Users.Models;

public class User
{
    public Guid Id { get; set; }
    
    public string Email { get; set; } = "";

    public string Token { get; set; } = "";
    
    public string? Phone { get; set; }
    
    public string? Telegram { get; set; }

    public string Username { get; set; } = "";
}