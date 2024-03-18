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
        private Game _game = new Game();
        public MainWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Changes Main screen to the How to Play screen on click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnHow_Click(object sender, RoutedEventArgs e)
        {
            gridMenu.Visibility = Visibility.Collapsed;
            gridHowto.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// Changes How to Play screen to Main screen on click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnReturnHow_Click(object sender, RoutedEventArgs e)
        {
            gridMenu.Visibility = Visibility.Visible;
            gridHowto.Visibility = Visibility.Collapsed;
        }

        /// <summary>
        /// Changes Main screen to the About screen on click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAbout_Click(object sender, RoutedEventArgs e)
        {
            gridMenu.Visibility = Visibility.Collapsed;
            gridAbout.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// Changes About screen to the Main screen on click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnReturnAbout_Click(object sender, RoutedEventArgs e)
        {
            gridMenu.Visibility = Visibility.Visible;
            gridAbout.Visibility = Visibility.Collapsed;
        }

        /// <summary>
        /// Changes Main screen to the Game screen on click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            gridMenu.Visibility = Visibility.Collapsed;
            gridGame.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// Toggles visibility of the statistics list
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        /// <summary>
        /// Toggles visibility of the inventory list
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        /// <summary>
        /// Removes text from the prompt when clicked
        /// Also changes font to Normal as placeholder text is in Italic
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tbxPrompt_GotFocus(object sender, RoutedEventArgs e)
        {
            tbxPrompt.FontStyle = FontStyles.Normal;
            tbxPrompt.Text = "";
        }
    }
}
