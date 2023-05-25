namespace ProductStorage.Bitrix;

public class BitrixApiClient
{
    private readonly HttpClient _httpClient;
    
    public BitrixApiClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<string> Get(string url)
    {
        var response = await _httpClient.GetAsync(url);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsStringAsync();
    }
}