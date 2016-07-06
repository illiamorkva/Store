using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace GameStore_mvc_internet.Models
{
    public class GameDbInitializer : DropCreateDatabaseAlways<GameContext>
    {
        //для инициализации базы
        protected override void Seed(GameContext context)
        {
            context.Games.Add(new Game() { GameId = 1, Name = "Игра 1", Description = "Описание 1", Category = "Категория 1", Price = 137 });
            context.Games.Add(new Game() { GameId = 2, Name = "Игра 2", Description = "Описание 2", Category = "Категория 2", Price = 137 });
            context.Games.Add(new Game() { GameId = 3, Name = "Игра 3", Description = "Описание 3", Category = "Категория 3", Price = 137 });
            context.Games.Add(new Game() { GameId = 4, Name = "Игра 4", Description = "Описание 4", Category = "Категория 4", Price = 137 });

            base.Seed(context);
        }
    }
}