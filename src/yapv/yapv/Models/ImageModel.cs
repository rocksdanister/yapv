using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YAPV.Models
{
    public class ImageModel : IImageModel
    {
        public ImageModel(string path)
        {
            this.Path = path;
            var img = Image.FromStream(File.OpenRead(path), false, false);
            Width = img.Width;
            Height = img.Height;
        }
        public string Path { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }
    }
}
