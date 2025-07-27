using System.Threading.Tasks;
using ApiEcommerce.Data;
using ApiEcommerce.Models;
using ApiEcommerce.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace ApiEcommerce.Repository;

public class CategoryRepository : ICategoryRepository
{

  private readonly ApplicationDbContext _db;

  public CategoryRepository(ApplicationDbContext db)
  {
    _db = db;
  }

  public async Task<bool> CategoryExists(int id)
  {
    return await _db.Categories.AnyAsync(c => c.Id == id);
  }

  public async Task<bool> CategoryExists(string name)
  {
    return await _db.Categories.AnyAsync(c => c.Name.ToLower().Trim() == name.ToLower().Trim());
  }

  public async Task<bool> CreateCategory(Category category)
  {
    category.CreationDate = DateTime.Now;
    await _db.Categories.AddAsync(category);
    return await Save();

  }

  public async Task<bool> DeleteCategory(Category category)
  {
    _db.Categories.Remove(category);
    return await Save();
  }

  public async Task<ICollection<Category>> GetCategories()
  {
    return await _db.Categories.OrderBy(c => c.Name).ToListAsync();
  }

  public async Task<Category?> GetCategory(int id)
  {
    return await _db.Categories.FirstOrDefaultAsync(c => c.Id == id);
  }

  public async Task<bool> Save()
  {
    return await _db.SaveChangesAsync() >= 0;

  }

  public async Task<bool> UpdateCategory(Category category)
  {
    category.CreationDate = DateTime.Now;
    // _db.Entry(category).State = EntityState.Modified;
    // var rowsAffected = await _db.SaveChangesAsync();
    _db.Categories.Update(category);
    return await Save();
  }
}