﻿using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Animation;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using SettingsUI.Helpers;
using YAPV.Views.Pages;
using WinUIEx;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace YAPV
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainWindow : WindowEx
    {
        public MainWindow()
        {
            this.InitializeComponent();
            contentFrame.Navigate(typeof(GalleryView), null, new DrillInNavigationTransitionInfo());

            //ExtendsContentIntoTitleBar = true;
            //SetTitleBar(TitleBar);
            //this.Activated += MainWindow_Activated;

            MaterialHelper.MakeTransparent(this);
            MaterialHelper.SetBlur(true, true);
            //MaterialHelper.SetMica(true, true);
        }
    }
}
