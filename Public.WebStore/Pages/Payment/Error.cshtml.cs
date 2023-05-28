using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Public.WebStore.Pages.Payment;

public class Error : PageModel
{
    public int? ProductId;
    
    public void OnGet(int? id)
    {
        ProductId = id;
    }
}