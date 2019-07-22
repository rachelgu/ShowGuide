using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using ShowGuide.Models;

namespace ShowGuide.Controllers
{
    public class FilmesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Filmes
        public ActionResult Index()
        {
            return View(db.Filmes.ToList());
        }

        // GET: Filmes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Filme filme = db.Filmes.Find(id);
            if (filme == null)
            {
                return HttpNotFound();
            }
            filme.Comentraios = filme.Comentraios.OrderByDescending(m => m.Data).ToList();
            return View(filme);
        }

        // GET: Filmes/Create
        public ActionResult Create()
        {
            ViewBag.Categorias = new SelectList(db.Categorias, "Id", "Nome");
            return View();
        }

        // POST: Filmes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Titulo,Descricao,DataLancamento,Elenco,TraillerLink")] Filme filme, List<int> Categorias)
        {
            if (ModelState.IsValid)
            {
                if (Categorias == null)
                {
                    Categorias = new List<int>();
                }
                //Para adicionar Lista de categorias aos filmes
                IQueryable<Categoria> categorias = db.Categorias.Where(a => Categorias.Any(aa => a.Id == aa));
                filme.Categorias = categorias.ToList();

                db.Filmes.Add(filme);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(filme);
        }

        // GET: Filmes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Filme filme = db.Filmes.Find(id);
            if (filme == null)
            {
                return HttpNotFound();
            }
            ViewBag.Categorias = new SelectList(db.Categorias, "Id", "Nome");
            ViewBag.sel_Categorias = filme.Categorias.Select(i => i.Id).ToList();
            return View(filme);
        }

        // POST: Filmes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Titulo,Descricao,DataLancamento,Elenco,TraillerLink")] Filme filme, List<int> Categorias)
        {
            if (ModelState.IsValid)
            {
                if (Categorias == null)
                {
                    Categorias = new List<int>();
                }

                //get current entry from db (db is context)
                var filmeEntry = db.Entry<Models.Filme>(filme);

                //define o filme como editado
                filmeEntry.State = EntityState.Modified;

                //carrega a lista de categorias existentes para a variavel filme
                filmeEntry.Collection(i => i.Categorias).Load();

                //Para adicionar Lista de categorias aos filmes
                IQueryable<Categoria> categorias = db.Categorias.Where(a => Categorias.Any(aa => a.Id == aa));
                //adicionamos as categorias selecionadas por o utilizador
                filme.Categorias = categorias.ToList();

                db.SaveChanges();
                return RedirectToAction("Details", new { id = filme.Id});
            }
            ViewBag.Categorias = new SelectList(db.Categorias, "Id", "Nome");
            ViewBag.sel_Categorias = Categorias;
            return View(filme);
        }

        // GET: Filmes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Filme filme = db.Filmes.Find(id);
            if (filme == null)
            {
                return HttpNotFound();
            }
            return View(filme);
        }

        // POST: Filmes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Filme filme = db.Filmes.Find(id);
            db.Filmes.Remove(filme);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateComentario([Bind(Include = "Id,Texto,FilmeId")] Comentario comentario)
        {
            comentario.UserId = User.Identity.GetUserId();
            comentario.Data = DateTime.Now;
            if (ModelState.IsValid)
            {
                db.Comentarios.Add(comentario);
                db.SaveChanges();
                return RedirectToAction("Details", new { id=comentario.FilmeId });
            }

            if(comentario.FilmeId > 0)
            {
                return RedirectToAction("Details", new { id = comentario.FilmeId });
            }else
            {
                return RedirectToAction("Index");
            }

                
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        protected void UpdateCategoriasEntity(ICollection<Categoria> categorias)
        {
            if (categorias == null) return;
            foreach (var categoria in categorias)
            {
                db.Entry(categoria).State = EntityState.Modified;
            }
        }
    }
}
