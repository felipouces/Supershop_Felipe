using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Supershop.Data.Entities
{
    public class OrderDetailsTemp : IEntity
    {

        public int Id { get; set; }

        [ForeignKey("UserId")]
        public string UserId { get; set; }
        [Required]
        public User User { get; set; }
        //public int User { get; set; }


        [Required]
        public Product Product { get; set; }

        [DisplayFormat(DataFormatString = "{0:C2}")]
        public decimal Price { get; set; }


        [DisplayFormat(DataFormatString = "{0:N2}")]
        public double Quantity { get; set; }

        public decimal Value => Price * (decimal)Quantity;

    }
}
