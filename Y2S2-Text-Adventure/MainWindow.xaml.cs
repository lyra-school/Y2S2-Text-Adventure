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
        private Saves db = new Saves();
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

            _game.ReadScenes();
            SceneTextUpdater();
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

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnPrompt_Click(object sender, RoutedEventArgs e)
        {
            string command = tbxPrompt.Text;
            if(String.IsNullOrEmpty(command))
            {
                return;
            }
            command = command.ToLower();
            string[] commandComponents = command.Split(' ');
            Command cmd;
            bool exists = Enum.TryParse(commandComponents[0].ToUpper(), out cmd);
            if (!exists)
            {
                TextUpdater("Could not find command: " + commandComponents[0]);
                return;
            }
            if (cmd == Command.LOOK && commandComponents.Length == 1)
            {
                TextUpdater("You take a look again.");
                SceneTextUpdater();
                return;
            }
            if (cmd == Command.GO)
            {
                if (commandComponents.Length < 2)
                {
                    TextUpdater("\n\nGo where?");
                    return;
                }
                Direction dir;
                bool exists2 = Enum.TryParse(commandComponents[1].ToUpper(), out dir);
                if (!exists2)
                {
                    TextUpdater("Not a recognized direction: " + commandComponents[1]);
                    return;
                }
                string nextScene = _game.CurrentScene.GetConnectionName(dir);
                if (String.IsNullOrEmpty(nextScene))
                {
                    TextUpdater("You can't find a way out in that direction.");
                    return;
                }
                else
                {
                    Scene transition = new Scene();
                    foreach (Scene scene in _game.Scenes)
                    {
                        if (scene.Name == nextScene)
                        {
                            transition = scene;
                        }
                    }
                    _game.CurrentScene = transition;
                    TextUpdater("You head over to " + nextScene + ".");
                    SceneTextUpdater();
                    return;
                }
            }
            Item targetItem = new Item();
            Item secondItem = new Item();
            bool tItemInInv = false;
            bool sItemInInv = false;
            try
            {
                targetItem = _game.ItemFinder(commandComponents[1], out tItemInInv);
                if (targetItem.Name == "None")
                {
                    TextUpdater("\n\nItem not found: " + commandComponents[1] + ".");
                    return;
                }
                if (commandComponents.Length > 2)
                {
                    secondItem = _game.ItemFinder(commandComponents[2], out sItemInInv);
                    if (secondItem.Name == "None")
                    {
                        TextUpdater("Item not found: " + commandComponents[2] + ".");
                        return;
                    }
                }
            }
            catch
            {
                TextUpdater("The command " + cmd.ToString() + " is missing a parameter.");
                return;
            }
            if (secondItem.Name != "None" && (cmd == Command.LOOK || cmd == Command.TAKE))
            {
                TextUpdater($"The {cmd} command cannot be used with more than one item.");
                return;
            }
            else if (cmd == Command.LOOK)
            {
                TextUpdater(targetItem.Description);
                return;
            } else if(cmd == Command.TAKE)
            {
                if(targetItem.Type == ItemType.STATIC)
                {
                    TextUpdater("You can't pick up this item!");
                    return;
                }
                if(tItemInInv)
                {
                    TextUpdater("This item is already in your inventory.");
                    return;
                } else
                {
                    _game.CurrentScene.Items.Remove(targetItem);
                    _game.Inventory.Add(targetItem);
                }
            }
            Interaction intr = targetItem.ReturnInteraction(cmd, secondItem);
            if (intr.Description == "0")
            {
                TextUpdater("Command does not exist for this item.");
                return;
            }
            else if (intr.Description == "1")
            {
                TextUpdater($"{targetItem} cannot be used with {secondItem}.");
                return;
            }
            Type kindOfInteraction = intr.GetType();
            if (kindOfInteraction == typeof(AdvanceInteraction))
            {
                string nextScene = intr.GetTargetScene();
                Scene transition = new Scene();
                foreach (Scene scene in _game.Scenes)
                {
                    if (scene.Name == nextScene)
                    {
                        transition = scene;
                    }
                }
                _game.CurrentScene = transition;
                TextUpdater(intr.Description + "\n\nYou are whisked away to " + nextScene + ".");
                SceneTextUpdater();
                return;
            }
            else if (kindOfInteraction == typeof(StatInteraction))
            {
                Statistic stat = intr.GetStatistic();
                int pointChange = intr.GetPointPenalty();
                if (stat == Statistic.HEALTH)
                {
                    _game.Health += pointChange;
                    if (pointChange < 0)
                    {
                        TextUpdater($"{intr.Description}\n\nYou lost {Math.Abs(pointChange)} health.");
                        return;
                    }
                    else if (pointChange == 0)
                    {
                        TextUpdater($"{intr.Description}");
                        return;
                    }
                    else
                    {
                        TextUpdater($"{intr.Description}\n\nYou gained {pointChange} health.");
                        return;
                    }
                }
                else
                {
                    _game.Will += pointChange;
                    if (pointChange < 0)
                    {
                        TextUpdater($"{intr.Description}\n\nYou lost {Math.Abs(pointChange)} will.");
                        return;
                    }
                    else if (pointChange == 0)
                    {
                        TextUpdater($"{intr.Description}");
                        return;
                    }
                    else
                    {
                        TextUpdater($"{intr.Description}\n\nYou gained {pointChange} will.");
                        return;
                    }
                }
            }
            else
            {
                TextUpdater("" + intr.Description);
                return;
            }

        }

        private void TextUpdater(string feedback)
        {
            Run text = new Run("\n\n" + feedback);
            text.FontSize = 12;
            tblkGame.Inlines.Add(text);
        }

        private void SceneTextUpdater()
        {
            Run title = new Run("\n\n" + _game.CurrentScene.Heading);
            title.FontSize = 24;
            tblkGame.Inlines.Add(title);

            Run textBody = new Run(_game.CurrentScene.ReturnDynamicDescription());
            textBody.FontSize = 12;
            tblkGame.Inlines.Add(textBody);
        }
    }
}
