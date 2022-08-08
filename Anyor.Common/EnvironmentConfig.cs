namespace Anyor.Common;

public class EnvironmentConfig
{
    public string YdbEndpoint { get; private set; }
    
    public string YdbDbAddress { get; private set; }
    
    public string YaSaKeyFilePath { get; private set; }
    
    public string YaKeyId { get; private set; }
    
    public string YaServiceAccountId { get; private set; }
    
    public string YaPrivateKey { get; private set; }

    public EnvironmentConfig()
    {
        YdbEndpoint = Environment.GetEnvironmentVariable("YDB_ENDPOINT")!;
        if (string.IsNullOrEmpty(YdbEndpoint))
        {
            throw new InvalidOperationException("There is no environment variable YDB_ENDPOINT");
        }

        YdbDbAddress = Environment.GetEnvironmentVariable("YDB_ADDRESS")!;
        if (string.IsNullOrEmpty(YdbDbAddress))
        {
            throw new InvalidOperationException("There is no environment variable YDB_ADDRESS");
        }
        
        YaSaKeyFilePath = Environment.GetEnvironmentVariable("YA_SA_KEYFILE_PATH")!;
        
        YaKeyId = Environment.GetEnvironmentVariable("YA_KEY_ID")!;
        
        YaServiceAccountId = Environment.GetEnvironmentVariable("YA_SERVICE_ACCOUNT_ID")!;
        
        YaPrivateKey = Base64Decode(Environment.GetEnvironmentVariable("YA_PRIVATE_KEY", EnvironmentVariableTarget.Machine)!);
        
        if (string.IsNullOrEmpty(YaSaKeyFilePath) &&
            (string.IsNullOrEmpty(YaKeyId) || string.IsNullOrEmpty(YaServiceAccountId) || string.IsNullOrEmpty(YaPrivateKey)))
        {
            throw new InvalidOperationException("YA_SA_KEYFILE_PATH must be set. Or YA_KEY_ID + YA_SERVICE_ACCOUNT_ID + YA_PRIVATE_KEY");
        }
    }
    
    private static string Base64Decode(string base64EncodedData) {
        var base64EncodedBytes = Convert.FromBase64String(base64EncodedData);
        return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
    }
}