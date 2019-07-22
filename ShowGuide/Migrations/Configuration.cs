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

            List<ApplicationUser> user = new List<Models.ApplicationUser> {
                new ApplicationUser {Email = "admin@admin.adm", Nome = "Admin"},
                new ApplicationUser {Email = "dora_estrela97@ipt.pt", Nome = "Dora Fernandes"},
                new ApplicationUser {Email = "joao2000@ipt.pt", Nome = "João Manuel"},
                new ApplicationUser {Email = "tatianaPereira@ipt.pt", Nome = "Tatiana Pereira"},
            };
            /////////////////////////// USERS ///////////////////////////////////
            var store = new UserStore<ApplicationUser>(context);
            var manager = new UserManager<ApplicationUser>(store);
            /////////////////////////// ADMIN ///////////////////////////////////
            ApplicationUser us = user[0]; //Primeiro utilizador do seed da tabela de Users
            us.UserName = us.Email;
            if (!context.Users.Any(u => u.UserName == us.Email))
            {

                manager.Create(us, "Qwe123."); // Palavra passe do admin 
                manager.AddToRole(us.Id, "Admin");
            }
            //Os restantes users são views da aplicação web 
            for (int i = 1; i < user.Count(); i++)
            {
                ApplicationUser us2 = user[i];
                us2.UserName = us2.Email;
                if (!context.Users.Any(u => u.UserName == us2.Email))
                {
                    manager.Create(us2, "123Querty#");
                    manager.AddToRole(us2.Id, "Viewer");
                }
            }
        }
    }
}
