using BuyerService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BuyerService.BusinessLayer.Interfaces
{
    public interface IBuyerBusinessLogic
    {
        Task AddBid(BidAndBuyer bidDetails);
        Task UpdateBid(string productId, string buyerEmailId, double updatedBidAmount);
        Task IsBidPresentForProductIdAsync(string productId);
        void CollateResponseForQueue(string operation, string productId, bool isValid);
        Task GetAllBidDetailsAsync(string productId);
    }
}
