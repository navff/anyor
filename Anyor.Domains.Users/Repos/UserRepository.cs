using Anyor.Common;
using Anyor.Common.YaDb;

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
        SELECT `id`, `email`, `token`, `phone`, `telegram`, `username`, `vk_id`, `first_name`, `last_name`, `site_amo_id`
        FROM `Users/users`
        ORDER BY `email`
        LIMIT 300;
    ";
        var result = await _yaDb.GetData(query, row => 
            new User.Models.User
            {
                Email = row["email"].ToStringField("email"),
                Token = row["token"].ToStringField("token"),
                Phone = row["phone"].ToStringOptionalField("phone"),
                Telegram = row["telegram"].ToStringOptionalField("telegram"),
                Username = row["username"].ToStringField("username"),
                VkId = row["vk_id"].ToStringOptionalField("vk_id"),
                FirstName = row["first_name"].ToUtf8OptionalField("first_name"),
                LastName = row["last_name"].ToUtf8OptionalField("last_name"),
                Id = row["id"].ToGuidField("id"),
                SiteAmoId = row["site_amo_id"].ToStringOptionalField("site_amo_id"),
            });
        return result;
    }
    
    
    public async Task<User.Models.User> GetUser(string id)
    {
        var query = @$"
        SELECT `id`, `email`, `token`, `phone`, `telegram`, `username`, `vk_id`, `first_name`, `last_name`, `site_amo_id`
        FROM `Users/users`
        WHERE id = '{id}'
        ORDER BY `email`
        LIMIT 1;
    ";
        var result = (await _yaDb.GetData(query, row => 
            new User.Models.User
            {
                Email = row["email"].ToStringField("email"),
                Token = row["token"].ToStringField("token"),
                Phone = row["phone"].ToStringOptionalField("phone"),
                Telegram = row["telegram"].ToStringOptionalField("telegram"),
                Username = row["username"].ToStringField("username"),
                VkId = row["vk_id"].ToStringOptionalField("vk_id"),
                FirstName = row["first_name"].ToUtf8OptionalField("first_name"),
                LastName = row["last_name"].ToUtf8OptionalField("last_name"),
                Id = row["id"].ToGuidField("id"),
                SiteAmoId = row["site_amo_id"].ToStringOptionalField("site_amo_id"),
            }))
            .SingleOrDefault();
        return result;
    }
    
    public async Task<Guid> AddUser(User.Models.User user)
    {
        
        if (user.Id == Guid.Empty)
        {
            user.Id = Guid.NewGuid();
        }
        

        string query = $@"UPSERT INTO `Users/users` ( 
                `id`, `email`, `phone`, `telegram`, `token`, `username`, `vk_id`, `first_name`, `last_name`, `site_amo_id` )
            VALUES ( 
                '{user.Id}',
                '{user.Email}',
                '{user.Phone}',
                '{user.Telegram}',
                '{user.Token}',
                '{user.Username}',
                '{user.VkId}',
                '{user.FirstName}',
                '{user.LastName}',
                '{user.SiteAmoId}');
            ";
        await _yaDb.ExecuteQuery(query);
        return user.Id;
    }
    
    public async Task RemoveUser(Guid id)
    {
        await _yaDb.RemoveRecord("Users/users", id);
    }
}