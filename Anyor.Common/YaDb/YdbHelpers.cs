using Ydb.Sdk.Value;

namespace Anyor.Common;

public static class YdbHelpers
{
    public static string ToStringField(this YdbValue value, string fieldName)
    {
        try
        {
            // Костыль: иногда от YDB может неожиданно вместо OptionalType прийти String
            var bytes = value.TypeId switch
            {
                YdbTypeId.String => value.GetString(),
                YdbTypeId.OptionalType => value.GetOptionalString(),
                _ => throw new InvalidDataException($"Unknown YDB Type: {value.TypeId}")
            };

            if (bytes == null)
                throw new InvalidOperationException("Empty value in not-nullable field");
            return System.Text.Encoding.Default.GetString(bytes);
        }
        catch (Exception e)
        {
            throw new Exception($"YDB: Cannot get {fieldName}, {value.TypeId}", e);
        }
    }
    
    public static string? ToStringOptionalField(this YdbValue value, string fieldName)
    {
        try
        {
            byte[]? bytes = value.GetOptionalString();
            return bytes == null 
                ? null 
                : System.Text.Encoding.Default.GetString(bytes);
        }
        catch (Exception e)
        {
            throw new Exception($"YDB: Cannot get {fieldName}", e);
        }
    }
    
    public static string? ToUtf8OptionalField(this YdbValue value, string fieldName)
    {
        try
        {
            return value.GetOptionalUtf8();
        }
        catch (Exception e)
        {
            throw new Exception($"YDB: Cannot get {fieldName}", e);
        }
        
    }
    
    public static Guid ToGuidField(this YdbValue value, string fieldName)
    {
        return Guid.Parse(value.ToStringField(fieldName));
    }
    
    public static Guid ToGuidOptionalField(this YdbValue value, string fieldName)
    {
        return Guid.Parse(value.ToStringOptionalField(fieldName)!);
    }
}