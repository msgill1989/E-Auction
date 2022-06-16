using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SellerService.RepositoryLayer.Interfaces
{
    interface ISellerRepository
    {
        void AddProduct(ProductAndSeller productObj);
        void DeleteProduct(string productId);
    }
}
