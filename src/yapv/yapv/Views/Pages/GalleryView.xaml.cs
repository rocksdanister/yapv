﻿using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using YAPV.ViewModels;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace YAPV.Views.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class GalleryView : Page
    {
        public GalleryView()
        {
            this.InitializeComponent();
            var vm = App.Services.GetRequiredService<GalleryViewModel>();
            this.DataContext = vm;
            vm.PropertyChanged += (sender, args) => {
                if (args.PropertyName.Equals("SelectedItem"))
                {
                    ScrlImg.ChangeView(0f, 0f, (float)(PreviewRow.ActualHeight / vm.SelectedItem.Height));
                }
            };
        }
    }
}
