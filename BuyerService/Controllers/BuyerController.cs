using BuyerService.BusinessLayer.Interfaces;
using BuyerService.Enums;
using BuyerService.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BuyerService.Controllers
{
    [ApiController]
    [Route("e-auction/api/v1/[controller]")]
    public class BuyerController : Controller
    {
        private readonly IBuyerBusinessLogic _buyerBusinessLogic;
        private readonly ILogger<BuyerController> _logger;
        public BuyerController(IBuyerBusinessLogic buyerBusinessLogic, ILogger<BuyerController> logger)
        {
            _buyerBusinessLogic = buyerBusinessLogic;
            _logger = logger;
        }
        // POST: BuyerController/AddBid
        [HttpPost("place-bid")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<AddBidResponse>> AddBid([FromBody] BidAndBuyer bidDetails)
        {
            try
            {
                await _buyerBusinessLogic.AddBid(bidDetails);
                return new AddBidResponse { BidId = bidDetails.Id, Message = GlobalVariables.AddBidSuccessMsg };
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogError(ex.Message);
                var details = ProblemDetailsFactory.CreateProblemDetails(HttpContext, 400, "Bad request", null, ex.Message);
                return StatusCode(400, details);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Some internal error happened.");
                var details = ProblemDetailsFactory.CreateProblemDetails(HttpContext, 500, "Internal Server Error", null, "Error while adding the Bid.");
                return StatusCode(500, details);
            }
        }

        [HttpPatch("update-bid/{productId}/{buyerEmailId}/{newBidAmount}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<UpdateBidAmountSuccessResponse>> EditBid(string productId, string buyerEmailId, double newBidAmount)
        {
            try
            {
                await _buyerBusinessLogic.UpdateBid(productId, buyerEmailId, Convert.ToDouble(newBidAmount));
                return new UpdateBidAmountSuccessResponse { Message = "Amount has been successfully updated"};
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Some internal error happened.");
                var details = ProblemDetailsFactory.CreateProblemDetails(HttpContext, 500, "Internal Server Error", null, "Error while updating the bid amount of the product.");
                return StatusCode(500, details);
            }
        }
    }
}
