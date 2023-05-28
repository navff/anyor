using System.Runtime.InteropServices;
using Anyor.Domains.Orders.Models;
using Anyor.Domains.Orders.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ProductStorage.Abstract;
using ProductStorage.CommonModels;

namespace Public.WebStore.Pages.Store;

public class ProductModel : PageModel
{
    private IProductStorage _productStorage;
    private PaymentOrderService _paymentOrderService;
    public Product Product;

    public ProductModel(
        IProductStorage productStorage, 
        PaymentOrderService paymentOrderService)
    {
        _productStorage = productStorage;
        _paymentOrderService = paymentOrderService;
    }

    public async Task OnGet(int id)
    {
        this.Product = await _productStorage.GetProduct(id);
    }

    public async Task<IActionResult > OnPost(int id)
    {
        Product = await _productStorage.GetProduct(id);
        var order = new PaymentOrder()
        {
            Amount = Convert.ToUInt32(Product.Price * 100),
            Id = Guid.NewGuid(),
            DateCreated = DateTimeOffset.Now,
            DateModified = DateTimeOffset.Now,
            InternalStatus = PaymentOrderStatusInternal.New
        };
        var baseUrl = $"{this.Request.Scheme}://{this.Request.Host}{this.Request.PathBase}";
        order = await _paymentOrderService.CreateBankPaymentOrder(
            order,
            baseUrl + "/payment/success",
            baseUrl + $"$/payment/error/{id}");
        if (string.IsNullOrEmpty(order.PaymentUrl))
            throw new ExternalException("Cannot create Payment Order");
            
        return Redirect(order.PaymentUrl);
    }
}