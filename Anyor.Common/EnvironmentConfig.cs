namespace Anyor.Common;

public class EnvironmentConfig
{
    public string YdbEndpoint { get; private set; }
    
    public string YdbDbAddress { get; private set; }
    
    public string YaSaKeyFilePath { get; private set; }

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
        if (string.IsNullOrEmpty(YaSaKeyFilePath))
        {
            throw new InvalidOperationException("There is no environment variable YA_SA_KEYFILE_PATH");
        }
    }
}