using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace ProductStorage.Bitrix.Models;

public class GetProductBitrixResult
{
    public long Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Code { get; set; } = string.Empty;
    
    public string Active { get; set; } = string.Empty;

    [JsonProperty("PREVIEW_PICTURE")] 
    public string PreviewPicture { get; set; } = string.Empty;

    [JsonProperty("DETAIL_PICTURE")] 
    public string DetailPicture { get; set; } = string.Empty;
    
    public int Sort { get; set; }
    
    [JsonProperty("DATE_CREATE")] 
    public DateTimeOffset CreateDate { get; set; }
    
    [JsonProperty("CATALOG_ID")] 
    public int CatalogId { get; set; }
    
    [JsonProperty("SECTION_ID")] 
    public int SectionId { get; set; }
    
    public string Description { get; set; } = string.Empty;

    [JsonProperty("DESCRIPTION_TYPE")] 
    public string DescriptionType { get; set; } = "text";
    
    public decimal Price { get; set; }

    [JsonProperty("CURRENCY_ID")] 
    public string CurrencyId { get; set; } = string.Empty;

    [JsonProperty("PROPERTY_62")] 
    public List<BitrixImage> Images { get; set; } = new ();
}

public class BitrixImage
{
    public int ValueId { get; set; }
    public BitrixImageValue? Value { get; set; }
}

public class BitrixImageValue
{
    public int Id { get; set; }
    public string ShowUrl { get; set; } = string.Empty;
    public string DownloadUrl { get; set; } = string.Empty;
}