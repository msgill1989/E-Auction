using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BuyerService.Models;
using BuyerService.RepositoryLayer.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace BuyerService.RepositoryLayer
{
    public class BuyerRepository : IBuyerRepository
    {
        private readonly ILogger<BuyerRepository> _logger;
        private readonly IOptions<EAuctionDatabaseSettings> _dbSettings;
        private readonly IMongoCollection<BidAndBuyer> _bidCollections;
        public BuyerRepository(ILogger<BuyerRepository> logger, IOptions<EAuctionDatabaseSettings> DBSettings)
        {
            _logger = logger;
            _dbSettings = DBSettings;

            var mongoClient = new MongoClient(_dbSettings.Value.ConnectionString);
            var mongoDatabase = mongoClient.GetDatabase(_dbSettings.Value.DatabaseName);

            _bidCollections = mongoDatabase.GetCollection<BidAndBuyer>
                (_dbSettings.Value.BuyerCollectionName);
        }
        public async Task AddBid(BidAndBuyer bidDetails)
        {
            try
            {
                await _bidCollections.InsertOneAsync(bidDetails);
            }
            catch (Exception)
            {

                throw;
            }
        }
        public async Task UpdateBid(string bidId, double amountToUpdate)
        {
            try
            {
                var earlierBidDetails = await _bidCollections.Find(x => x.Id == bidId).FirstOrDefaultAsync();
                earlierBidDetails.BidAmount = amountToUpdate;
                await _bidCollections.ReplaceOneAsync(y => y.Id == bidId, earlierBidDetails);
            }
            catch (Exception)
            {

                throw;
            }
        }
        public async Task<BidAndBuyer> GetBidDetails(string productId, string bidderEmailId)
        {
            try
            {
                var bidDetails = _bidCollections.Find(x => x.Email == bidderEmailId && x.ProductId == productId).FirstOrDefault();
                return bidDetails;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
