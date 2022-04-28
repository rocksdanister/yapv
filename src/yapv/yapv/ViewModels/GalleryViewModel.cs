using Microsoft.Toolkit.Mvvm.ComponentModel;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YAPV.Models;

namespace YAPV.ViewModels
{
    public class GalleryViewModel : ObservableObject
    {
        public GalleryViewModel()
        {
            var images = GetFiles(Environment.GetFolderPath(Environment.SpecialFolder.MyPictures), 
                new string[] {"*.jpg", "*.jpeg", "*.png", "*.bmp", "*.tif", "*.tiff", "*.webp", "*.gif" }, 
                SearchOption.TopDirectoryOnly);
            foreach (var item in images)
            {
                LibraryItems.Add(new ImageModel(item));
            }
        }

        private IImageModel _selectedItem;
        public IImageModel SelectedItem
        {
            get => _selectedItem;
            private set => SetProperty(ref _selectedItem, value);
        }

        private ObservableCollection<IImageModel> _libraryItems = new();
        public ObservableCollection<IImageModel> LibraryItems
        {
            get => _libraryItems;
            private set => SetProperty(ref _libraryItems, value);
        }

        #region helpers

        private static IEnumerable<string> GetFiles(string path, string[] searchPattern, SearchOption searchOption)
        {
            foreach (string sp in searchPattern)
            {
                foreach (var item in Directory.EnumerateFiles(path, sp, searchOption))
                {
                    yield return item;
                }
            }
        }

        #endregion //helpers

    }
}
