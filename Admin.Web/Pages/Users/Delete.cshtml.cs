using Anyor.Domains.Users.Repos;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Admin.Web.Pages.Users;

public class UsersDeleteModel : PageModel
{
    private readonly UserRepository _userRepository;

    public UsersDeleteModel(UserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public void OnGet(string id)
    {
        Console.WriteLine(id);
    }
    
    public void OnPost(string id)
    {
        _userRepository.RemoveUser(Guid.Parse(id));
    }
}