using System.Runtime.InteropServices;
using Anyor.Domains.Orders.Models;
using Anyor.Domains.Orders.Repos;
using TinkoffAcquiringSdk;
using TinkoffAcquiringSdk.Requests;
using TinkoffAcquiringSdk.Responses;

namespace Anyor.Domains.Orders.Services;

public class PaymentOrderService
{
    private readonly PaymentOrderRepository _paymentOrderRepository;
    private readonly AcquiringApiClient _acquiringApiClient;

    public PaymentOrderService(
        PaymentOrderRepository paymentOrderRepository, 
        AcquiringApiClient acquiringApiClient)
    {
        _paymentOrderRepository = paymentOrderRepository;
        _acquiringApiClient = acquiringApiClient;
    }

    public async Task<PaymentOrder> CreateBankPaymentOrder(PaymentOrder paymentOrder, string successUrl, string failUrl)
    {
        var initRequest = new InitRequest
        {
            Amount = paymentOrder.Amount,
            OrderId = paymentOrder.Id.ToString(),
            ChargeFlag = false,
            CustomerKey = paymentOrder.FullCustomerName,
            PayForm = "Checkout",
            Recurrent = false,
            SuccessURL = successUrl,
            FailURL = failUrl
        };
        var result = await _acquiringApiClient.InitPaymentSessionAsync(initRequest);

        if (result == null)
        {
            throw new ExternalException("Error while creating Tinkoff order");
        }

        return await UpdatePaymentOrderFromTinkoffResponse(paymentOrder, result);
    }

    private async Task<PaymentOrder> UpdatePaymentOrderFromTinkoffResponse(PaymentOrder paymentOrder, InitResponse tinkoffResponse)
    {
        paymentOrder.BankOrderId = tinkoffResponse.PaymentId.Value.ToString();
        paymentOrder.BankStatus = tinkoffResponse.Status.Value.ToString();
        paymentOrder.PaymentUrl = tinkoffResponse.PaymentURL;
        
        // TODO: обновить внутренний статус на основании внешнего

        await _paymentOrderRepository.AddOrUpdateOrder(paymentOrder);
        return paymentOrder;
    }
}