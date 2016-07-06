using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Web;

namespace GameStore_mvc_internet.Models
{
    public class CaptchaImage
    {
        private string text; // текст капчи
        private int width; // ширина картинки
        private int height; // высота картинки
        public Bitmap Image { get; set; } // само изображение капчи

        public CaptchaImage(string s, int width, int height)
        {
            text = s;
            this.width = width;
            this.height = height;
            GenerateImage();
        }
        // создаем изображение
        private void GenerateImage()
        {
            Bitmap bitmap = new Bitmap(width, height, PixelFormat.Format32bppArgb);

            Graphics g = Graphics.FromImage(bitmap);
            SolidBrush b = new SolidBrush(Color.FromArgb(16, 164, 210));
            g.FillRectangle(b, 0, 0, this.width, this.height);
            // отрисовка строки
            g.DrawString(text, new Font("Arial", height / 2, FontStyle.Bold),
                                Brushes.White, new RectangleF(0, 0, width, height));

            g.Dispose();

            Image = bitmap;
        }

        ~CaptchaImage()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
                Image.Dispose();
        }
    }
}