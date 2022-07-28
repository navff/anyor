using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyTested.AspNetCore.Mvc.Builders.Data;

namespace Public.WebStore.Pages.Store
{
    public class StoreIndexModel : PageModel
    {
        public string CategoryName = "";

        public void OnGet(string? category)
        {
            if (category == "sweets") CategoryName = "��������";
            if (category == "cakes") CategoryName = "�������";

            ViewData["Title"] = CategoryName;
        }
    }
}
