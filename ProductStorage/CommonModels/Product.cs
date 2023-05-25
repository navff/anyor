namespace ProductStorage.CommonModels;

public class Product
{
    public long Id { get; set; }
    
    public string Title { get; set; }
    
    public bool IsActive { get; set; }
    
    public int Sort { get; set; }
    
    public string Description { get; set; }
    
    public decimal Price { get; set; }
    
    public string CurrencyCode { get; set; }

    public List<ProductImage> Images { get; set; }
}

public class ProductImage
{
    public int Id { get; set; }
    public string? PreviewUrl { get; set; }
    public string? FullUrl { get; set; }
}