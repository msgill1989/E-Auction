using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SellerService.BusinessLayer.Interfaces
{
    public interface ISellerBusinessLogic
    {
        void AddProduct(ProductAndSeller ProductObj);

        void DeleteProduct(string productId);
    }
}
