using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GameStore_mvc_internet.Models
{
    // корзина
    public class Cart
    {
        //коллекция товарок в корзине
        private List<CartLine> lineCollection = new List<CartLine>();

        //метод добавления в корзину
        public void AddItem(Game game, int quantity)
        {
            CartLine line = lineCollection
                .Where(g => g.Game.GameId == game.GameId)
                .FirstOrDefault();

            if (line == null)
            {
                lineCollection.Add(new CartLine
                {
                    Game = game,
                    Quantity = quantity
                });
            }
            else
            {
                line.Quantity += quantity;
            }
        }

        // метод удаления из корзины
        public void RemoveLine(Game game)
        {
            lineCollection.RemoveAll(l => l.Game.GameId == game.GameId);
        }
        //итого
        public decimal ComputeTotalValue()
        {
            return lineCollection.Sum(e => e.Game.Price * e.Quantity);

        }
        //чистка коллекции
        public void Clear()
        {
            lineCollection.Clear();
        }
        // вывод коллекци
        public IEnumerable<CartLine> Lines
        {
            get { return lineCollection; }
        }
    }

    public class CartLine
    {
        public Game Game { get; set; }
        public int Quantity { get; set; } // количество
    }
}