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

        private void btnReturnTheme_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnAcceptTheme_Click(object sender, RoutedEventArgs e)
        {
            if(lbxThemes.SelectedItem == null)
            {
                return;
            }
            ListBoxItem selection = (ListBoxItem)lbxThemes.SelectedItem;
            Window win = Application.Current.MainWindow;
            SolidColorBrush bgt = (SolidColorBrush)this.Resources["BackgroundTheme"];
            SolidColorBrush bgt2 = (SolidColorBrush)win.Resources["BackgroundTheme"];
            SolidColorBrush ft = (SolidColorBrush)this.Resources["FontTheme"];
            SolidColorBrush ft2 = (SolidColorBrush)win.Resources["FontTheme"];
            SolidColorBrush bt = (SolidColorBrush)this.Resources["BorderTheme"];
            SolidColorBrush bt2 = (SolidColorBrush)win.Resources["BorderTheme"];
            string bgc;
            string fc;
            switch (selection.Name)
            {
                case "theme1":
                    bgc = "#2d2d2d";
                    fc = "#ffffff";
                    bgt.Color = Color.FromRgb(Convert.ToByte(bgc.Substring(1, 2), 16), Convert.ToByte(bgc.Substring(3, 2), 16), Convert.ToByte(bgc.Substring(5, 2), 16));
                    bgt2.Color = Color.FromRgb(Convert.ToByte(bgc.Substring(1, 2), 16), Convert.ToByte(bgc.Substring(3, 2), 16), Convert.ToByte(bgc.Substring(5, 2), 16));
                    ft.Color = Color.FromRgb(Convert.ToByte(fc.Substring(1, 2), 16), Convert.ToByte(fc.Substring(3, 2), 16), Convert.ToByte(fc.Substring(5, 2), 16));
                    ft2.Color = Color.FromRgb(Convert.ToByte(fc.Substring(1, 2), 16), Convert.ToByte(fc.Substring(3, 2), 16), Convert.ToByte(fc.Substring(5, 2), 16));
                    bt.Color = Color.FromRgb(Convert.ToByte(fc.Substring(1, 2), 16), Convert.ToByte(fc.Substring(3, 2), 16), Convert.ToByte(fc.Substring(5, 2), 16));
                    bt2.Color = Color.FromRgb(Convert.ToByte(fc.Substring(1, 2), 16), Convert.ToByte(fc.Substring(3, 2), 16), Convert.ToByte(fc.Substring(5, 2), 16));
                    break;
                case "theme2":
                    bgc = "#fffb7a";
                    fc = "#16787b";
                    bgt.Color = Color.FromRgb(Convert.ToByte(bgc.Substring(1, 2), 16), Convert.ToByte(bgc.Substring(3, 2), 16), Convert.ToByte(bgc.Substring(5, 2), 16));
                    bgt2.Color = Color.FromRgb(Convert.ToByte(bgc.Substring(1, 2), 16), Convert.ToByte(bgc.Substring(3, 2), 16), Convert.ToByte(bgc.Substring(5, 2), 16));
                    ft.Color = Color.FromRgb(Convert.ToByte(fc.Substring(1, 2), 16), Convert.ToByte(fc.Substring(3, 2), 16), Convert.ToByte(fc.Substring(5, 2), 16));
                    ft2.Color = Color.FromRgb(Convert.ToByte(fc.Substring(1, 2), 16), Convert.ToByte(fc.Substring(3, 2), 16), Convert.ToByte(fc.Substring(5, 2), 16));
                    bt.Color = Color.FromRgb(Convert.ToByte(fc.Substring(1, 2), 16), Convert.ToByte(fc.Substring(3, 2), 16), Convert.ToByte(fc.Substring(5, 2), 16));
                    bt2.Color = Color.FromRgb(Convert.ToByte(fc.Substring(1, 2), 16), Convert.ToByte(fc.Substring(3, 2), 16), Convert.ToByte(fc.Substring(5, 2), 16));
                    break;
                case "theme3":
                    bgc = "#ff00ee";
                    fc = "#ffffff";
                    bgt.Color = Color.FromRgb(Convert.ToByte(bgc.Substring(1, 2), 16), Convert.ToByte(bgc.Substring(3, 2), 16), Convert.ToByte(bgc.Substring(5, 2), 16));
                    bgt2.Color = Color.FromRgb(Convert.ToByte(bgc.Substring(1, 2), 16), Convert.ToByte(bgc.Substring(3, 2), 16), Convert.ToByte(bgc.Substring(5, 2), 16));
                    ft.Color = Color.FromRgb(Convert.ToByte(fc.Substring(1, 2), 16), Convert.ToByte(fc.Substring(3, 2), 16), Convert.ToByte(fc.Substring(5, 2), 16));
                    ft2.Color = Color.FromRgb(Convert.ToByte(fc.Substring(1, 2), 16), Convert.ToByte(fc.Substring(3, 2), 16), Convert.ToByte(fc.Substring(5, 2), 16));
                    bt.Color = Color.FromRgb(Convert.ToByte(fc.Substring(1, 2), 16), Convert.ToByte(fc.Substring(3, 2), 16), Convert.ToByte(fc.Substring(5, 2), 16));
                    bt2.Color = Color.FromRgb(Convert.ToByte(fc.Substring(1, 2), 16), Convert.ToByte(fc.Substring(3, 2), 16), Convert.ToByte(fc.Substring(5, 2), 16));
                    break;
                case "theme4":
                    bgc = "#004104";
                    fc = "#ffd24c";
                    bgt.Color = Color.FromRgb(Convert.ToByte(bgc.Substring(1, 2), 16), Convert.ToByte(bgc.Substring(3, 2), 16), Convert.ToByte(bgc.Substring(5, 2), 16));
                    bgt2.Color = Color.FromRgb(Convert.ToByte(bgc.Substring(1, 2), 16), Convert.ToByte(bgc.Substring(3, 2), 16), Convert.ToByte(bgc.Substring(5, 2), 16));
                    ft.Color = Color.FromRgb(Convert.ToByte(fc.Substring(1, 2), 16), Convert.ToByte(fc.Substring(3, 2), 16), Convert.ToByte(fc.Substring(5, 2), 16));
                    ft2.Color = Color.FromRgb(Convert.ToByte(fc.Substring(1, 2), 16), Convert.ToByte(fc.Substring(3, 2), 16), Convert.ToByte(fc.Substring(5, 2), 16));
                    bt.Color = Color.FromRgb(Convert.ToByte(fc.Substring(1, 2), 16), Convert.ToByte(fc.Substring(3, 2), 16), Convert.ToByte(fc.Substring(5, 2), 16));
                    bt2.Color = Color.FromRgb(Convert.ToByte(fc.Substring(1, 2), 16), Convert.ToByte(fc.Substring(3, 2), 16), Convert.ToByte(fc.Substring(5, 2), 16));
                    break;
                case "theme5":
                    bgc = "#ffd49b";
                    fc = "#2d2d2d";
                    bgt.Color = Color.FromRgb(Convert.ToByte(bgc.Substring(1, 2), 16), Convert.ToByte(bgc.Substring(3, 2), 16), Convert.ToByte(bgc.Substring(5, 2), 16));
                    bgt2.Color = Color.FromRgb(Convert.ToByte(bgc.Substring(1, 2), 16), Convert.ToByte(bgc.Substring(3, 2), 16), Convert.ToByte(bgc.Substring(5, 2), 16));
                    ft.Color = Color.FromRgb(Convert.ToByte(fc.Substring(1, 2), 16), Convert.ToByte(fc.Substring(3, 2), 16), Convert.ToByte(fc.Substring(5, 2), 16));
                    ft2.Color = Color.FromRgb(Convert.ToByte(fc.Substring(1, 2), 16), Convert.ToByte(fc.Substring(3, 2), 16), Convert.ToByte(fc.Substring(5, 2), 16));
                    bt.Color = Color.FromRgb(Convert.ToByte(fc.Substring(1, 2), 16), Convert.ToByte(fc.Substring(3, 2), 16), Convert.ToByte(fc.Substring(5, 2), 16));
                    bt2.Color = Color.FromRgb(Convert.ToByte(fc.Substring(1, 2), 16), Convert.ToByte(fc.Substring(3, 2), 16), Convert.ToByte(fc.Substring(5, 2), 16));
                    break;
                case "theme6":
                    bgc = "#7f3a5c";
                    fc = "#9b9bff";
                    bgt.Color = Color.FromRgb(Convert.ToByte(bgc.Substring(1, 2), 16), Convert.ToByte(bgc.Substring(3, 2), 16), Convert.ToByte(bgc.Substring(5, 2), 16));
                    bgt2.Color = Color.FromRgb(Convert.ToByte(bgc.Substring(1, 2), 16), Convert.ToByte(bgc.Substring(3, 2), 16), Convert.ToByte(bgc.Substring(5, 2), 16));
                    ft.Color = Color.FromRgb(Convert.ToByte(fc.Substring(1, 2), 16), Convert.ToByte(fc.Substring(3, 2), 16), Convert.ToByte(fc.Substring(5, 2), 16));
                    ft2.Color = Color.FromRgb(Convert.ToByte(fc.Substring(1, 2), 16), Convert.ToByte(fc.Substring(3, 2), 16), Convert.ToByte(fc.Substring(5, 2), 16));
                    bt.Color = Color.FromRgb(Convert.ToByte(fc.Substring(1, 2), 16), Convert.ToByte(fc.Substring(3, 2), 16), Convert.ToByte(fc.Substring(5, 2), 16));
                    bt2.Color = Color.FromRgb(Convert.ToByte(fc.Substring(1, 2), 16), Convert.ToByte(fc.Substring(3, 2), 16), Convert.ToByte(fc.Substring(5, 2), 16));
                    break;
            }
        }
    }
}
