namespace YAPV.Models
{
    internal interface IImageModel
    {
        string Path { get; set; }
        double Width { get; set; }
        double Height { get; set; }
    }
}