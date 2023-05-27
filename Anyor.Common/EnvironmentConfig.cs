using AspNetCore.Yandex.ObjectStorage;
using AspNetCore.Yandex.ObjectStorage.Configuration;

namespace Anyor.Common;

public class EnvironmentConfig
{
    public string YdbEndpoint { get; private set; }
    
    public string YdbDbAddress { get; private set; }
    
    public string YaSaKeyFilePath { get; private set; }
    
    public string YaKeyId { get; private set; }
    
    public string YaServiceAccountId { get; private set; }
    
    public string YaPrivateKey { get; private set; }
    
    public string BitrixWebhookKey { get; private set; }
    
    public string BitrixBaseUrl { get; private set; }
    
    public string TinkoffTerminalKey { get; private set; }
    
    public string TinkoffTerminalPassword { get; private set; }

    public EnvironmentConfig()
    {
        var yaSecretKey = Environment.GetEnvironmentVariable("YA_OBJECT_STORAGE_SECRET_KEY")!;
        if (string.IsNullOrEmpty(yaSecretKey))
        {
            throw new InvalidOperationException("There is no environment variable YA_OBJECT_STORAGE_SECRET_KEY");
        };
        
        var oss = new YandexStorageService(new YandexStorageOptions()
        {
            AccessKey = "YCAJEWT3HNKiSCnrfiXOf_Bsm",
            SecretKey = yaSecretKey,
            BucketName = "secrets"
        });
        var result = oss.ObjectService.GetAsync("anyor.json").Result;
        
        var stream = result.ReadAsStreamAsync().Result.Value;
        var reader = new StreamReader(stream);
        var text = reader.ReadToEnd();
        
        var config = Newtonsoft.Json.JsonConvert.DeserializeObject<EnvironmentConfigJson>(text);
        if (config != null)
        {
            this.YdbEndpoint = config.YdbEndpoint;
            this.YaKeyId = config.YaKeyId;
            this.YaPrivateKey = Base64Decode(config.YaPrivateKey!);
            this.YdbDbAddress = config.YdbDbAddress;
            this.YaServiceAccountId = config.YaServiceAccountId;
            this.YaSaKeyFilePath = config.YaSaKeyFilePath;
            this.BitrixWebhookKey = config.BitrixWebhookKey;
            this.BitrixBaseUrl = config.BitrixBaseUrl;
            this.TinkoffTerminalKey = config.TinkoffTerminalKey;
            this.TinkoffTerminalPassword = config.TinkoffTerminalPassword;
        }
        else
        {
            throw new InvalidOperationException("Cannot read config file from Yandex Object Storage");
        }
    }

    private static string Base64Decode(string base64EncodedData) {
        // если ключ обрезал знак `=` скриптом деплоя
        if (!base64EncodedData.EndsWith("="))
        {
            base64EncodedData += "=";
        }
        var base64EncodedBytes = Convert.FromBase64String(base64EncodedData);
        return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
    }
}

public class EnvironmentConfigJson
{
    public string YdbEndpoint { get; set; }
    
    public string YdbDbAddress { get; set; }
    
    public string YaSaKeyFilePath { get; set; }
    
    public string YaKeyId { get; set; }
    
    public string YaServiceAccountId { get; set; }
    
    public string YaPrivateKey { get; set; }
    
    public string BitrixWebhookKey { get; set; }
    
    public string BitrixBaseUrl { get; set; }
    
    public string TinkoffTerminalKey { get; set; }
    
    public string TinkoffTerminalPassword { get; set; }
}