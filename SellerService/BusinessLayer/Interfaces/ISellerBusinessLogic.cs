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
        void IsBidPresentForProductId(string message);
        Task<ShowBidsResponse> GetAllBidDetailsAsync(string productId);
        Task IsBidDateValidAsync(ValidateDateRequest request);
        void CollateBidsResponse(string productId, List<BidDetails> bids);
    }
}
