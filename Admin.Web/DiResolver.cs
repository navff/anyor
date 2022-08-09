using Anyor.Common;
using Anyor.Domains.Users.Repos;

namespace Admin.Web;

public static class DiResolver
{
    public static void RegisterDependencies( IServiceCollection services)
    {
        services.AddSingleton(typeof(YaDb));

        services.AddTransient<UserRepository, UserRepository>();
    }
}