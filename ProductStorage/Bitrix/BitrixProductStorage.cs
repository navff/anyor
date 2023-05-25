using Anyor.Common;
using Newtonsoft.Json;
using ProductStorage.Abstract;
using ProductStorage.Bitrix.Models;
using ProductStorage.CommonModels;

namespace ProductStorage.Bitrix;

public class BitrixProductStorage: IProductStorage
{
    private BitrixApiClient _bitrixApiClient;
    private EnvironmentConfig _environmentConfig;
    private string _bitrixHookUrl = string.Empty;

    public BitrixProductStorage(HttpClient httpClient, EnvironmentConfig environmentConfig)
    {
        _bitrixApiClient = new BitrixApiClient(httpClient);
        _environmentConfig = environmentConfig;
        _bitrixHookUrl = $"https://p.anyor.ru/rest/1/{_environmentConfig.BitrixWebhookKey}/";
    }

    public async Task<Product> GetProduct(long id)
    {
        var stringResponse = await _bitrixApiClient.Get(_bitrixHookUrl + $"crm.product.get.json?ID={id}");
        if (string.IsNullOrEmpty(stringResponse))
        {
            throw new InvalidOperationException("Error while getting Product from Bitrix");
        }

        var bitrixProduct = JsonConvert.DeserializeObject<BitrixApiResult<GetProductBitrixResult>>(stringResponse)?.Result;
        
        var result = new Product
        {
            Id = bitrixProduct.Id,
            Description = bitrixProduct.Description,
            Images = bitrixProduct.Images.Select(i => new ProductImage
            {
                Id = i.Value.Id,
                FullUrl = _environmentConfig.BitrixBaseUrl + i.Value.DownloadUrl,
                PreviewUrl = _environmentConfig.BitrixBaseUrl + i.Value.ShowUrl
            }).ToList(),
            Price = bitrixProduct.Price,
            Sort = bitrixProduct.Sort,
            Title = bitrixProduct.Name,
            CurrencyCode = bitrixProduct.CurrencyId,
            IsActive = bitrixProduct.Active == "Y"
        };
        return result;
    }
}