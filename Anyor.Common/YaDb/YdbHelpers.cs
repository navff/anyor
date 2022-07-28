using Ydb.Sdk.Value;

namespace Anyor.Common;

public static class YdbHelpers
{
    public static string ToStringField(this YdbValue value)
    {
        byte[]? bytes = value.GetOptionalString();
        if (bytes == null)
            throw new InvalidOperationException("Empty value in not-nullable field");
        return System.Text.Encoding.Default.GetString(bytes);
    }
    
    public static string? ToStringOptionalField(this YdbValue value)
    {
        byte[]? bytes = value.GetOptionalString();
        return bytes == null 
            ? null 
            : System.Text.Encoding.Default.GetString(bytes);
    }
    
    public static Guid ToGuidField(this YdbValue value)
    {
        return Guid.Parse(value.ToStringField());
    }
    
    public static Guid ToGuidOptionalField(this YdbValue value)
    {
        return Guid.Parse(value.ToStringOptionalField()!);
    }
}