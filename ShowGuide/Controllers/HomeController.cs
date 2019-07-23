using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ShowGuide.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return RedirectToActionPermanent("Index","Filmes");//a página por defeito é a página da lista de filmes
        }

        public ActionResult About()
        {
            return View();
        }
        
    }
}