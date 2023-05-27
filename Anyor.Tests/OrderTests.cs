using Anyor.Common;
using Anyor.Domains.Orders.Models;
using Anyor.Domains.Orders.Repos;
using Anyor.Domains.User.Models;
using Anyor.Domains.Users.Repos;

namespace Anyor.Tests;

public class OrderTests
{
    private readonly PaymentOrderRepository _orderRepository = new (new YaDb());
    
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public async Task GetOrders()
    {
        var orders = await _orderRepository.GetOrders();
        Assert.IsTrue(orders.Any());
        Assert.IsFalse(string.IsNullOrEmpty(orders.First().Id.ToString()));
    }
    
    [Test]
    public async Task AddOrderPartial()
    {
        var order = new PaymentOrder
        {
            Amount = 2000,
            DateCreated = DateTimeOffset.Now,
            DateModified = DateTimeOffset.Now,
            InternalStatus = PaymentOrderStatusInternal.New
        };
        
        var orderId = await _orderRepository.AddOrUpdateOrder(order);
        Assert.That(orderId, Is.Not.EqualTo(Guid.Empty));
    }
}