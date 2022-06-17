using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System.ComponentModel.DataAnnotations;

namespace BuyerService.Models
{
    public class BidAndBuyer
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [Required(ErrorMessage = "Buyer first name is required")]
        [RegularExpression("[a-zA-Z]{5,30}", ErrorMessage = "Buyer first name should be string and minimum of length 5, maximum of length 30.")]
        public string BuyerFName { get; set; }

        [Required(ErrorMessage = "Buyer last name is required")]
        [RegularExpression("[a-zA-Z]{3,25}", ErrorMessage = "Buyer last name should be string and minimum of length 3, maximum of length 25.")]
        public string BuyerLName { get; set; }

        public string Address { get; set; }

        public string City { get; set; }

        public string State { get; set; }

        public string Pin { get; set; }

        [Required(ErrorMessage = "Phone number is reuired.")]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "The phone number should be numeric 10 digits.")]
        public int Phone { get; set; }

        [Required(ErrorMessage = "Buyer email is required")]
        [EmailAddress]
        public string Email { get; set; }

        public string ProductId { get; set; }

        public double BidAmount { get; set; }
    }
}
