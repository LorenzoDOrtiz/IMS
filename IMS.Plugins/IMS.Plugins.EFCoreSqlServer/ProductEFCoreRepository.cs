﻿using IMS.CoreBusiness;
using IMS.UseCases.PluginInterfaces;
using Microsoft.EntityFrameworkCore;

namespace IMS.Plugins.EFCoreSqlServer;
public class ProductEFCoreRepository : IProductRepository
{
    private readonly IDbContextFactory<IMSContext> _contextFactory;

    public ProductEFCoreRepository(IDbContextFactory<IMSContext> contextFactory)
    {
        _contextFactory = contextFactory;
    }

    public async Task AddProductAsync(Product product)
    {
        using var db = _contextFactory.CreateDbContext();
        db.Products?.Add(product);
        FlagInentoryUnchanged(product, db);

        await db.SaveChangesAsync();
    }

    public async Task DeleteProductByIdAsync(int productId)
    {
        using var db = _contextFactory.CreateDbContext();
        var product = db.Products?.Find(productId);
        if (product is null) return;

        db.Products?.Remove(product);
        await db.SaveChangesAsync();

    }

    public async Task<Product?> GetProductByIdAsync(int productId)
    {
        using var db = _contextFactory.CreateDbContext();
        var product = await db.Products.Include(x => x.ProductInventories)
            .ThenInclude(x => x.Inventory)
            .FirstOrDefaultAsync(x => x.ProductId == productId);
        if (product is not null) return product;

        return new Product();
    }

    public async Task<IEnumerable<Product>> GetProductsByNameAsync(string name)
    {
        using var db = _contextFactory.CreateDbContext();
        return await db.Products.Where(x => x.ProductName.ToLower().IndexOf(name.ToLower()) >= 0).ToListAsync();
    }

    public async Task UpdateProductAsync(Product product)
    {
        using var db = _contextFactory.CreateDbContext();
        var prod = await db.Products.Include(x => x.ProductInventories)
            .FirstOrDefaultAsync(x => x.ProductId == product.ProductId);
        if (prod is not null)
        {
            prod.ProductName = product.ProductName;
            prod.Price = product.Price;
            prod.Quantity = product.Quantity;
            prod.ProductInventories = product.ProductInventories;
            FlagInentoryUnchanged(product, db);

            await db.SaveChangesAsync();
        }
    }

    private void FlagInentoryUnchanged(Product product, IMSContext db)
    {
        if (product?.ProductInventories != null & product.ProductInventories.Count() > 0)
        {
            foreach (var prodInv in product.ProductInventories)
            {
                if (prodInv.Inventory is not null)
                {
                    db.Entry(prodInv.Inventory).State = EntityState.Unchanged;
                }
            }
        }
    }
}