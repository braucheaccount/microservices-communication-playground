using ProductService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductService.Repositories
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> ListProductsAsync();

        Task<Product> FindProductByIdAsync(Guid id);

        Task<Product> CreateProductAsync(Product newProduct);

        Task DeleteProductAsync(Guid id);

        Task<Product> UpdateProductAsync(Product updatedProduct);
    }
}
