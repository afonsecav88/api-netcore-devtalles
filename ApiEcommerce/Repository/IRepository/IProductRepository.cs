using System;
using ApiEcommerce.Model;

namespace ApiEcommerce.Repository.IRepository;

public interface IProductRepository
{
  Task<ICollection<Product>> GetProducts();
  Task<ICollection<Product>> GetProductsForCategory(int categoryId);
  Task<ICollection<Product>> SearchProduct(string productName);
  Task<Product?> GetProduct(int productId);
  Task<bool> BuyProduct(int productQuantity, string productName);
  Task<bool> ProductExists(int productId);
  Task<bool> ProductExists(string productName);
  Task<bool> CreateProduct(Product product);
  Task<bool> UpdateProduct(Product product);
  Task<bool> DeleteProduct(Product product);
  Task<bool> Save();

}
