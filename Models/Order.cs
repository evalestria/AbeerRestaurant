using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AbeerRestaurant.Models
{
    public class Order
    {
        [Key]
        public int ID { get; set; }

        [Required]
        public string UserId { get; set; }

        [Required]
        public string Items { get; set; }

        [DataType(DataType.Currency)]
        [Column(TypeName = "Money")]
        public decimal TotalPrice { get; set; }

        [Required]
        public DateTime OrderDate { get; set; } = DateTime.Now;
    }
}
