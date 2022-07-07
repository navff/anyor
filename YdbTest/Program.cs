using YdbTest;

var yaDb = new YaDb();
var query = @"
        SELECT `id`, `email`, `password_hash`, `phone`, `telegram`, `username`
        FROM `Users/users`
        ORDER BY `id`
        LIMIT 10;
    ";
var result = await yaDb.GetData(query, row => 
    new User
    {
        Email = System.Text.Encoding.Default.GetString(row["email"].GetOptionalString()),
        Id = row["id"].GetOptionalUint64()
    });

Console.WriteLine(result.First().Email);