using SellerService.RepositoryLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SellerService.Models;
using MongoDB.Driver;

namespace SellerService.RepositoryLayer
{
    public class SellerRepository : ISellerRepository
    {
        private readonly ILogger<SellerRepository> _logger;
        private readonly IOptions<EAuctionDatabaseSettings> _dbSettings;
        private readonly IMongoCollection<ProductAndSeller> _productCollection;
        public SellerRepository(ILogger<SellerRepository> logger, IOptions<EAuctionDatabaseSettings> DBSettings)
        {
            _logger = logger;
            _dbSettings = DBSettings;

            var mongoClient = new MongoClient(DBSettings.Value.ConnectionString);
            var mongoDatabase = mongoClient.GetDatabase(DBSettings.Value.DatabaseName);
            _productCollection = mongoDatabase.GetCollection<ProductAndSeller>
                (DBSettings.Value.SellerCollectionName);

        }
        public async Task AddProductAsync(ProductAndSeller productObj)
        {
            try
            {
                await _productCollection.InsertOneAsync(productObj);
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public async Task DeleteProductAsync(string productId)
        {
            try
            {
                await _productCollection.DeleteOneAsync(x => x.Id == productId);
            }
            catch (Exception)
            {

                throw;
            }
        }
        public async Task<ProductAndSeller?> GetProductAsync(string productId)
        {
            try
            {
                return await _productCollection.Find(x => x.Id == productId).FirstOrDefaultAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
