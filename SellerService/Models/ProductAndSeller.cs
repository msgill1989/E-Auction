using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SellerService.Enums;
using System.ComponentModel.DataAnnotations;
using SellerService.CustomValidators;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace SellerService
{
    public class ProductAndSeller
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("Name")]
        [Required(ErrorMessage = "Product name is required.")]
        [RegularExpression("[a-zA-Z]{3,30}", ErrorMessage = "Product name should be string and minimum of length 3, maximum of length 30.")]
        public string ProductName { get; set; }

        public string ProductShortDescription { get; set; }

        public string ProductDetailedDescription { get; set; }

        [EnumDataType(typeof(GlobalVariables.ProductCategory), ErrorMessage = "Product category should one of these : Painting, Sculptor, Ornament.")]
        public string ProductCategory { get; set; }

        public double ProductStartingPrice { get; set; }

        [BidDateValidator(ErrorMessage = "Bid end date should be a future date.")]
        public DateTime BidEndDate { get; set; }

        [Required(ErrorMessage = "Seller first name is required.")]
        [RegularExpression("[a-zA-Z]{5,30}", ErrorMessage = "Seller First name should be string and minimum of length 5, maximum of length 30.")]
        public string SellerFName { get; set; }

        [Required(ErrorMessage = "Seller last name is required.")]
        [RegularExpression("[a-zA-Z]{3,25}", ErrorMessage = "Seller last name should be string and minimum of length 3, maximum of length 25.")]
        public string SellerLName { get; set; }

        public string Address { get; set; }

        public string City { get; set; }

        public string State { get; set; }

        public string Pin { get; set; }

        [Required(ErrorMessage = "Phone number is reuired.")]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "The phone number should be numeric 10 digits.")]
        public Int64 Phone { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Email address should be in proper format.")]
        public string Email { get; set; }
    }
}
