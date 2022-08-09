using Anyor.Common;

namespace Anyor.Domains.Users.Repos;

public class UserRepository
{
    private readonly YaDb _yaDb;

    public UserRepository(YaDb yaDb)
    {
        _yaDb = yaDb;
    }

    public async Task<List<User.Models.User>> GetUsers()
    {
        var query = @"
        SELECT `id`, `email`, `token`, `phone`, `telegram`, `username`, `vk_id`, `first_name`, `last_name`
        FROM `Users/users`
        ORDER BY `email`
        LIMIT 300;
    ";
        await _yaDb.Init();
        var result = await _yaDb.GetData(query, row => 
            new User.Models.User
            {
                Email = row["email"].ToStringField(),
                Token = row["token"].ToStringField(),
                Phone = row["phone"].ToStringOptionalField(),
                Telegram = row["telegram"].ToStringOptionalField(),
                Username = row["username"].ToStringField(),
                VkId = row["vk_id"].ToStringOptionalField(),
                FirstName = row["first_name"].ToUtf8OptionalField(),
                LastName = row["last_name"].ToUtf8OptionalField(),
                Id = row["id"].ToGuidField()
            });
        return result;
    }
    
    public async Task<Guid> AddUser(User.Models.User user)
    {
        
        if (user.Id == Guid.Empty)
        {
            user.Id = Guid.NewGuid();
        }
        

        string query = $@"UPSERT INTO `Users/users` ( 
                `id`, `email`, `phone`, `telegram`, `token`, `username`, `vk_id`, `first_name`, `last_name` )
            VALUES ( 
                '{user.Id}',
                '{user.Email}',
                '{user.Phone}',
                '{user.Telegram}',
                '{user.Token}',
                '{user.Username}',
                '{user.VkId}',
                '{user.FirstName}',
                '{user.LastName}');
            ";
        await _yaDb.ExecuteQuery(query);
        return user.Id;
    }
    
    public async Task RemoveUser(Guid id)
    {
        await _yaDb.RemoveRecord("Users/users", id);
    }
}