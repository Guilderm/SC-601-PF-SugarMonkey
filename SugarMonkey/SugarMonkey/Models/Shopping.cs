using System.ComponentModel.DataAnnotations;

namespace OnlineShopping.Models
{
    public class ShippingDetails
    {
        public string OrderId { get; set; }

        [Required] public string Address { get; set; }

        [Required] public string City { get; set; }

        [Required] public string State { get; set; }

        [Required] public string Country { get; set; }

        [Required] public string ZipCode { get; set; }

        public decimal TotalPrice { get; set; }

        [Required] public string PaymentType { get; set; }
    }
}