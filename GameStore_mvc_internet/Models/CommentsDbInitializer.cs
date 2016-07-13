using System.Data.Entity;

namespace GameStore_mvc_internet.Models
{
    public class CommentsDbInitializer : DropCreateDatabaseAlways<CommentContext>
    {
        //для инициализации базы
        protected override void Seed(CommentContext context)
        {
            context.Comments.Add(new Comment() { ShopId = 1, Author = "Автор 1", Description = "Комментарий 1" });
            context.Comments.Add(new Comment() { ShopId = 2, Author = "Автор 2", Description = "Комментарий 2" });
            context.Comments.Add(new Comment() { ShopId = 3, Author = "Автор 3", Description = "Комментарий 3" });
            context.Comments.Add(new Comment() { ShopId = 4, Author = "Автор 4", Description = "Комментарий 4" });

            base.Seed(context);
        }
    }
}