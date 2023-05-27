namespace Anyor.Domains.Orders.Models;

public static class PaymentOrderStatusInternal
{
    public static string New = "New";
    public static string InProgress = "InProgress";
    public static string Paid = "Paid";
    public static string Cancelled = "Cancelled";
    public static string Error = "Error";
}