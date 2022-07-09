﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SellerService.BusinessLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SellerService.Models;
using SellerService.Enums;

namespace SellerService.Controllers
{
    [ApiController]
    public class SellerController : Controller
    {
        private readonly ISellerBusinessLogic _sellerBusinessLogic;
        private readonly ILogger<SellerController> _logger;
        public SellerController(ISellerBusinessLogic sellerBusinessLogic, ILogger<SellerController> logger)
        {
            _sellerBusinessLogic = sellerBusinessLogic;
            _logger = logger;
        }

        [HttpPost("AddProduct")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<AddProductSuccessResponse>> AddProduct([FromBody] ProductAndSeller productToAdd)
        {
            try
            {
                await _sellerBusinessLogic.AddProductBLayerAsync(productToAdd);
                return new AddProductSuccessResponse { ProductId = productToAdd.Id, Message = GlobalVariables.AddProductSuccessMsg };

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Some internal error happened.");
                var details = ProblemDetailsFactory.CreateProblemDetails(HttpContext, 500, "Internal Server Error", null, "Error while adding the product.");
                return StatusCode(500, details);
            }
        }

        // DELETE product
        [HttpDelete("DeleteProduct")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<DeleteProductSuccessResponse>> DeleteProduct([FromQuery]string productId)
        {
            try
            {
                await _sellerBusinessLogic.DeleteProductBLayerAsync(productId);
                return new DeleteProductSuccessResponse { Message = "Successfully Deleted" };
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
                var details = ProblemDetailsFactory.CreateProblemDetails(HttpContext, 500, "Internal Server Error", null, "Error while Deleting the product.");
                return StatusCode(500, details);
            }
        }

        [HttpGet("ShowBids")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ShowBidsResponse>> ShowBids([FromQuery] string productId)
        {
            try
            {
                var result = _sellerBusinessLogic.GetAllBidDetailsAsync(productId);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Some internal error happened.");
                var details = ProblemDetailsFactory.CreateProblemDetails(HttpContext, 500, "Internal Server Error", null, "Error while getting bid details.");
                return StatusCode(500, details);
            }
        }
    }
}
