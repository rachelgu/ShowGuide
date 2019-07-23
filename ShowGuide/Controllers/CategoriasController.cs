using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ShowGuide.Models;

namespace ShowGuide.Controllers
{
    [Authorize(Roles = "Admin")] //dá permições ao Admin
    public class CategoriasController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Categorias
        public ActionResult Index(string search = "")
        {
            //guarda a pesquisa no viewbag para a view poder saber que termos estão definidos
            ViewBag.search = search;
            //aplica o termo de pesquisa ao select a BD caso este esteja definido
            if (search.Equals("")) return View(db.Categorias.ToList());
            else return View(db.Categorias.Where(f => f.Nome.Contains(search)).ToList());
        }

        // GET: Categorias/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Categorias/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Nome")] Categoria categoria)
        {
            if (ModelState.IsValid)
            {
                db.Categorias.Add(categoria);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(categoria);
        }

        // GET: Categorias/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null) // se o id não estiver definido volta para a lista de categorias
            {
                return RedirectToAction("Index");
            }
            //procura a categoria
            Categoria categoria = db.Categorias.Find(id);
            if (categoria == null) //se a categoria não for encontrada volta para a lista de categorias
            {
                return RedirectToAction("Index");
            }
            return View(categoria);
        }

        // POST: Categorias/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Nome")] Categoria categoria)
        {
            if (ModelState.IsValid)
            {
                db.Entry(categoria).State = EntityState.Modified;
                db.SaveChanges();
                //volta para a lista de categorias
                return RedirectToAction("Index");
            }
            return View(categoria);
        }

        // POST: Categorias/Delete/5
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            Categoria categoria = db.Categorias.Find(id);
            if(categoria == null) //se a categoria não for encontrada, volta para  a lista de categorias
            {
                return RedirectToAction("Index");
            }
            db.Categorias.Remove(categoria);
            db.SaveChanges();
            //volta para a lista de categorias
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
