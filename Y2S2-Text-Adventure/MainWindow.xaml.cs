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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Y2S2_Text_Adventure
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnHow_Click(object sender, RoutedEventArgs e)
        {
            gridMenu.Visibility = Visibility.Collapsed;
            gridHowto.Visibility = Visibility.Visible;
        }

        private void btnReturnHow_Click(object sender, RoutedEventArgs e)
        {
            gridMenu.Visibility = Visibility.Visible;
            gridHowto.Visibility = Visibility.Collapsed;
        }

        private void btnAbout_Click(object sender, RoutedEventArgs e)
        {
            gridMenu.Visibility = Visibility.Collapsed;
            gridAbout.Visibility = Visibility.Visible;
        }

        private void btnReturnAbout_Click(object sender, RoutedEventArgs e)
        {
            gridMenu.Visibility = Visibility.Visible;
            gridAbout.Visibility = Visibility.Collapsed;
        }

        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            gridMenu.Visibility = Visibility.Collapsed;
            gridGame.Visibility = Visibility.Visible;
        }

        private void btnStats_Click(object sender, RoutedEventArgs e)
        {
            if(tblkStats.Visibility == Visibility.Visible)
            {
                tblkStats.Visibility = Visibility.Collapsed;
            } else
            {
                tblkStats.Visibility = Visibility.Visible;
            }
        }

        private void btnInventory_Click(object sender, RoutedEventArgs e)
        {
            if(tblkInventory.Visibility == Visibility.Visible)
            {
                tblkInventory.Visibility = Visibility.Collapsed;
            } else
            {
                tblkInventory.Visibility = Visibility.Visible;
            }
        }

        private void tbxPrompt_GotFocus(object sender, RoutedEventArgs e)
        {
            tbxPrompt.FontStyle = FontStyles.Normal;
            tbxPrompt.Text = "";
        }
    }
}
