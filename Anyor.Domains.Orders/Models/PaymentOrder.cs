namespace Anyor.Domains.Orders.Models;

/// <summary>
/// Заказ на оплату
/// </summary>
public class PaymentOrder
{
    /// <summary>
    /// Internal PaymentOrderId
    /// </summary>
    public Guid Id { get; set; } = Guid.Empty;
    
    /// <summary>
    /// Id заказа в Банке
    /// </summary>
    public string BankOrderId { get; set; }
    
    /// <summary>
    /// Сумма заказа в рублях
    /// </summary>
    public uint Amount { get; set; }
    
    /// <summary>
    /// ID пользователя, если не аноним 
    /// </summary>
    public Guid? UserId { get; set; }
    
    /// <summary>
    /// Имя пользователя, если не аноним
    /// </summary>
    public string? Username { get; set; }
    
    /// <summary>
    /// Человеческое имя покупателя
    /// </summary>
    public string? FullCustomerName { get; set; }
    
    /// <summary>
    /// Email пользователя, если не аноним
    /// </summary>
    public string? UserEmail { get; set; }
    
    /// <summary>
    /// Статус заказа по системе банка
    /// </summary>
    public string? BankStatus { get; set; }
    
    /// <summary>
    /// Статус заказа внутренний
    /// </summary>
    public string? InternalStatus { get; set; }
    
    /// <summary>
    /// Дата создания
    /// </summary>
    public DateTimeOffset DateCreated { get; set; }
    
    /// <summary>
    /// Время последнего изменения
    /// </summary>
    public DateTimeOffset DateModified { get; set; }
    
    /// <summary>
    /// Урл, страницы оплаты
    /// </summary>
    public string? PaymentUrl { get; set; }
}
