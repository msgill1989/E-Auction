using BuyerService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BuyerService.RepositoryLayer.Interfaces
{
    public interface IBuyerRepository
    {
        Task AddBid(BidAndBuyer bidDetails);
        Task UpdateBid(string productId, string buyerEmailId, double bidAmount);
        Task<BidAndBuyer> GetBidDetails(string productId, string bidderEmailId = null);
        public List<BidAndBuyer> GetAllBidsForProductId(string productId);
        BidAndBuyer GetBidDetailsByBidId(string bidId);
    }
}
