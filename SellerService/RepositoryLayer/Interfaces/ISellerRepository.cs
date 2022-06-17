using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SellerService.RepositoryLayer.Interfaces
{
    interface ISellerRepository
    {
        Task AddProductAsync(ProductAndSeller productObj);
        Task DeleteProductAsync(string productId);
        Task GetProductAsync(string productId);
    }
}
