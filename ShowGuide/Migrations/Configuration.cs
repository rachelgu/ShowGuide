namespace ShowGuide.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using ShowGuide.Models;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<ShowGuide.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(ShowGuide.Models.ApplicationDbContext context)
        {
            var storeR = new RoleStore<IdentityRole>(context);
            var managerR = new RoleManager<IdentityRole>(storeR);

            if (!context.Roles.Any(r => r.Name == "Admin"))
            {
                var role = new IdentityRole { Name = "Admin" };

                managerR.Create(role);
            }
            if (!context.Roles.Any(r => r.Name == "Viewer"))
            {
                var role = new IdentityRole { Name = "Viewer" };

                managerR.Create(role);
            }

            List<ApplicationUser> users = new List<Models.ApplicationUser> {
                new ApplicationUser {Email = "admin@admin.adm", Nome = "Admin"},
                new ApplicationUser {Email = "dora_estrela97@ipt.pt", Nome = "Dora Fernandes"},
                new ApplicationUser {Email = "joao2000@ipt.pt", Nome = "João Manuel"},
                new ApplicationUser {Email = "tatianaPereira@ipt.pt", Nome = "Tatiana Pereira"},
            };
            /////////////////////////// USERS ///////////////////////////////////
            var store = new UserStore<ApplicationUser>(context);
            var manager = new UserManager<ApplicationUser>(store);
            /////////////////////////// ADMIN ///////////////////////////////////
            ApplicationUser us = users[0]; //Primeiro utilizador do seed da tabela de Users
            us.UserName = us.Email;
            if (!context.Users.Any(u => u.UserName == us.Email))
            {

                manager.Create(us, "Qwe123."); // Palavra passe do admin 
                manager.AddToRole(us.Id, "Admin");
            }
            //Os restantes users são views da aplicação web 
            for (int i = 1; i < users.Count(); i++)
            {
                ApplicationUser us2 = users[i];
                us2.UserName = us2.Email;
                if (!context.Users.Any(u => u.UserName == us2.Email))
                {
                    manager.Create(us2, "123Querty#");
                    manager.AddToRole(us2.Id, "Viewer");
                }
            }
            //começa os ids a 0 para o primeiro ser 1
            int id = 0;
            //Categorias
            var categorias = new List<Categoria>
            {
                new Categoria {Id=++id, Nome="Acção"},
                new Categoria {Id=++id, Nome="Aventura"},
                new Categoria {Id=++id, Nome="Ficção Científica"},
                new Categoria {Id=++id, Nome="Fantasia"},
                new Categoria {Id=++id, Nome="Terror"},
                new Categoria {Id=++id, Nome="Drama"},
                new Categoria {Id=++id, Nome="Animação"},
                new Categoria {Id=++id, Nome="Comédia"},
                new Categoria {Id=++id, Nome="Romance"}
            };
            categorias.ForEach(dd => context.Categorias.AddOrUpdate(d => d.Id, dd));
            context.SaveChanges();

            //começa os ids a 0 para o primeiro ser 1
            id = 0;
            //Filmes
            var filmes = new List<Filme>
            {
                new Filme {Id=++id, Titulo = "Black Panther",ImageExtension=".jpg", DataLancamento = new DateTime(2018,1,29) , Descricao = "Conheça a história de T'Challa, príncipe do reino de Wakanda, que perde o seu pai e viaja para os Estados Unidos, onde tem contato com os Vingadores. Entre as suas habilidades estão a velocidade, inteligência e os sentidos apurados.", Categorias = new List<Categoria>{categorias[1], categorias[2], categorias[3]}, Elenco="Chadwick Boseman, Michael B. Jordan, Lupita Nyong'o, Danai Gurira, Letitia Wright, Winston Duke, Angela Bassett, Forest Whitaker, Andy Serkis", TraillerLink="https://www.youtube.com/watch?v=xjDjIWPwcPU"},
                new Filme {Id=++id, Titulo = "The Wolverine",ImageExtension=".jpg", DataLancamento = new DateTime(2013,7,26) , Descricao = "Após matar Jean Grey para salvar a humanidade, Logan passou a viver sozinho na selva. Deprimido, ele é encontrado por uma jovem que foi enviada pelo seu pai adotivo Yashida, salvo por Logan anos atrás. Yashima deseja transferir seu fator de cura para Logan para que ele possa viver uma vida normal como mortal. Logan se recusa e acaba infectado por Víbora, uma mutante que é imune a qualquer veneno. Fragilizado, Logan acaba envolvido em um conflito que o obriga a enfrentar seus próprios demônios.", Categorias = new List<Categoria>{categorias[1], categorias[2], categorias[3]}, Elenco="Hugh Jackman, Liev Schreiber, Danny Huston, will.i.am, Lynn Collins  ", TraillerLink="https://www.youtube.com/watch?v=toLpchTUYk8"},
                new Filme {Id=++id, Titulo = "Transformers: The Last Knight",ImageExtension=".jpg", DataLancamento = new DateTime(2017,6,21) , Descricao = "Optimus Prime descobre que seu planeta natal, Cybertron, agora é um planeta morto e que ele foi o responsável por matá-lo. Ele encontra uma forma de trazer o planeta de volta a vida, mas primeiro precisa encontrar uma artefato que está na Terra.", Categorias = new List<Categoria>{categorias[1], categorias[2], categorias[3]}, Elenco="Mark Wahlberg,Josh Duhamel,Laura Haddock,Isabela Moner,Anthony Hopkins,John Turturro,Santiago Cabrera,Stanley Tucci,Liam Garrigan,Jerrod Carmichael,Mitch Pileggi", TraillerLink="https://www.youtube.com/watch?v=AntcyqJ6brc"},
                new Filme {Id=++id, Titulo = "Green Lantern",ImageExtension=".jpg", DataLancamento = new DateTime(2011,7,17) , Descricao = "", Categorias = new List<Categoria>{categorias[1], categorias[2], categorias[3]}, Elenco="Ryan Reynolds,Blake Lively,Peter Sarsgaard,Mark Strong,Angela Bassett,Tim Robbins", TraillerLink="https://www.youtube.com/watch?v=_axLoYlwwmU"},
                new Filme {Id=++id, Titulo = "Lights Out",ImageExtension=".jpg", DataLancamento = new DateTime(1968,1,18) , Descricao = "", Categorias = new List<Categoria>{categorias[4]}, Elenco="", TraillerLink="https://www.youtube.com/watch?v=6LiKKFZyhRU"},
                new Filme {Id=++id, Titulo = "Crimson Peak",ImageExtension=".jpg", DataLancamento = new DateTime(1968,1,18) , Descricao = "", Categorias = new List<Categoria>{categorias[7], categorias[5], categorias[6]}, Elenco="", TraillerLink="https://www.youtube.com/watch?v=6yAbFYbi8XU"},
                new Filme {Id=++id, Titulo = "A Cure for Wellness",ImageExtension=".jpg", DataLancamento = new DateTime(1968,1,18) , Descricao = "", Categorias = new List<Categoria>{categorias[1], categorias[2], categorias[9]}, Elenco="", TraillerLink="https://www.youtube.com/watch?v=JF1rLFCdewU"},
                new Filme {Id=++id, Titulo = "Jumanji: Welcome to the Jungle",ImageExtension=".jpg", DataLancamento = new DateTime(1968,1,18) , Descricao = "", Categorias = new List<Categoria>{categorias[1], categorias[2], categorias[9]}, Elenco="", TraillerLink="https://www.youtube.com/watch?v=2QKg5SZ_35I"},
                new Filme {Id=++id, Titulo = "Zathura: A Space Adventure",ImageExtension=".jpg", DataLancamento = new DateTime(1968,1,18) , Descricao = "", Categorias = new List<Categoria>{categorias[1], categorias[2], categorias[9]}, Elenco="", TraillerLink="https://www.youtube.com/watch?v=Whfg26yGPx4"},
                new Filme {Id=++id, Titulo = "Peter Rabbit",ImageExtension=".jpg", DataLancamento = new DateTime(1968,1,18) , Descricao = "", Categorias = new List<Categoria>{categorias[1], categorias[2], categorias[9]}, Elenco="", TraillerLink="https://www.youtube.com/watch?v=7Pa_Weidt08"},
                new Filme {Id=++id, Titulo = "Ratatouille",ImageExtension=".jpg", DataLancamento = new DateTime(1968,1,18) , Descricao = "", Categorias = new List<Categoria>{categorias[1], categorias[2], categorias[9]}, Elenco="", TraillerLink="https://www.youtube.com/watch?v=NgsQ8mVkN8w"},
                new Filme {Id=++id, Titulo = "Rio 2",ImageExtension=".jpg", DataLancamento = new DateTime(1968,1,18) , Descricao = "", Categorias = new List<Categoria>{categorias[1], categorias[2], categorias[9]}, Elenco="", TraillerLink="https://www.youtube.com/watch?v=leJuOObuCxM"},
            };
            filmes.ForEach(dd => context.Filmes.AddOrUpdate(d => d.Id, dd));
            context.SaveChanges();

            //começa os ids a 0 para o primeiro ser 1
            id = 0;
            //Filmes
            var comentarios = new List<Comentario>
            {
                new Comentario {Id=++id, Texto = "",FilmeId=1, Data = new DateTime(1968,1,18,22,33,33) , UserId =  manager.FindByEmail(users[1].Email).Id},
            };
            comentarios.ForEach(dd => context.Comentarios.AddOrUpdate(d => d.Id, dd));
            context.SaveChanges();
        }
    }
}
