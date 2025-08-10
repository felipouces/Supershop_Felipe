using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Supershop.Data;
using Supershop.Data.Entities;
using Supershop.Helpers;
using Supershop.Models;

namespace Supershop.Controllers
{
    public class ProductsController : Controller
    {
        
        private readonly IProductRepository _productRepository;
        private readonly IUserHelper _userHelper;

        public ProductsController(IProductRepository productRepository,
            IUserHelper userHelper)
        {
            _productRepository = productRepository;
            _userHelper = userHelper;
        }

        // GET: Products
        /*public IActionResult Index()
        {
            return View(_productRepository.GetAll());
        }*/

        // GET: Products
        public IActionResult Index()
        {
            var products = _productRepository.GetAll()
                                             .OrderBy(p => p.Name); // ordena  ascendente
            return View(products);
        }


        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _productRepository.GetByIdAsync(id.Value);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Products/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("Id,Name,Price,ImageUrl,LastPurchase,LastSale,IsAvailable,Stock")] Product product)
        //public async Task<IActionResult> Create([Bind("Id,Name,Price,ImageUrl,LastPurchase,LastSale,IsAvailable,Stock, User")] ProductViewModel model)
        public async Task<IActionResult> Create(ProductViewModel model)
        {
            if (ModelState.IsValid)
            {
                // save the image path
                var path = string.Empty;

                if (model.ImageFile != null && model.ImageFile.Length > 0)
                // optional image upload
                {

                    // Generate a unique identifier for the image file
                    var guid = Guid.NewGuid().ToString();
                    // Get the file extension of the uploaded image
                    var file = $"{guid}.jpg";

                    // Build the path where you will record it
                    //path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\image\\products", model.ImageFile.FileName);
                    path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\image\\products", file);

                    // now I'm going to record
                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        await model.ImageFile.CopyToAsync(stream);
                    }

                    // and now I assign the path to the product
                    //build the path that I will put in the database
                    //path = $"~/image/products/{model.ImageFile.FileName}";
                    path = $"~/image/products/{file}";

                }

                // Create Method to convert ProductViewModel to Product
                var product = this.ToProduct(model, path);

                // Here we are setting the User property of the product to a specific user
                // In a real application, you might want to get the current logged-in user instead
                product.User = await _userHelper.GetUserByEmailAsync("felipe.g.sales1985@gmail.com");
                await _productRepository.CreateAsync(product);
                return RedirectToAction(nameof(Index));
            }
            //return View(product);
            return View(model);
        }

        // Method to convert ProductViewModel to Product
        private Product ToProduct(ProductViewModel model, string path)
        {
            return new Product
            {
                Id = model.Id,
                Name = model.Name,
                Price = model.Price,
                ImageUrl = path, // Use the path for the image URL
                LastPurchase = model.LastPurchase,
                LastSale = model.LastSale,
                IsAvailable = model.IsAvailable,
                Stock = model.Stock,
                User = model.User // Assuming User is a property in ProductViewModel
            };
        }

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _productRepository.GetByIdAsync(id.Value);
            if (product == null)
            {
                return NotFound();
            }

            var model = this.ToProductViewModel(product);
            return View(model);
        }

        private ProductViewModel ToProductViewModel(Product product)
        {
            return new ProductViewModel
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
                ImageUrl = product.ImageUrl,
                LastPurchase = product.LastPurchase,
                LastSale = product.LastSale,
                IsAvailable = product.IsAvailable,
                Stock = product.Stock,
                User = product.User // Assuming User is a property in Product
            };
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Price,ImageUrl,LastPurchase,LastSale,IsAvailable,Stock")] Product product)
        public async Task<IActionResult> Edit(ProductViewModel model)
        {
            /*if (id != product.Id)
            {
                return NotFound();
            }*/
            if (ModelState.IsValid)
            {
                try
                {
                    // save the image path
                    var path = model.ImageUrl; 
                    // Use the existing image URL if no new image is uploaded

                    if (model.ImageFile != null && model.ImageFile.Length > 0)
                    {

                        // Generate a unique identifier for the image file
                        var guid = Guid.NewGuid().ToString();
                        // Get the file extension of the uploaded image
                        var file = $"{guid}.jpg";

                        // Build the path where you will record it
                        path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\image\\products", file);

                        // now I'm going to record
                        using (var stream = new FileStream(path, FileMode.Create))
                        {
                            await model.ImageFile.CopyToAsync(stream);
                        }
                        // and now I assign the path to the product
                        //build the path that I will put in the database
                        path = $"~/image/products/{file}";
                    }

                    var product = this.ToProduct(model, path);

                    // Here we are setting the User property of the product to a specific user
                    product.User = await _userHelper.GetUserByEmailAsync("felipe.g.sales1985@gmail.com"); // Replace with the actual user email or logic to get the current user
                    await _productRepository.UpdateAsync(product);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (! await _productRepository.ExistAsync(model.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _productRepository.GetByIdAsync(id.Value);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            await _productRepository.DeleteAsync(product);
            return RedirectToAction(nameof(Index));
        }
    }
}
