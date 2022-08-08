using Anyor.Domains.Users.Repos;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Admin.Web.Pages.Users;

public class UsersIndexModel : PageModel
{
    private readonly UserRepository _userRepository;

    public UsersIndexModel(UserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task OnGet()
    {
        ViewData["Users"] = await _userRepository.GetUsers();
    }
}