using AspNetCore.Yandex.ObjectStorage;
using AspNetCore.Yandex.ObjectStorage.Configuration;
using Newtonsoft.Json;

namespace Api.Common
{
    public class EnvironmentConfig
    {
        public string GoogleCredsJson { get; private set; } 
        public AmoTokenConfig AmoTokenConfig { get; private set; }
        
        public EnvironmentConfig()
        {
            var yaSecretKey = Environment.GetEnvironmentVariable("YA_OBJECT_STORAGE_SECRET_KEY")!;
            if (string.IsNullOrEmpty(yaSecretKey))
            {
                throw new InvalidOperationException("There is no environment variable YA_OBJECT_STORAGE_SECRET_KEY");
            }
            
            var oss = new YandexStorageService(new YandexStorageOptions()
            {
                AccessKey = "YCAJEWT3HNKiSCnrfiXOf_Bsm",
                SecretKey = yaSecretKey,
                BucketName = "secrets"
            });
            
            GetAmoKeyFileFromYaObjectStorage(oss);
            GetGoogleSheetsCreds(oss);
        }
    
       
        private void GetAmoKeyFileFromYaObjectStorage(YandexStorageService oss)
        {
            var amoSecretsResult = oss.ObjectService.GetAsync("anyor_amocrm_token.json").Result;
    
            var stream = amoSecretsResult.ReadAsStreamAsync().Result.Value;
            var reader = new StreamReader(stream);
            var text = reader.ReadToEnd();

            var config = JsonConvert.DeserializeObject<AmoTokenConfig>(text);
            if (config == null)
                throw new InvalidOperationException("Cannot read config file from Yandex Object Storage");
            
            this.AmoTokenConfig = config;
        }
        
        private void GetGoogleSheetsCreds(YandexStorageService oss)
        {
            var result = oss.ObjectService.GetAsync("google-sheets-vn-varnavskiy.json").Result;
    
            var stream = result.ReadAsStreamAsync().Result.Value;
            var reader = new StreamReader(stream);
            this.GoogleCredsJson = reader.ReadToEnd();
    
            if (string.IsNullOrEmpty(this.GoogleCredsJson))
            {
                throw new InvalidOperationException("Cannot read Google Creds from Yandex Object Storage");
            }
        }
    }
    
    public class AmoTokenConfig
    {
        [JsonProperty("token_type")]
        public string TokenType { get; set; } = "Bearer";
        
        [JsonProperty("expires_in")]
        public long ExpiresIn { get; set; }

        [JsonProperty("access_token")]
        public string AccessToken { get; set; } = string.Empty;
        
        [JsonProperty("refresh_token")]
        public string RefreshToken { get; set; } = string.Empty;
    }
}

