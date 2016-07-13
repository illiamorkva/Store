using System.Data.Entity;

namespace GameStore_mvc_internet.Models
{
    public class NewsDbInitializer : DropCreateDatabaseAlways<NewsContext>
    {
        //для инициализации базы
        protected override void Seed(NewsContext context)
        {
            context.News.Add(new News() { NewsId = 1, Name = "Новость 1", Description = "Описание 1" });
            context.News.Add(new News() { NewsId = 2, Name = "Новость 2", Description = "Описание 2" });
            context.News.Add(new News() { NewsId = 3, Name = "Новость 3", Description = "Описание 3" });
            context.News.Add(new News() { NewsId = 4, Name = "Новость 4", Description = "Описание 4" });

            base.Seed(context);
        }
    }
}