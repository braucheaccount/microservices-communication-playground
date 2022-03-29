using Microsoft.EntityFrameworkCore;
using ProductService.Data;
using ProductService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductService.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ProductContext _context;

        public ProductRepository(ProductContext context)
        {
            _context = context;
        }

        public async Task<Product> CreateProductAsync(Product newProduct)
        {
            _context.Products.Add(newProduct);
            await _context.SaveChangesAsync();

            return newProduct;
        }

        public async Task DeleteProductAsync(Guid id)
        {
            var product = await _context.Products.FirstOrDefaultAsync(x => x.Id == id);
            
            if (product == null)
            {
                throw new ArgumentNullException($"product '{id}' is null");
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
        }

        public async Task<Product> FindProductByIdAsync(Guid id)
        {
            var product = await _context.Products.FirstOrDefaultAsync(x => x.Id == id);

            if (product == null)
            {
                throw new ArgumentNullException($"product '{id}' is null");
            }

            return product;
        }

        public async Task<IEnumerable<Product>> ListProductsAsync()
        {
            return await _context.Products.ToListAsync();
        }
    }
}
