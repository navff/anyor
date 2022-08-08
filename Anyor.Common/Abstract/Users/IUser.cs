namespace Anyor.Common.Abstract.Users;

public interface IUser
{
    Guid Id { get; set; }
    
    string Email { get; set; }

    string Token { get; set; }
    
    string? Phone { get; set; }
    
    string? Telegram { get; set; }

    string Username { get; set; }
    
    string? VkId { get; set; }
}