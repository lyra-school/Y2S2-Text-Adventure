using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Y2S2_Text_Adventure
{
    /// <summary>
    /// Interaction logic for ThemeSelection.xaml
    /// </summary>
    public partial class ThemeSelection : Window
    {
        public ThemeSelection()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Closes the window when the Return button is clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnReturnTheme_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Changes the UI theme to the selected option when the Accept button is clicked, but only for the ThemeSelection window.
        /// The original method was intended to target the MainWindow themes as well, but due to a limitation with WPF, any attempt to reach it through this window reports that its resources are frozen.
        /// This isn't solved by using Show() to display this window.
        /// The window and the method are preserved for the sake of demonstration.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAcceptTheme_Click(object sender, RoutedEventArgs e)
        {
            // Do nothing if the user hasn't selected anything.
            if(lbxThemes.SelectedItem == null)
            {
                return;
            }

            // Gets the selected theme as well as appropriate resoures from ThemeSelection window.
            // The commented code is how it would be done for MainWindow, but will result in an exception if ran as it is.
            ListBoxItem selection = (ListBoxItem)lbxThemes.SelectedItem;
            //Window win = Application.Current.MainWindow;
            SolidColorBrush bgt = (SolidColorBrush)this.Resources["BackgroundTheme"];
            //SolidColorBrush bgt2 = (SolidColorBrush)win.Resources["BackgroundTheme"];
            SolidColorBrush ft = (SolidColorBrush)this.Resources["FontTheme"];
            //SolidColorBrush ft2 = (SolidColorBrush)win.Resources["FontTheme"];
            SolidColorBrush bt = (SolidColorBrush)this.Resources["BorderTheme"];
            //SolidColorBrush bt2 = (SolidColorBrush)win.Resources["BorderTheme"];

            /*
             * Hacky way of changing colours. WPF offers no way to extract a Color object from Brush (which is how Background is stored in a ListBoxItem.)
             * Instead, each ListBoxItem is given a name that corresponds to specific two colour hexes, segments of which are converted into bytes when applied to the resource.
             * All bytes are in R, G, B order.
             */
            string bgc;
            string fc;
            switch (selection.Name)
            {
                case "theme1":
                    bgc = "#2d2d2d";
                    fc = "#ffffff";
                    bgt.Color = Color.FromRgb(Convert.ToByte(bgc.Substring(1, 2), 16), Convert.ToByte(bgc.Substring(3, 2), 16), Convert.ToByte(bgc.Substring(5, 2), 16));
                    //bgt2.Color = Color.FromRgb(Convert.ToByte(bgc.Substring(1, 2), 16), Convert.ToByte(bgc.Substring(3, 2), 16), Convert.ToByte(bgc.Substring(5, 2), 16));
                    ft.Color = Color.FromRgb(Convert.ToByte(fc.Substring(1, 2), 16), Convert.ToByte(fc.Substring(3, 2), 16), Convert.ToByte(fc.Substring(5, 2), 16));
                    //ft2.Color = Color.FromRgb(Convert.ToByte(fc.Substring(1, 2), 16), Convert.ToByte(fc.Substring(3, 2), 16), Convert.ToByte(fc.Substring(5, 2), 16));
                    bt.Color = Color.FromRgb(Convert.ToByte(fc.Substring(1, 2), 16), Convert.ToByte(fc.Substring(3, 2), 16), Convert.ToByte(fc.Substring(5, 2), 16));
                    //bt2.Color = Color.FromRgb(Convert.ToByte(fc.Substring(1, 2), 16), Convert.ToByte(fc.Substring(3, 2), 16), Convert.ToByte(fc.Substring(5, 2), 16));
                    break;
                case "theme2":
                    bgc = "#fffb7a";
                    fc = "#16787b";
                    bgt.Color = Color.FromRgb(Convert.ToByte(bgc.Substring(1, 2), 16), Convert.ToByte(bgc.Substring(3, 2), 16), Convert.ToByte(bgc.Substring(5, 2), 16));
                    //bgt2.Color = Color.FromRgb(Convert.ToByte(bgc.Substring(1, 2), 16), Convert.ToByte(bgc.Substring(3, 2), 16), Convert.ToByte(bgc.Substring(5, 2), 16));
                    ft.Color = Color.FromRgb(Convert.ToByte(fc.Substring(1, 2), 16), Convert.ToByte(fc.Substring(3, 2), 16), Convert.ToByte(fc.Substring(5, 2), 16));
                    //ft2.Color = Color.FromRgb(Convert.ToByte(fc.Substring(1, 2), 16), Convert.ToByte(fc.Substring(3, 2), 16), Convert.ToByte(fc.Substring(5, 2), 16));
                    bt.Color = Color.FromRgb(Convert.ToByte(fc.Substring(1, 2), 16), Convert.ToByte(fc.Substring(3, 2), 16), Convert.ToByte(fc.Substring(5, 2), 16));
                    //bt2.Color = Color.FromRgb(Convert.ToByte(fc.Substring(1, 2), 16), Convert.ToByte(fc.Substring(3, 2), 16), Convert.ToByte(fc.Substring(5, 2), 16));
                    break;
                case "theme3":
                    bgc = "#ff00ee";
                    fc = "#ffffff";
                    bgt.Color = Color.FromRgb(Convert.ToByte(bgc.Substring(1, 2), 16), Convert.ToByte(bgc.Substring(3, 2), 16), Convert.ToByte(bgc.Substring(5, 2), 16));
                    //bgt2.Color = Color.FromRgb(Convert.ToByte(bgc.Substring(1, 2), 16), Convert.ToByte(bgc.Substring(3, 2), 16), Convert.ToByte(bgc.Substring(5, 2), 16));
                    ft.Color = Color.FromRgb(Convert.ToByte(fc.Substring(1, 2), 16), Convert.ToByte(fc.Substring(3, 2), 16), Convert.ToByte(fc.Substring(5, 2), 16));
                    //ft2.Color = Color.FromRgb(Convert.ToByte(fc.Substring(1, 2), 16), Convert.ToByte(fc.Substring(3, 2), 16), Convert.ToByte(fc.Substring(5, 2), 16));
                    bt.Color = Color.FromRgb(Convert.ToByte(fc.Substring(1, 2), 16), Convert.ToByte(fc.Substring(3, 2), 16), Convert.ToByte(fc.Substring(5, 2), 16));
                    //bt2.Color = Color.FromRgb(Convert.ToByte(fc.Substring(1, 2), 16), Convert.ToByte(fc.Substring(3, 2), 16), Convert.ToByte(fc.Substring(5, 2), 16));
                    break;
                case "theme4":
                    bgc = "#004104";
                    fc = "#ffd24c";
                    bgt.Color = Color.FromRgb(Convert.ToByte(bgc.Substring(1, 2), 16), Convert.ToByte(bgc.Substring(3, 2), 16), Convert.ToByte(bgc.Substring(5, 2), 16));
                    //bgt2.Color = Color.FromRgb(Convert.ToByte(bgc.Substring(1, 2), 16), Convert.ToByte(bgc.Substring(3, 2), 16), Convert.ToByte(bgc.Substring(5, 2), 16));
                    ft.Color = Color.FromRgb(Convert.ToByte(fc.Substring(1, 2), 16), Convert.ToByte(fc.Substring(3, 2), 16), Convert.ToByte(fc.Substring(5, 2), 16));
                    //ft2.Color = Color.FromRgb(Convert.ToByte(fc.Substring(1, 2), 16), Convert.ToByte(fc.Substring(3, 2), 16), Convert.ToByte(fc.Substring(5, 2), 16));
                    bt.Color = Color.FromRgb(Convert.ToByte(fc.Substring(1, 2), 16), Convert.ToByte(fc.Substring(3, 2), 16), Convert.ToByte(fc.Substring(5, 2), 16));
                    //bt2.Color = Color.FromRgb(Convert.ToByte(fc.Substring(1, 2), 16), Convert.ToByte(fc.Substring(3, 2), 16), Convert.ToByte(fc.Substring(5, 2), 16));
                    break;
                case "theme5":
                    bgc = "#ffd49b";
                    fc = "#2d2d2d";
                    bgt.Color = Color.FromRgb(Convert.ToByte(bgc.Substring(1, 2), 16), Convert.ToByte(bgc.Substring(3, 2), 16), Convert.ToByte(bgc.Substring(5, 2), 16));
                    //bgt2.Color = Color.FromRgb(Convert.ToByte(bgc.Substring(1, 2), 16), Convert.ToByte(bgc.Substring(3, 2), 16), Convert.ToByte(bgc.Substring(5, 2), 16));
                    ft.Color = Color.FromRgb(Convert.ToByte(fc.Substring(1, 2), 16), Convert.ToByte(fc.Substring(3, 2), 16), Convert.ToByte(fc.Substring(5, 2), 16));
                    //ft2.Color = Color.FromRgb(Convert.ToByte(fc.Substring(1, 2), 16), Convert.ToByte(fc.Substring(3, 2), 16), Convert.ToByte(fc.Substring(5, 2), 16));
                    bt.Color = Color.FromRgb(Convert.ToByte(fc.Substring(1, 2), 16), Convert.ToByte(fc.Substring(3, 2), 16), Convert.ToByte(fc.Substring(5, 2), 16));
                    //bt2.Color = Color.FromRgb(Convert.ToByte(fc.Substring(1, 2), 16), Convert.ToByte(fc.Substring(3, 2), 16), Convert.ToByte(fc.Substring(5, 2), 16));
                    break;
                case "theme6":
                    bgc = "#7f3a5c";
                    fc = "#9b9bff";
                    bgt.Color = Color.FromRgb(Convert.ToByte(bgc.Substring(1, 2), 16), Convert.ToByte(bgc.Substring(3, 2), 16), Convert.ToByte(bgc.Substring(5, 2), 16));
                    //bgt2.Color = Color.FromRgb(Convert.ToByte(bgc.Substring(1, 2), 16), Convert.ToByte(bgc.Substring(3, 2), 16), Convert.ToByte(bgc.Substring(5, 2), 16));
                    ft.Color = Color.FromRgb(Convert.ToByte(fc.Substring(1, 2), 16), Convert.ToByte(fc.Substring(3, 2), 16), Convert.ToByte(fc.Substring(5, 2), 16));
                    //ft2.Color = Color.FromRgb(Convert.ToByte(fc.Substring(1, 2), 16), Convert.ToByte(fc.Substring(3, 2), 16), Convert.ToByte(fc.Substring(5, 2), 16));
                    bt.Color = Color.FromRgb(Convert.ToByte(fc.Substring(1, 2), 16), Convert.ToByte(fc.Substring(3, 2), 16), Convert.ToByte(fc.Substring(5, 2), 16));
                    //bt2.Color = Color.FromRgb(Convert.ToByte(fc.Substring(1, 2), 16), Convert.ToByte(fc.Substring(3, 2), 16), Convert.ToByte(fc.Substring(5, 2), 16));
                    break;
            }
        }
    }
}
