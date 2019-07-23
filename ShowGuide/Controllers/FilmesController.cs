using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
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
        public ActionResult Index(string search = "")
        {
            ViewBag.search = search;
            if (search.Equals("")) return View(db.Filmes.ToList());
            else return View(db.Filmes.Where(f => f.Titulo.Contains(search)).ToList());
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
        [Authorize(Roles = "Admin")] //dá permições ao Admin
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
        [Authorize(Roles = "Admin")] //dá permições ao Admin
        public ActionResult Create([Bind(Include = "Id,Titulo,Descricao,DataLancamento,Elenco,TraillerLink")] Filme filme, HttpPostedFileBase Image, List<int> Categorias)
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
                //adicionar Imagem
                if (Image != null)
                {
                    filme.ImageExtension = Path.GetExtension(Image.FileName);
                }
                db.Filmes.Add(filme);
                db.SaveChanges();
                if (Image != null)
                {
                    Image.SaveAs(Path.Combine(Server.MapPath("~/Imagens/" + filme.Id + filme.ImageExtension)));
                }
            return RedirectToAction("Details", new { id = filme.Id});
            }

            return View(filme);
        }

        // GET: Filmes/Edit/5
        [Authorize(Roles = "Admin")] //dá permições ao Admin
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
        [Authorize(Roles = "Admin")] //dá permições ao Admin
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Titulo,Descricao,DataLancamento,Elenco,TraillerLink,ImageExtension")] Filme filme, HttpPostedFileBase Image, List<int> Categorias)
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


                //Editar Imagem
                if (Image != null)
                {
                    if (System.IO.File.Exists(Server.MapPath("~/Imagens/" + filme.Id + filme.ImageExtension)))
                    {
                        System.IO.File.Delete(Server.MapPath("~/Imagens/" + filme.Id + filme.ImageExtension));
                    }
                    filme.ImageExtension = Path.GetExtension(Image.FileName);
                    Image.SaveAs(Path.Combine(Server.MapPath("~/Imagens/" + filme.Id + filme.ImageExtension)));
                }

                db.SaveChanges();
                return RedirectToAction("Details", new { id = filme.Id});
            }
            ViewBag.Categorias = new SelectList(db.Categorias, "Id", "Nome");
            ViewBag.sel_Categorias = Categorias;
            return View(filme);
        }

        // POST: Filmes/Delete/5
        [Authorize(Roles = "Admin")] //dá permições ao Admin
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            Filme filme = db.Filmes.Find(id);
            if(filme == null)
            {
                return HttpNotFound();
            }
            //remover todas as categorias associadas ao filme
            filme.Categorias.Clear();
            //eliminar todos os comentários associados ao filme
            List<Comentario> comentarios = filme.Comentraios.ToList();
            foreach (var comentario in comentarios)
            {
                db.Comentarios.Remove(comentario);
            }
            //Eliminar Imagem
            if (filme.ImageExtension != null)
            {
                if (System.IO.File.Exists(Server.MapPath("~/Imagens/" + filme.Id + filme.ImageExtension)))
                {
                    System.IO.File.Delete(Server.MapPath("~/Imagens/" + filme.Id + filme.ImageExtension));
                }
            }
            //eliminar o filme
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
    }
}
