using Bulky.Data;
using BulkyWeb.Model.Models;
using BulkyWeb.Model.ViewModels;
using BulkyWeb.DataAccess.Repository;
using BulkyWeb.DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Hosting;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;

namespace Bulky.Controllers
{
        [Area("Admin")]
        public class ProductController : Controller
        {
            //private readonly ApplicationDbContext _db;
            private readonly IUnitOfWork _unitOfWork;
            private readonly IWebHostEnvironment _webHostEnvironment;

        public int Id { get; private set; }

        public ProductController(IUnitOfWork db, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = db;
            _webHostEnvironment = webHostEnvironment;
        }






        // ---------- INDEX ----------
        public IActionResult Index()
        {

            //List<Product> productList = _unitOfWork.Product.GetAll().ToList();
            List<Product> productList = _unitOfWork.Product.GetAll(includeProperties: "Category").ToList();
            return View(productList);
        }


        // ---------- CREATE (GET) ----------

        /*public IActionResult Create()
        {
            IEnumerable<SelectListItem> CategoryList = _unitOfWork.Category.GetAll().Select(c => new SelectListItem
            {
                Text = c.Name,
                Value = c.Id.ToString()
            });

            ProductVM ProductVM = new ProductVM
            {
                CategoryList = CategoryList,
                Product = new Product()
            };


            return View(ProductVM);
        }*/

        //----------Upsert (Get)---------------

        public IActionResult Upsert (int? Id) 
        {
            IEnumerable<SelectListItem> CategoryList = _unitOfWork.Category.GetAll().Select(c => new SelectListItem
            {
                Text = c.Name,
                Value = c.Id.ToString(),
            });

            ProductVM ProductVM = new ProductVM
            {
                CategoryList = CategoryList,
                Product = new Product()
            };

            if (Id == null || Id == 0)
            {
                // create
                return View(ProductVM);
            }
            else
            {
                // update
                ProductVM.Product = _unitOfWork.Product.Get(c => c.Id == Id);
                return View(ProductVM);
            }
          
        }

        //------------Create (Post)-------------

        /*[HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(ProductVM obj , IFormFile? file )
        {

            if ( file == null)
            {
                ModelState.AddModelError("ImageUrl", "PLease Upload Image for Product..");
            }

            if (ModelState.IsValid)
            {
                
                 //* wwwRoot path
                 //* file != null
                 //* new Name => GuId +extension
                 //* path => images/product
                 //* FileStream => create new file
                 //* file.copyTo(fileStream)
                 

                string wwwRootPath  = _webHostEnvironment.WebRootPath;
                if ( file != null)
                {
                    string filename = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    string productpath = Path.Combine(wwwRootPath, "image/product");

                    if (!Directory.Exists(productpath))
                    {
                        Directory.CreateDirectory(productpath);
                    }

                    using (var fileStream = new FileStream (Path.Combine(productpath, filename), FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }
                    obj.Product.ImageUrl = @"\image\product\" + filename;
                    }
                    else
                    {
                        // ✅ DEFAULT IMAGE (important)
                        obj.Product.ImageUrl = @"\image\product\default.png";

                    }

                _unitOfWork.Product.Add(obj.Product);
                _unitOfWork.Save();

                TempData["create"] = "Product Created Successfully!";
                return RedirectToAction(nameof(Index));
            }

            // 🔴 VERY IMPORTANT — repopulate dropdown
            obj.CategoryList = _unitOfWork.Category.GetAll().Select(c => new SelectListItem
            {
                Text = c.Name,
                Value = c.Id.ToString()
            });

            return View(obj);
        }
        */

        // ---------- Edit (get) ----------
        //public IActionResult Edit(int? id)
        //{
        //    if (id == null || id == 0)
        //    {
        //        return NotFound();
        //    }

        //    Product? ProductFromDb = _unitOfWork.Product.Get(c => c.Id == id);

        //    if (ProductFromDb == null)
        //    {
        //        return NotFound();
        //    }
        //    return View(ProductFromDb);
        //}

        /*public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            ProductVM productVM = new()
            {
                Product = _unitOfWork.Product.Get(u => u.Id == id),
                CategoryList = _unitOfWork.Category.GetAll().Select(c => new SelectListItem
                {
                    Text = c.Name,
                    Value = c.Id.ToString()
                })
            };

            if (productVM.Product == null)
            {
                return NotFound();
            }

            return View(productVM);
        }*/


        // ---------- Edit (Post)----------
        //[HttpPost]
        //public IActionResult Edit(Product ProductFromDb , IFormFile? file )
        //{
        //    if (file == null) 
        //    {
        //        string wwwRootPath = _webHostEnvironment.WebRootPath;
        //    }


        //    if (ModelState.IsValid)
        //    {
        //        _unitOfWork.Product.Update(ProductFromDb);
        //        _unitOfWork.Save();
        //        TempData["update"] = "Poduct Updated Successfully.";
        //        return RedirectToAction("Index");
        //    }
        //    return View();
        //}

