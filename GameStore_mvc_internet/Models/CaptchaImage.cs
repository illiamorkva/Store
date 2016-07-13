using System;
using System.Drawing;
using System.Drawing.Imaging;

namespace GameStore_mvc_internet.Models
{
    public class CaptchaImage
    {
        private string text;
        private int width;
        private int height;
        public Bitmap Image { get; set; }

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
            g.FillRectangle(b, 0, 0, width, height);
           
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