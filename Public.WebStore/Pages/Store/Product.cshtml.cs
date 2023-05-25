using Microsoft.AspNetCore.Mvc.RazorPages;
using ProductStorage.Abstract;
using ProductStorage.CommonModels;

namespace Public.WebStore.Pages.Store;

public class ProductModel : PageModel
{
    private IProductStorage _productStorage;
    public Product Product;

    public ProductModel(IProductStorage productStorage)
    {
        _productStorage = productStorage;
    }

    public async Task OnGet(int id)
    {
        this.Product = await _productStorage.GetProduct(id);
    }
}