        /*[HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(ProductVM obj, IFormFile? file)
        {
            if (ModelState.IsValid)
            {
                string wwwRootPath = _webHostEnvironment.WebRootPath;

                if (file != null)
                {
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    string productPath = Path.Combine(wwwRootPath, "image", "product");

                    if (!Directory.Exists(productPath))
                    {
                        Directory.CreateDirectory(productPath);
                    }

                    // Delete old image
                    if (!string.IsNullOrEmpty(obj.Product.ImageUrl))
                    {
                        string oldImagePath = Path.Combine(
                            wwwRootPath,
                            obj.Product.ImageUrl.TrimStart('\\')
                        );

                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }

                    using (var fileStream = new FileStream(
                        Path.Combine(productPath, fileName),
                        FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }

                    obj.Product.ImageUrl = @"\image\product\" + fileName;
                }
                // else → hidden ImageUrl keeps old image

                _unitOfWork.Product.Update(obj.Product);
                _unitOfWork.Save();

                TempData["update"] = "Product Updated Successfully.";
                return RedirectToAction("Index");
            }

            // Reload dropdown if validation fails
            obj.CategoryList = _unitOfWork.Category.GetAll().Select(c => new SelectListItem
            {
                Text = c.Name,
                Value = c.Id.ToString()
            });

            return View(obj);
        }*/


        //-----------Upsert (Post)------------

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(ProductVM obj, IFormFile? file)
        {
            if (ModelState.IsValid)
            {
                string wwwRootPath = _webHostEnvironment.WebRootPath;

                if (file != null)
                {
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    string productPath = Path.Combine(wwwRootPath, "image", "product");

                    if (!Directory.Exists(productPath))
                    {
                        Directory.CreateDirectory(productPath);
                    }

                    // Delete old image if editing
                    if (!string.IsNullOrEmpty(obj.Product.ImageUrl))
                    {
                        string oldImagePath = Path.Combine(
                            wwwRootPath,
                            obj.Product.ImageUrl.TrimStart('\\')
                        );

                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }

                    using (var fileStream = new FileStream(
                        Path.Combine(productPath, fileName),
                        FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }

                    obj.Product.ImageUrl = @"\image\product\" + fileName;
                }

                // 🔥 UPSERT LOGIC
                if (obj.Product.Id == 0)
                {
                    _unitOfWork.Product.Add(obj.Product);
                    TempData["create"] = "Product Created Successfully!";
                }
                else
                {
                    _unitOfWork.Product.Update(obj.Product);
                    TempData["update"] = "Product Updated Successfully!";
                }

                _unitOfWork.Save();
                return RedirectToAction(nameof(Index));
            }

            // Reload dropdown if validation fails
            obj.CategoryList = _unitOfWork.Category.GetAll().Select(c => new SelectListItem
            {
                Text = c.Name,
                Value = c.Id.ToString()
            });

            return View(obj);
        }


        // ---------- Delet (get) ----------

        //public IActionResult Delete(int? id)
        //{

        //    if (id == null || id == 0)
        //    {
        //        return NotFound();
        //    }

        //    Product? ProductFromDb = _unitOfWork.Product.Get(c => c.Id == id);

        //    if (ProductFromDb == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(ProductFromDb);
        //}


        // ---------- Delet (Post) ----------

        //[HttpPost, ActionName("Delete")]

        //public IActionResult DeletePOST(int? id)
        //{
        //    Product? ProductFromDb = _unitOfWork.Product.Get(c => c.Id == id);
        //    if (ProductFromDb == null)
        //    {
        //        return NotFound();
        //    }
        //    string wwwRootPath = _webHostEnvironment.WebRootPath;

        //    if (!string.IsNullOrEmpty(ProductFromDb.ImageUrl))
        //    {
        //        var imagePath = Path.Combine(
        //            wwwRootPath,
        //            ProductFromDb.ImageUrl.TrimStart('\\')
        //        );

        //        if (System.IO.File.Exists(imagePath))
        //        {
        //            System.IO.File.Delete(imagePath);
        //        }
        //    }

        //    _unitOfWork.Product.Remove(ProductFromDb);
        //    _unitOfWork.Save();
        //    TempData["delete"] = "Product deleted Successfully.";
        //    return RedirectToAction("Index");
        //}

        #region API CALLS
        [HttpGet]
        public IActionResult GetAll()
        {

            List<Product> productList = _unitOfWork.Product.GetAll(includeProperties: "Category").ToList();
            return Json(new {data = productList });
        }

        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var productToBeDelete = _unitOfWork.Product.Get(u => u.Id == id);

            if (productToBeDelete == null)
            {
                return Json(new { success = false, message = "Error while Deleteing" });
            }

            string wwwRootPath = _webHostEnvironment.WebRootPath;

           // Delete Image from Server
            if (!string.IsNullOrEmpty(productToBeDelete.ImageUrl))
            {
                var oldImagePath = Path.Combine(
                    wwwRootPath,
                    productToBeDelete.ImageUrl.TrimStart('\\')
                );

                if (System.IO.File.Exists(oldImagePath))
                {
                    System.IO.File.Delete(oldImagePath);
                }
            }



            // delete from database
            _unitOfWork.Product.Remove(productToBeDelete);
            _unitOfWork.Save();

            return Json(new
            {
                success = true,
                message = "Deleted Successfully!!!"
            });
        
            
         }

        #endregion
    }
}
