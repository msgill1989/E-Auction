using SellerService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SellerService.BusinessLayer.Interfaces
{
    public interface ISellerBusinessLogic
    {
        Task AddProductBLayerAsync(ProductAndSeller ProductObj);

        Task DeleteProductBLayerAsync(string productId);

        Task<ShowBidsResponse> GetAllBidDetailsAsync(string productId);
    }
}
