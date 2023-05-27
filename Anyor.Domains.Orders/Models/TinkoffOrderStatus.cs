namespace Anyor.Domains.Orders.Models;

public static class TinkoffOrderStatus
{
    public static string NEW = "NEW";
    public static string FORM_SHOWED = "FORM_SHOWED";
    public static string DEADLINE_EXPIRED = "DEADLINE_EXPIRED";
    public static string CANCELED = "CANCELED";
    public static string PREAUTHORIZING = "PREAUTHORIZING";
    public static string AUTHORIZING = "AUTHORIZING";
    public static string AUTHORIZED = "AUTHORIZED";
    public static string AUTH_FAIL = "AUTH_FAIL";
    public static string REJECTED = "REJECTED";
    public static string DS_CHECKING = "DS_CHECKING";
    public static string DS_CHECKED = "DS_CHECKED";
    public static string REVERSING = "REVERSING";
    public static string PARTIAL_REVERSED = "PARTIAL_REVERSED";
    public static string REVERSED = "REVERSED";
    public static string CONFIRMING = "CONFIRMING";
    public static string CONFIRMED = "CONFIRMED";
    public static string REFUNDING = "REFUNDING";
    public static string PARTIAL_REFUNDED = "PARTIAL_REFUNDED";
}