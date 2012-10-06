﻿using System;
using System.Drawing;
using System.Windows.Forms;

namespace LiveTelemetry
{
    public partial class ucResizableImage : UserControl
    {
        private string imagePath = "";
        private Bitmap imageBMP;
        private Size bmpSize;

        public Size PictureSize
        {
            get
            {
                return bmpSize;
            }
        }

        public ucResizableImage(string image)
        {
            InitializeComponent();

            imagePath = image;
            imageBMP = (Bitmap)Bitmap.FromFile(image);

            SetStyle(
              ControlStyles.AllPaintingInWmPaint |
              ControlStyles.UserPaint |
              ControlStyles.DoubleBuffer, true);
        }

        public void Crop(int w, int h)
        {
            Crop(w, h, true);
        }

        public void Crop(int w, int h, bool resize)
        {
            if (h > imageBMP.Size.Height)
                h = imageBMP.Size.Height;

            Size = new Size(w, h);
            if (w > imageBMP.Size.Width)
                w = imageBMP.Size.Width;
            
            bmpSize = new Size(w, h);
            
            if (w < imageBMP.Size.Height)
            {
                bmpSize = new Size(h * imageBMP.Size.Width / imageBMP.Size.Height, h);
            }
            
            if (imageBMP.Size.Width > w || w < imageBMP.Size.Width)
            {
                bmpSize = new Size(w, w * imageBMP.Size.Height / imageBMP.Size.Width);
            }
            
            if (this.bmpSize.Height > h)
            {
                // back to original..
                bmpSize = new Size(h * imageBMP.Size.Width / imageBMP.Size.Height, h);
            }

            if (resize)
            {
                Invalidate();
                OnResize(null);
            }
        }

        protected override void OnResize(EventArgs e)
        {
            if (this.bmpSize.Width == 0)
            {
                Crop(this.Size.Width, this.Size.Height, false);
            }
            Bitmap b = new Bitmap(this.Size.Width, this.Size.Height);
            Graphics g = Graphics.FromImage((Image)b);
            g.DrawImage(imageBMP, (this.Width - bmpSize.Width) / 2, (this.Height - bmpSize.Height) / 2, this.bmpSize.Width, this.bmpSize.Height);
            g.Dispose();
            this.BackgroundImage = (Image)b;
            if (e != null) base.OnResize(e);
        }
    }

}