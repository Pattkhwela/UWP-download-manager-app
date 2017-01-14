using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace DownloadManager
{
    /// <summary>
    /// An empty page that can be used on its own o r navigated to within a Frame.
    /// </summary>
    public sealed partial class Browser : Page
    {
        Library Library = new Library();

        public Browser()
        {
            this.InitializeComponent();
        }
        private void Value_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            Library.Go(ref Display, Value.Text, e);
        }

        
        private void Display_NavigationCompleted(WebView sender, WebViewNavigationCompletedEventArgs args)
        {
            if(args.IsSuccess)
            {
                Value.Text = args.Uri.ToString(); 
            }
        }
        private void Back_Click(object sender, RoutedEventArgs e)
        {
            Library.Back(ref Display);
        }
        private void Forward_Click(object sender, RoutedEventArgs e)
        {
            Library.Forward(ref Display);
        }
        private void Refresh_Click(object sender, RoutedEventArgs e)
        {
            Display.Refresh();
        }

        private void Home_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(MainPage));
        }
    }
}

