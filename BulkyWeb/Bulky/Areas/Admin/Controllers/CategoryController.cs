using Bulky.Data;
using BulkyWeb.Model.Models;
using BulkyWeb.DataAccess.Repository;
using BulkyWeb.DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;

namespace Bulky.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        //private readonly ApplicationDbContext _db;
        private readonly IUnitOfWork _unitOfWork;

        public int Id { get; private set; }

        public CategoryController(IUnitOfWork db)
        {
            _unitOfWork = db;   
        }


        // ---------- INDEX ----------
        public IActionResult Index()
        {

            List<Category> categoryList = _unitOfWork.Category.GetAll().ToList();
            return View(categoryList);
        }


        // ---------- CREATE (GET) ----------

        public IActionResult Create() 
        {

            return View();
        }

        //------------Create (Post)-------------

        [HttpPost]
        public IActionResult Create(Category CategoryObj)
        
        {
            if (CategoryObj.Name == CategoryObj.DisplayOrder.ToString())
            {
                ModelState.AddModelError("name", "Name and Display Order should be not Same");
            }



            if (ModelState.IsValid)
            {
                _unitOfWork.Category.Add(CategoryObj);
                _unitOfWork.Save();
                TempData["create"] = "Category Created Successfully..!!";
                return RedirectToAction("Index");
            }
            return View();
        }

        // ---------- Edit (get) ----------
        public IActionResult Edit( int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            Category? categoryFromDb = _unitOfWork.Category.Get( c => c.Id == id);

            if(categoryFromDb == null)
            {
                return NotFound();
            }
            return View(categoryFromDb);
        }

        // ---------- Edit (Post)----------
        [HttpPost]
        public IActionResult Edit(Category CategoryObj)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.Category.Update(CategoryObj);
                _unitOfWork.Save();
                TempData["update"] = "Category Updated Successfully.";
                return RedirectToAction("Index");
            }
            return View();
        }

        // ---------- Delet (get) ----------
        
        public IActionResult Delete(int? id)
        {

            if (id == null || id == 0)
            {
                return NotFound();
            }

            Category? categoryFromDb = _unitOfWork.Category.Get( c => c.Id == id);

            if (categoryFromDb == null)
            {
                return NotFound();
            }

            return View(categoryFromDb);
        }


        // ---------- Delet (Post) ----------

        [HttpPost , ActionName("Delete")]

        public IActionResult DeletePOST(int? id)
        {
            Category? categoryFromDb = _unitOfWork.Category.Get(c => c.Id == id); 
            if (categoryFromDb == null)
            {
                return NotFound();
            }
            _unitOfWork.Category.Remove(categoryFromDb);
            _unitOfWork.Save()  ;
            TempData["delete"] = "Category deleted Successfully.";
            return RedirectToAction("Index");
        }
    }
}
