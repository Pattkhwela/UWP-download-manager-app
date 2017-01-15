using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace DownloadManager
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        StorageFile downloadedFile;

        public MainPage()
        {
            this.InitializeComponent();
        }

        //Navigation button mapping

        private void Settings_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(Settings));
        }
        private void Browser_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(Browser));
        }

        private async void Download_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtLink.Text))
            {
                string fileName = string.Empty;
                string fileExtention = string.Empty;
                try
                {
                    fileName = System.IO.Path.GetFileName(txtLink.Text).Substring(0, System.IO.Path.GetFileName(txtLink.Text).Length - 4);
                    fileExtention = txtLink.Text.Substring(txtLink.Text.LastIndexOf(','));
                }
                catch
                {
                    MessageDialog dialog = new MessageDialog("Could not determine the name and the extention");
                    dialog.ShowAsync();
                }

                if (fileName != null && fileExtention != null)
                {
                    FolderPicker folderPicker = new FolderPicker();

                    folderPicker.SuggestedStartLocation = PickerLocationId.Desktop;
                    folderPicker.ViewMode = PickerViewMode.Thumbnail;
                    folderPicker.FileTypeFilter.Add("*");
                    StorageFolder folder = await folderPicker.PickSingleFolderAsync();
                    if (folder != null)
                    {
                        try
                        {
                            DownloadButton.IsEnabled = false;
                            ProgressBar.IsIndeterminate = true;
                            ProgressBar.Visibility = Visibility.Visible;
                            Uri address = new Uri(txtLink.Text, UriKind.Absolute);
                            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(address);
                            WebResponse response = await request.GetResponseAsync();
                            Stream stream = response.GetResponseStream();
                            StorageFile file = await folder.CreateFileAsync(fileName + fileExtention, CreationCollisionOption.GenerateUniqueName);
                            await Windows.Storage.FileIO.WriteBytesAsync(file, ReadStream(stream));
                            downloadedFile = file;
                            DownloadButton.IsEnabled = true;
                            ProgressBar.Visibility = Visibility.Collapsed;
                            ProgressBar.IsIndeterminate = false;
                            txtDownloadCompleted.Visibility = Visibility.Visible;
                            btnOpenFile.Visibility = Visibility.Visible;
                        }


                        catch
                        {
                            MessageDialog dialog = new MessageDialog("Error 1");
                            dialog.ShowAsync();
                            DownloadButton.IsEnabled = true;
                            ProgressBar.Visibility = Visibility.Collapsed;
                            ProgressBar.IsIndeterminate = false;
                            txtDownloadCompleted.Visibility = Visibility.Collapsed;
                            btnOpenFile.Visibility = Visibility.Collapsed;
                            downloadedFile = null;

                        }


                    }
                }
            }
        }

        private byte[] ReadStream(Stream stream)
        {
            byte[] buffer = new byte[16 * 1024];
            using (MemoryStream ms = new MemoryStream())
            {
                int read;
                while ((read = stream.Read(buffer, 0, buffer.Length))>0)
                {
                    ms.Write(buffer, 0, read);
                }
                return ms.ToArray();
            }
        }

        private async void btnOpenFile_Click(object sender, RoutedEventArgs e)
        {
            if (downloadedFile != null)
            {
                await Windows.System.Launcher.LaunchFileAsync(downloadedFile);
            }

        }
    }
}
