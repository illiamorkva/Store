using System.Data.Entity;

namespace GameStore_mvc_internet.Models
{
    public class ProductsDbInitializer : DropCreateDatabaseAlways<ShareContext>
    {
        //для инициализации базы
        protected override void Seed(ShareContext context)
        {
            context.Shares.Add(new Share() { ShareId = 1, Name = "Акция 1", Description = "Описание 1" });
            context.Shares.Add(new Share() { ShareId = 2, Name = "Акция 2", Description = "Описание 2" });
            context.Shares.Add(new Share() { ShareId = 3, Name = "Акция 3", Description = "Описание 3" });
            context.Shares.Add(new Share() { ShareId = 4, Name = "Акция 4", Description = "Описание 4" });

            base.Seed(context);
        }
    }

   
   

}