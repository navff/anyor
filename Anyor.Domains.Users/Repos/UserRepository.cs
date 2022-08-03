using Anyor.Common;

namespace Anyor.Domains.Users.Repos;

public class UserRepository
{
    private readonly YaDb _yaDb = new YaDb();

    public async Task<List<Models.User>> GetUsers()
    {
        var query = @"
        SELECT `id`, `email`, `token`, `phone`, `telegram`, `username`
        FROM `Users/users`
        ORDER BY `email`
        LIMIT 10;
    ";
        var result = await _yaDb.GetData(query, row => 
            new Models.User
            {
                Email = row["email"].ToStringField(),
                Token = row["token"].ToStringField(),
                Phone = row["phone"].ToStringOptionalField(),
                Telegram = row["telegram"].ToStringOptionalField(),
                Username = row["username"].ToStringField(),
                Id = row["id"].ToGuidField()
            });
        return result;
    }
    
    public async Task<Guid> AddUser(Models.User user)
    {
        
        if (user.Id == Guid.Empty)
        {
            user.Id = Guid.NewGuid();
        }
        

        string query = $@"UPSERT INTO `Users/users` ( 
                `id`, `email`, `phone`, `telegram`, `token`, `username` )
            VALUES ( 
                '{user.Id}',
                '{user.Email}',
                '{user.Phone}',
                '{user.Telegram}',
                '{user.Token}',
                '{user.Username}');
            ";
        await _yaDb.ExecuteQuery(query);
        return user.Id;
    }
    
    public async Task RemoveUser(Guid id)
    {
        await _yaDb.RemoveRecord("Users/users", id);
    }
}