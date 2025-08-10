using Microsoft.AspNetCore.Http;
using Supershop.Data.Entities;
using System.ComponentModel.DataAnnotations;

namespace Supershop.Models
{
    public class ProductViewModel : Product
    {

        // This class is used to represent a product in the view, including the image file for upload.
        [Display(Name = "Image")]
        public IFormFile ImageFile { get; set; } = null!;
    }
    
}
