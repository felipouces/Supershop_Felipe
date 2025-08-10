using System;
using System.ComponentModel.DataAnnotations;

namespace Supershop.Data.Entities
{
    // This class represents a product entity in the Supershop application.

    public class Product : IEntity  // Assuming IEntity is an interface that defines an Id property
    {

        public int Id { get; set; }

        [Required]
        [MaxLength(50, ErrorMessage = "The field {0} can contain {1} characters lenght")]
        public string Name { get; set; } = string.Empty;

        [DisplayFormat(DataFormatString = "{0:C2}", ApplyFormatInEditMode = false)]
        public decimal Price { get; set; }

        [Display(Name = "Image")]
        public string ImageUrl { get; set; }

        [Display(Name = "Last Purchase")]
        public DateTime? LastPurchase { get; set; }

        [Display(Name = "Last Sale")]
        public DateTime? LastSale { get; set; }

        [Display(Name = "Is Available")]
        public bool IsAvailable { get; set; } = true;

        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = false)]
        public double Stock { get; set; } = 0.0;


        // Each product that is inserted will have to have a User
        public User User { get; set; }
        // Creating a one-to-many relationship. Because a user can have many products


        // Create read-only property, and this property also gives the image path
        public string ImageFullPatch
        {
            get
            {
                if (string.IsNullOrEmpty(ImageUrl))
                {
                    return null;
                }
                // If I had published it on Azure I would have put the full address, but in this case I'm putting the localhost address.
                return $"https://localhost:44325{ImageUrl.Substring(1)}"; 
            }
        }


    }
}
