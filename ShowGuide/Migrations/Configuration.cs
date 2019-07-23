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
                new ApplicationUser {Email = "joao2000@ipt.pt", Nome = "Jo�o Manuel"},
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
            //Os restantes users s�o views da aplica��o web 
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
            //come�a os ids a 0 para o primeiro ser 1
            int id = 0;
            //Categorias
            var categorias = new List<Categoria>
            {
                new Categoria {Id=++id, Nome="Ac��o"},
                new Categoria {Id=++id, Nome="Aventura"},
                new Categoria {Id=++id, Nome="Fic��o Cient�fica"},
                new Categoria {Id=++id, Nome="Fantasia"},
                new Categoria {Id=++id, Nome="Terror"},
                new Categoria {Id=++id, Nome="Drama"},
                new Categoria {Id=++id, Nome="Anima��o"},
                new Categoria {Id=++id, Nome="Com�dia"},
                new Categoria {Id=++id, Nome="Romance"}
            };
            categorias.ForEach(dd => context.Categorias.AddOrUpdate(d => d.Id, dd));
            context.SaveChanges();

            //come�a os ids a 0 para o primeiro ser 1
            id = 0;
            //Filmes
            var filmes = new List<Filme>
            {
                new Filme {Id=++id, Titulo = "Black Panther",ImageExtension=".jpg", DataLancamento = new DateTime(2018,1,29) , Descricao = "Conhe�a a hist�ria de T'Challa, pr�ncipe do reino de Wakanda, que perde o seu pai e viaja para os Estados Unidos, onde tem contato com os Vingadores. Entre as suas habilidades est�o a velocidade, intelig�ncia e os sentidos apurados.", Categorias = new List<Categoria>{categorias[0], categorias[1], categorias[2]}, Elenco="Chadwick Boseman, Michael B. Jordan, Lupita Nyong'o, Danai Gurira, Letitia Wright, Winston Duke, Angela Bassett, Forest Whitaker, Andy Serkis", TraillerLink="https://www.youtube.com/watch?v=xjDjIWPwcPU"},
                new Filme {Id=++id, Titulo = "The Wolverine",ImageExtension=".jpg", DataLancamento = new DateTime(2013,7,26) , Descricao = "Ap�s matar Jean Grey para salvar a humanidade, Logan passou a viver sozinho na selva. Deprimido, ele � encontrado por uma jovem que foi enviada pelo seu pai adotivo Yashida, salvo por Logan anos atr�s. Yashima deseja transferir seu fator de cura para Logan para que ele possa viver uma vida normal como mortal. Logan se recusa e acaba infectado por V�bora, uma mutante que � imune a qualquer veneno. Fragilizado, Logan acaba envolvido em um conflito que o obriga a enfrentar seus pr�prios dem�nios.", Categorias = new List<Categoria>{categorias[0], categorias[1], categorias[2]}, Elenco="Hugh Jackman, Liev Schreiber, Danny Huston, will.i.am, Lynn Collins  ", TraillerLink="https://www.youtube.com/watch?v=toLpchTUYk8"},
                new Filme {Id=++id, Titulo = "Transformers: The Last Knight",ImageExtension=".jpg", DataLancamento = new DateTime(2017,6,21) , Descricao = "Optimus Prime descobre que seu planeta natal, Cybertron, agora � um planeta morto e que ele foi o respons�vel por mat�-lo. Ele encontra uma forma de trazer o planeta de volta a vida, mas primeiro precisa encontrar uma artefato que est� na Terra.", Categorias = new List<Categoria>{categorias[0], categorias[1], categorias[2]}, Elenco="Mark Wahlberg,Josh Duhamel,Laura Haddock,Isabela Moner,Anthony Hopkins,John Turturro,Santiago Cabrera,Stanley Tucci,Liam Garrigan,Jerrod Carmichael,Mitch Pileggi", TraillerLink="https://www.youtube.com/watch?v=AntcyqJ6brc"},
                new Filme {Id=++id, Titulo = "Green Lantern",ImageExtension=".jpg", DataLancamento = new DateTime(2011,7,17) , Descricao = "Hal Jordan, um vaidoso piloto de testes, recebe um poderoso anel e � recrutado por um esquadr�o intergal�ctico chamado Lanterna Verde para lutar contra um feroz inimigo que amea�a acabar com o equil�brio do universo.", Categorias = new List<Categoria>{categorias[0], categorias[1], categorias[2]}, Elenco="Ryan Reynolds,Blake Lively,Peter Sarsgaard,Mark Strong,Angela Bassett,Tim Robbins", TraillerLink="https://www.youtube.com/watch?v=_axLoYlwwmU"},
                new Filme {Id=++id, Titulo = "Lights Out",ImageExtension=".jpg", DataLancamento = new DateTime(2016,6,8) , Descricao = "Rebecca saiu de casa e pensou que iria esquecer seus medos da inf�ncia, j� que nunca soube o que era real e o que n�o quando as luzes se apagavam. Agora, seu irm�o Martin vive os mesmos eventos que j� testaram a sanidade de Rebecca.", Categorias = new List<Categoria>{categorias[3]}, Elenco="Teresa Palmer,Gabriel Bateman,Billy Burke,Maria Bello", TraillerLink="https://www.youtube.com/watch?v=6LiKKFZyhRU"},
                new Filme {Id=++id, Titulo = "Crimson Peak",ImageExtension=".jpg", DataLancamento = new DateTime(2015,10,9) , Descricao = "Edith se casa com o sedutor Sir Thomas Sharpe e vai morar em uma remota mans�o g�tica. L�, tamb�m vive a misteriosa Lady Lucille, irm� de Thomas. A casa � assombrada e Edith decide investigar as apari��es fantasmag�ricas. � medida que se aproxima da verdade, a jovem percebe que os verdadeiros monstros s�o feitos de carne e osso.", Categorias = new List<Categoria>{categorias[6], categorias[4], categorias[5]}, Elenco="Mia Wasikowska,Jessica Chastain,Tom Hiddleston,Charlie Hunnam,Jim Beaver", TraillerLink="https://www.youtube.com/watch?v=6yAbFYbi8XU"},
                new Filme {Id=++id, Titulo = "A Cure for Wellness",ImageExtension=".jpg", DataLancamento = new DateTime(2017,4,6) , Descricao = "Um ambicioso executivo � enviado para os Alpes Su��os para resgatar o CEO de sua companhia de um 'Centro de Cura', mas logo descobre que o local n�o � t�o in�cuo quanto parece.", Categorias = new List<Categoria>{categorias[0], categorias[1], categorias[8]}, Elenco="Dane DeHaan,Jason Isaacs,Mia Goth", TraillerLink="https://www.youtube.com/watch?v=JF1rLFCdewU"},
                new Filme {Id=++id, Titulo = "Jumanji: Welcome to the Jungle",ImageExtension=".jpg", DataLancamento = new DateTime(2017,12,21) , Descricao = "Quatro adolescentes encontram um videogame cuja a��o se passa em uma floresta tropical. Empolgados com o jogo, eles escolhem seus avatares para o desafio, mas um evento inesperado faz com que eles sejam transportados para dentro do universo fict�cio, transformando-os nos personagens da aventura.", Categorias = new List<Categoria>{categorias[0], categorias[1], categorias[8]}, Elenco="Dwayne Johnson,Kevin Hart,Jack Black,Karen Gillan,Nick Jonas", TraillerLink="https://www.youtube.com/watch?v=2QKg5SZ_35I"},
                new Filme {Id=++id, Titulo = "Zathura: A Space Adventure",ImageExtension=".jpg", DataLancamento = new DateTime(2005,11,8) , Descricao = "Dois meninos, Walter e Danny , ficam em casa sob os cuidados de sua irm� adolescente, Lisa, quando o pai tem que ir trabalhar. eles, que achavam que o dia seria chato, ficam chocados ao come�arem a jogar Zathura, um jogo de tabuleiro de tem�tica espacial. Eles descobrem que o jogo tem poderes m�sticos quando sua casa � baleada no espa�o. Com a ajuda de um astronauta, os meninos tentam voltar para casa.", Categorias = new List<Categoria>{categorias[0], categorias[1], categorias[8]}, Elenco="Josh Hutcherson,Jonah Bobo,Dax Shepard,Kristen Stewart,Tim Robbins", TraillerLink="https://www.youtube.com/watch?v=Whfg26yGPx4"},
                new Filme {Id=++id, Titulo = "Peter Rabbit",ImageExtension=".jpg", DataLancamento = new DateTime(2018,2,3) , Descricao = "Pedro Coelho � um animal rebelde que apronta todas no quintal e at� dentro da casa do Mr. McGregor, com quem trava uma dura batalha pelo carinho e aten��o da amante de animais Bea.", Categorias = new List<Categoria>{categorias[0], categorias[1], categorias[8]}, Elenco="Rose Byrne,Domhnall Gleeson,Sam Neill,Daisy Ridley,Elizabeth Debicki,Margot Robbie,James Corden", TraillerLink="https://www.youtube.com/watch?v=7Pa_Weidt08"},
                new Filme {Id=++id, Titulo = "Ratatouille",ImageExtension=".jpg", DataLancamento = new DateTime(2007,8,15) , Descricao = "Remy reside em Paris e possui um sofisticado paladar. Seu sonho � se tornar um chef de cozinha e desfrutar as diversas obras da arte culin�ria. O �nico problema � que ele � um rato. Quando se acha dentro de um dos restaurantes mais finos de Paris, Remy decide transformar seu sonho em realidade.", Categorias = new List<Categoria>{categorias[0], categorias[1], categorias[8]}, Elenco="Patton Oswalt,Lou Romano,Peter Sohn,Brad Garrett,Janeane Garofalo,Ian Holm,Brian Dennehy,Peter O'Toole", TraillerLink="https://www.youtube.com/watch?v=NgsQ8mVkN8w"},
                new Filme {Id=++id, Titulo = "Rio 2",ImageExtension=".jpg", DataLancamento = new DateTime(2014,4,3) , Descricao = "As araras Blu e Jade vivem felizes com seus filhos no Rio de Janeiro. Quando seus donos, T�lio e Linda, encontram p�ssaros de sua esp�cie na Amaz�nia, eles decidem partir para novas aventuras na Regi�o Norte do pa�s. S� que nem tudo � perfeito. Nigel, o velho inimigo de Blu e Jade, est� de volta para se vingar.", Categorias = new List<Categoria>{categorias[0], categorias[1], categorias[8]}, Elenco="Jesse Eisenberg,Anne Hathaway,Leslie Mann,Rodrigo Santoro,Andy Garc�a,Bruno Mars,Jemaine Clement,George Lopez,Jamie Foxx,will.i.am,Kristin Chenoweth,Miguel Ferrer", TraillerLink="https://www.youtube.com/watch?v=leJuOObuCxM"},
            };
            filmes.ForEach(dd => context.Filmes.AddOrUpdate(d => d.Id, dd));
            context.SaveChanges();
            
            //come�a os ids a 0 para o primeiro ser 1
            id = 0;
            //Filmes
            var comentarios = new List<Comentario>
            {
                new Comentario {Id=++id, Texto = "Muito bom este filme, adorei!",FilmeId=3, Data = new DateTime(2019,4,18,16,33,33) , UserId =  manager.FindByEmail(users[2].Email).Id},
                new Comentario {Id=++id, Texto = "��timo filme!",FilmeId=5, Data = new DateTime(2019,3,16,14,33,33) , UserId =  manager.FindByEmail(users[3].Email).Id},
                new Comentario {Id=++id, Texto = "Uma obra de arte !",FilmeId=5, Data = new DateTime(2019,3,11,19,23,33) , UserId =  manager.FindByEmail(users[1].Email).Id},
                new Comentario {Id=++id, Texto = "Estes atores s�o dignos de Oscar!",FilmeId=7, Data = new DateTime(2019,1,7,11,23,33) , UserId =  manager.FindByEmail(users[2].Email).Id},
                new Comentario {Id=++id, Texto = "Espetacular!",FilmeId=7, Data = new DateTime(2019,1,12,22,14,33) , UserId =  manager.FindByEmail(users[1].Email).Id},
                new Comentario {Id=++id, Texto = "Espetacular!",FilmeId=12, Data = new DateTime(2019,1,12,22,14,33) , UserId =  manager.FindByEmail(users[3].Email).Id},
                new Comentario {Id=++id, Texto = "Espetacular!",FilmeId=10, Data = new DateTime(2019,1,12,22,14,33) , UserId =  manager.FindByEmail(users[2].Email).Id},
                new Comentario {Id=++id, Texto = "Espetacular!",FilmeId=1, Data = new DateTime(2018,1,12,22,14,33) , UserId =  manager.FindByEmail(users[3].Email).Id},
                new Comentario {Id=++id, Texto = "Espetacular!",FilmeId=4, Data = new DateTime(2018,1,15,22,14,33) , UserId =  manager.FindByEmail(users[2].Email).Id},
                new Comentario {Id=++id, Texto = "Espetacular!",FilmeId=6, Data = new DateTime(2019,7,20,22,14,33) , UserId =  manager.FindByEmail(users[1].Email).Id},
                new Comentario {Id=++id, Texto = "Espetacular!",FilmeId=8, Data = new DateTime(2017,11,9,22,14,33) , UserId =  manager.FindByEmail(users[2].Email).Id},
                new Comentario {Id=++id, Texto = "Espetacular!",FilmeId=2, Data = new DateTime(2019,12,12,22,14,33) , UserId =  manager.FindByEmail(users[3].Email).Id},
                new Comentario {Id=++id, Texto = "Espetacular!",FilmeId=11, Data = new DateTime(2019,12,12,22,14,33) , UserId =  manager.FindByEmail(users[2].Email).Id},
                new Comentario {Id=++id, Texto = "Espetacular!",FilmeId=9, Data = new DateTime(2019,12,12,22,14,33) , UserId =  manager.FindByEmail(users[3].Email).Id},
            };
            comentarios.ForEach(dd => context.Comentarios.AddOrUpdate(d => d.Id, dd));
            context.SaveChanges();
            

        }
    }
}
