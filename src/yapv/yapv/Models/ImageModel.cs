using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YAPV.Models
{
    internal class ImageModel : IImageModel
    {
        public ImageModel(string path)
        {
            this.Path = path;
            //TODO: WxH
        }

        public string Path { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }
    }
}
