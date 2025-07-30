using System;
using ApiEcommerce.Data;
using ApiEcommerce.Model;
using ApiEcommerce.Repository.IRepository;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace ApiEcommerce.Repository;

public class ProductRepository : IProductRepository
{

  private readonly ApplicationDbContext _dbContext;


  public ProductRepository(ApplicationDbContext dbContext)
  {
    _dbContext = dbContext;

  }

  public async Task<bool> BuyProduct(int productQuantity, string productName)
  {
    if (string.IsNullOrWhiteSpace(productName) || productQuantity <= 0)
    {
      return false;
    }
    var product = await _dbContext.Products.FirstOrDefaultAsync(c => c.Name.ToLower().Trim() == productName.ToLower().Trim());
    if (product == null || product.Stock < productQuantity)
    {
      return false;
    }

    product.Stock -= productQuantity;
    _dbContext.Update(product);
    return await Save();

  }

  public async Task<bool> CreateProduct(Product product)
  {
    if (product == null)
    {
      return false;
    }
    product.CreationDate = DateTime.Now;
    product.UpdateDate = DateTime.Now;
    await _dbContext.AddAsync(product);
    return await Save();

  }

  public async Task<bool> DeleteProduct(Product product)
  {

    if (product == null)
    {
      return false;
    }

    var exists = await _dbContext.Products.AnyAsync(p => p.ProductId == product.ProductId);
    if (!exists)
    {
      return false;
    }
    _dbContext.Remove(product);
    return await Save();

  }

  public async Task<Product?> GetProduct(int productId)
  {
    if (productId <= 0)
      return null;

    return await _dbContext.Products.FirstOrDefaultAsync(p => p.ProductId == productId);
  }

  public async Task<ICollection<Product>> GetProducts()
  {
    return await _dbContext.Products.OrderBy(p => p.Name).ToListAsync();
  }

  public async Task<ICollection<Product>> GetProductsForCategory(int categoryId)
  {
    if (categoryId <= 0)
    {
      return new List<Product>();
    }
    return await _dbContext.Products.Where(p => p.CategoryId == categoryId).ToListAsync();
  }

  public async Task<bool> ProductExists(int productId)
  {
    if (productId <= 0)
    {
      return false;
    }
    return await _dbContext.Products.AnyAsync(p => p.ProductId == productId);
  }

  public async Task<bool> ProductExists(string productName)
  {
    if (string.IsNullOrWhiteSpace(productName))
    {
      return false;
    }
    return await _dbContext.Products.AnyAsync(p => p.Name.ToLower().Trim() == productName.ToLower().Trim());
  }

  public async Task<bool> Save()
  {
    return await _dbContext.SaveChangesAsync() >= 0;
  }

  public async Task<ICollection<Product>> SearchProduct(string productName)
  {
    IQueryable<Product> query = _dbContext.Products;
    if (!string.IsNullOrEmpty(productName))
    {
      query = query.Where(p => p.Name.ToLower().Trim() == productName.ToLower().Trim());
    }
    return await query.OrderBy(p => p.Name).ToListAsync();
  }

  public async Task<bool> UpdateProduct(Product product)
  {
    if (product == null) return false;
    product.UpdateDate = DateTime.Now;
    _dbContext.Update(product);
    return await Save();
  }
}
