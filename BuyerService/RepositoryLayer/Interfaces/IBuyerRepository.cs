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
        Task UpdateBid(string bidId, double amountToUpdate);
        Task<BidAndBuyer> GetBidDetails(string productId, string bidderEmailId);
    }
}
