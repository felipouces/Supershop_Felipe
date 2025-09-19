using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
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
    //[Authorize]  Ensure the user is authenticated to access any action in this controller
    // The [Authorize] attribute can be applied at the controller level to enforce authentication for all actions within the controller.
    // If you want to allow anonymous access to specific actions, you can use the [AllowAnonymous] attribute on those actions.

    public class ProductsController : Controller
    {
        
        private readonly IProductRepository _productRepository;
        private readonly IUserHelper _userHelper;
        private readonly IImageHelper _imageHelper;
        private readonly IConverterHelper _converterHelper;

        public ProductsController(IProductRepository productRepository,
            IUserHelper userHelper,
            IImageHelper imageHelper,
            IConverterHelper converterHelper)
        {
            _productRepository = productRepository;
            _userHelper = userHelper;
            _imageHelper = imageHelper;
            _converterHelper = converterHelper;
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
        [Authorize] // Ensure the user is authenticated to access this action
        public async Task<IActionResult> Create(ProductViewModel model)
        {
            if (ModelState.IsValid)
            {
                // save the image path
                var path = string.Empty;

                if (model.ImageFile != null && model.ImageFile.Length > 0)
                // optional image upload
                {
                    // Call the UploadImageAsync method from the ImageHelper class to upload the image and get the path
                    path = await _imageHelper.UploadImageAsync(model.ImageFile, "products");

                }

              
                var product =  _converterHelper.ToProduct(model, path, true);

                // Here we are setting the User property of the product to a specific user
                // In a real application, you might want to get the current logged-in user instead
                //product.User = await _userHelper.GetUserByEmailAsync("felipe.g.sales1985@gmail.com");
                product.User = await _userHelper.GetUserByEmailAsync(this.User.Identity.Name); // Get the current logged-in user
                await _productRepository.CreateAsync(product);
                return RedirectToAction(nameof(Index));
            }
            //return View(product);
            return View(model);
        }

        // Method to convert ProductViewModel to Product
        /*private Product ToProduct(ProductViewModel model, string path)
        {
            return new Product
            {
                Id = model.Id,
                Name = model.Name,
                Price = model.Price,
                ImageUrl = path,  Use the path for the image URL
                LastPurchase = model.LastPurchase,
                LastSale = model.LastSale,
                IsAvailable = model.IsAvailable,
                Stock = model.Stock,
                User = model.User  Assuming User is a property in ProductViewModel
            };
        }*/

        // GET: Products/Edit/5
        [Authorize] // Ensure the user is authenticated to access this action
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

            
            var model = _converterHelper.ToProductViewModel(product);

            return View(model);
        }

        /*private ProductViewModel ToProductViewModel(Product product)
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
                User = product.User  Assuming User is a property in Product
            };
        }*/

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

                        path = await _imageHelper.UploadImageAsync(model.ImageFile, "products");
                    }

                    var product = _converterHelper.ToProduct(model, path, false);

                    // Here we are setting the User property of the product to a specific user
                   // product.User = await _userHelper.GetUserByEmailAsync("felipe.g.sales1985@gmail.com");
                    product.User = await _userHelper.GetUserByEmailAsync(this.User.Identity.Name); // Get the current logged-in user
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
        [Authorize] // Ensure the user is authenticated to access this action
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
