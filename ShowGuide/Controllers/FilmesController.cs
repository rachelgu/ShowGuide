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
        public ActionResult Index(int? filtroVistos = -1,string search = "")
        {
            //guarda a pesquisa no viewbag para a view poder saber que termos estão definidos
            ViewBag.search = search;
            ViewBag.filtroVistos = filtroVistos;
            //carrega querable de filmes
            IQueryable<Filme> list = db.Filmes;
            //se houver termo de pesquisa, filtra com esta
            if (!search.Equals("")) list = list.Where(f => f.Titulo.Contains(search));
            //se o utilizador estiver autenticado
            if (User.Identity.IsAuthenticated)
            {
                //carrega o id do utilizador
                string userId = User.Identity.GetUserId();
                if (filtroVistos == 0) //filtra por só filmes vistos por o utilizador
                {
                    list = list.Where(f => f.Utilizadores.Any(u => u.Id.Equals(userId)));
                }else if (filtroVistos == 1) //filtra por só filmes NÃO vistos por o utilizador
                {
                    list = list.Where(f => !f.Utilizadores.Any(u => u.Id.Equals(userId)));
                }
            }
            //devole lista de utilizadores com os filtros aplicados
            return View(list.ToList());
        }

        // GET: Filmes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null) //se não for passado o id, volta para a lista de filmes
            {
                return RedirectToAction("Index");
            }
            //carrega dados do filme
            Filme filme = db.Filmes.Find(id);
            if (filme == null)//se o filme não for encontrado volta para a lista de filmes
            {
                return RedirectToAction("Index");
            }
            //carrega os comentários ordenados por data
            filme.Comentraios = filme.Comentraios.OrderByDescending(m => m.Data).ToList();
            return View(filme);
        }

        // GET: Filmes/Create
        [Authorize(Roles = "Admin")] //dá permições ao Admin
        public ActionResult Create()
        {
            //carrega no viewbag para a view a lista de categorias
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
                //se não for passado lista de categorias
                if (Categorias == null)
                {
                    //carrega lista vazia
                    Categorias = new List<int>();
                }
                //Para adicionar Lista de categorias aos filmes
                IQueryable<Categoria> categorias = db.Categorias.Where(a => Categorias.Any(aa => a.Id == aa));
                filme.Categorias = categorias.ToList();
                //se for enviado Imagem
                if (Image != null)
                {
                    //guarda a extensão da imagem enviada
                    filme.ImageExtension = Path.GetExtension(Image.FileName);
                }
                //adiciona o filme
                db.Filmes.Add(filme);
                db.SaveChanges();
                //se for enviado, este tem de ser aqui porque o filme tem de ser adicionado primeiro
                if (Image != null)
                {
                    //guarda a imagem
                    Image.SaveAs(Path.Combine(Server.MapPath("~/Imagens/" + filme.Id + filme.ImageExtension)));
                }
                //redireciona para os detalhes do filme
                return RedirectToAction("Details", new { id = filme.Id});
            }
            //carrega no viewbag para a view a lista de categorias
            ViewBag.Categorias = new SelectList(db.Categorias, "Id", "Nome");
            //carrega no viewbag para a view a lista de categorias selecionadas
            ViewBag.sel_Categorias = Categorias;
            return View(filme);
        }

        // GET: Filmes/Edit/5
        [Authorize(Roles = "Admin")] //dá permições ao Admin
        public ActionResult Edit(int? id)
        {
            if (id == null) //se não foi passado Id, volta para a lista de filmes
            {
                return RedirectToAction("Index");
            }
            Filme filme = db.Filmes.Find(id);
            if (filme == null) //se não foi encontrado o filme, volta para a lista de filmes
            {
                return RedirectToAction("Index");
            }
            //carrega no viewbag para a view a lista de categorias
            ViewBag.Categorias = new SelectList(db.Categorias, "Id", "Nome");
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
                //se não for passado lista de categorias
                if (Categorias == null)
                {
                    //carrega lista vazia
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
                
                //se foi enviado Imagem
                if (Image != null)
                {
                    //se ja existe imagem para este filme
                    if (System.IO.File.Exists(Server.MapPath("~/Imagens/" + filme.Id + filme.ImageExtension)))
                    {
                        //apaga a imagem
                        System.IO.File.Delete(Server.MapPath("~/Imagens/" + filme.Id + filme.ImageExtension));
                    }
                    //guarda a nova extensao da nova imagem
                    filme.ImageExtension = Path.GetExtension(Image.FileName);
                    //guarda a nova imagem
                    Image.SaveAs(Path.Combine(Server.MapPath("~/Imagens/" + filme.Id + filme.ImageExtension)));
                }
                //guarda as alterações
                db.SaveChanges();
                //volta para os detalhes do filme
                return RedirectToAction("Details", new { id = filme.Id});
            }
            //carrega no viewbag para a view a lista de categorias
            ViewBag.Categorias = new SelectList(db.Categorias, "Id", "Nome");
            //carrega no viewbag para a view a lista de categorias selecionadas
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
            if (filme == null) //se não foi encontrado o filme, volta para a lista de filmes
            {
                return RedirectToAction("Index");
            }
            //remover todas as categorias associadas ao filme
            filme.Categorias.Clear();
            //eliminar todos os comentários associados ao filme
            List<Comentario> comentarios = filme.Comentraios.ToList();
            foreach (var comentario in comentarios)
            {
                db.Comentarios.Remove(comentario);
            }
            //Eliminar Imagem caso esta exista
            if (filme.ImageExtension != null)
            {
                //verifica se a imagem existe
                if (System.IO.File.Exists(Server.MapPath("~/Imagens/" + filme.Id + filme.ImageExtension)))
                {
                    //apaga a imagem
                    System.IO.File.Delete(Server.MapPath("~/Imagens/" + filme.Id + filme.ImageExtension));
                }
            }
            //eliminar o filme
            db.Filmes.Remove(filme);
            //guarda as alterações
            db.SaveChanges();
            return RedirectToAction("Index");
        }


        //POST: Filmes/CreateComentario
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateComentario([Bind(Include = "Id,Texto,FilmeId")] Comentario comentario)
        {
            //define o utilizador que está a comentar
            comentario.UserId = User.Identity.GetUserId();
            //define a data do comentário como a data atual
            comentario.Data = DateTime.Now;
            if (ModelState.IsValid)
            {
                //adiciona o novo comentário
                db.Comentarios.Add(comentario);
                //guarda as alterações
                db.SaveChanges();
                //volta para os detalhes do filme que foi comentado
                return RedirectToAction("Details", new { id=comentario.FilmeId });
            }
            //caso ocorra algum erro, e se o id do filme estiver definido
            if(comentario.FilmeId > 0)
            {
                //volta para a página de detalhes do filme
                return RedirectToAction("Details", new { id = comentario.FilmeId });
            }else //caso o id não esteja definido
            {
                //volta para a lista de filmes
                return RedirectToAction("Index");
            }   
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ToggleViewed([Bind(Include = "Id")] Filme filme, int filtroVistos = -1, string search = "")
        {
            //carrega os dados do filme com os que estão na BD
            filme = db.Filmes.Find(filme.Id);
            //se o filme for encontrado
            if(filme != null)
            {
                //carrega o utilizador autenticado
                ApplicationUser user = db.Users.Find(User.Identity.GetUserId());
                //adicionamos o utilizador caso este filme não esteja atualmente como visto
                if (!filme.Utilizadores.Contains(user))
                {
                    filme.Utilizadores.Add(user);
                }
                else
                {//removemos o utilizador caso este filme esteja atualmente como visto
                    filme.Utilizadores.Remove(user);
                }
                //define o filme como editado
                db.Entry(filme).State = EntityState.Modified;
                //guarda as alterações
                db.SaveChanges();
            }
            //volta para a lista de filmes com os filtros de pesquisa aplicados
            return RedirectToAction("Index",new { filtroVistos , search});
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
