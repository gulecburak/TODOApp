namespace ToDoApp503.Migrations
{
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using System.Threading.Tasks;
    using ToDoApp503.Models;

    internal sealed class Configuration : DbMigrationsConfiguration<ToDoApp503.Models.AppDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(ToDoApp503.Models.AppDbContext context)
        {
            if (!context.Categories.Any())
            {
                context.Categories.Add(new Models.Category { Name = "Deneme", CreateDate = DateTime.Now, CreatedBy = "username", UpdateDate = DateTime.Now, UpdatedBy = "username" });
                var store = new UserStore<ApplicationUser>(context);
                var manager = new ApplicationUserManager(store);
                var user = new ApplicationUser() { Email = "burak@gmail.com", UserName = "burak@gmail.com" };
                Task<Microsoft.AspNet.Identity.IdentityResult> task = Task.Run(()=>manager.CreateAsync(user,"ToDoItem123+"));
                var result = task.Result;
                context.SaveChanges();
            }
        }
    }
}
