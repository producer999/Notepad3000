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

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Notepad3000
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        private void TabKeyDown(object sender, KeyRoutedEventArgs e)
        {
            if(e.Key == Windows.System.VirtualKey.Tab)
            {
                var reb = (RichEditBox)sender;
                //string selectedText = "";

                e.Handled = true;
                // reb.Document.Selection.GetText(Windows.UI.Text.TextGetOptions.None, selectedText);
                reb.Document.Selection.TypeText("\t");
            }
        }

        private void MenuButtonClicked(object sender, RoutedEventArgs e)
        {
            var menu = (Button)sender;
            var fly = (MenuFlyout)menu.Flyout;

            fly.ShowAt(menu, new Point(0, menu.ActualHeight));
        }

        private void NewClicked(object sender, RoutedEventArgs e)
        {
            MainTextBox.Document.SetText(Windows.UI.Text.TextSetOptions.None,"");
        }
    }
}
