using Anyor.Common;
using Anyor.Common.YaDb;
using Anyor.Domains.Orders.Models;

namespace Anyor.Domains.Orders.Repos;

public class PaymentOrderRepository
{
    private readonly YaDb _yaDb;

    public PaymentOrderRepository(YaDb yaDb)
    {
        _yaDb = yaDb;
    }
    
    public async Task<List<PaymentOrder>> GetOrders()
    {
        var query = @"
        SELECT `Id`, `Amount`, `Username`, `BankStatus`, `DateCreated`, 
               `DateModified`, `FullCustomerName`, `UserId`, `UserEmail`, 
               `InternalStatus`, `BankOrderId`, `PaymentURL`
        FROM `Orders/PaymentOrders`
        ORDER BY `DateCreated`
        LIMIT 300;
    ";
        var result = await _yaDb.GetData(query, row => 
            new PaymentOrder
            {
                Id = row["Id"].ToGuidField("Id"),
                Amount = row["Amount"].GetOptionalUint32() ?? 0,
                Username = row["Username"].ToStringOptionalField("Username"),
                BankStatus = row["BankStatus"].ToStringOptionalField("BankStatus"),
                DateCreated = row["DateCreated"].GetOptionalDatetime() ?? DateTime.MinValue,
                DateModified = row["DateModified"].GetOptionalDatetime() ?? DateTime.MinValue,
                FullCustomerName = row["FullCustomerName"].ToUtf8OptionalField("FullCustomerName"),
                UserId = row["UserId"].ToGuidField("UserId"),
                UserEmail = row["UserEmail"].ToStringOptionalField("UserEmail"),
                InternalStatus = row["InternalStatus"].ToStringOptionalField("InternalStatus"),
                BankOrderId = row["BankOrderId"].ToStringOptionalField("BankOrderId") ?? "",
                PaymentUrl = row["PaymentURL"].ToStringOptionalField("PaymentURL"),
            });
        return result;
    }
    
    public async Task<Guid> AddOrUpdateOrder(PaymentOrder paymentOrder)
    {
        
        if (paymentOrder.Id == Guid.Empty)
        {
            paymentOrder.Id = Guid.NewGuid();
        }
        

        string query = $@"UPSERT INTO `Orders/PaymentOrders` ( 
                `Id`, `BankOrderId`, `Amount`, `UserId`, `Username`, `FullCustomerName`,
                `UserEmail`, `BankStatus`, `InternalStatus`, `DateCreated`, `DateModified`, `PaymentURL`)
            VALUES ( 
                '{paymentOrder.Id}',
                '{paymentOrder.BankOrderId}',
                 {paymentOrder.Amount},
                '{paymentOrder.UserId}',
                '{paymentOrder.Username}',
                '{paymentOrder.FullCustomerName}',
                '{paymentOrder.UserEmail}',
                '{paymentOrder.BankStatus}',
                '{paymentOrder.InternalStatus}',
                DateTime('{paymentOrder.DateCreated:yyyy-MM-ddThh:mm:ssZ}'),
                DateTime('{paymentOrder.DateModified:yyyy-MM-ddThh:mm:ssZ}'),
                '{paymentOrder.PaymentUrl}');
            ";
        await _yaDb.ExecuteQuery(query);
        return paymentOrder.Id;
    }
}