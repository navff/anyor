using Anyor.Domains.Users.Repos;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Admin.Web.Pages.Users;

public class Edit : PageModel
{
    private UserRepository _userRepository;

    public Edit(UserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task OnGet(string id)
    {
        ViewData["User"] = await _userRepository.GetUser(id);
        Console.WriteLine(ViewData["User"]);
    }
}