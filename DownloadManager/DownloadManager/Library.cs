using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace DownloadManager
{
    public class Library
    {
        public void Back(ref WebView web)
        {
            if (web.CanGoBack)
            {
                web.GoBack();

            }

        }
        public void Forward(ref WebView web)
        {
            if (web.CanGoForward)
            {
                web.GoForward();
            }
        }
        public void Go(ref WebView web, string value, KeyRoutedEventArgs args)
        {
            if(args.Key == Windows.System.VirtualKey.Enter)
            {
                try
                {
                    web.Navigate(new Uri(value));
                }
                catch (Exception ex)
                {
                    throw;
                }
                web.Focus(Windows.UI.Xaml.FocusState.Keyboard);
            }
        }

       
    }
}
