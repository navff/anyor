using ProductStorage.CommonModels;

namespace ProductStorage.Abstract;

public interface IProductStorage
{
    Task<Product> GetProduct(long id);
}