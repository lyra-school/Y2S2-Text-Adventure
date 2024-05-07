using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        // Define the main Game, database and the list for the displayable inventory.
        // Run DbCreator program before running the WPF
        private Game _game = new Game();
        private Saves _db = new Saves();
        private ObservableCollection<Item> _observableInventory = new ObservableCollection<Item>();
        public MainWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Changes Main screen to the How to Play screen on click.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnHow_Click(object sender, RoutedEventArgs e)
        {
            gridMenu.Visibility = Visibility.Collapsed;
            gridHowto.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// Changes How to Play screen to Main screen on click.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnReturnHow_Click(object sender, RoutedEventArgs e)
        {
            gridMenu.Visibility = Visibility.Visible;
            gridHowto.Visibility = Visibility.Collapsed;
        }

        /// <summary>
        /// Changes Main screen to the About screen on click.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAbout_Click(object sender, RoutedEventArgs e)
        {
            gridMenu.Visibility = Visibility.Collapsed;
            gridAbout.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// Changes About screen to the Main screen on click.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnReturnAbout_Click(object sender, RoutedEventArgs e)
        {
            gridMenu.Visibility = Visibility.Visible;
            gridAbout.Visibility = Visibility.Collapsed;
        }

        /// <summary>
        /// Changes Main screen to the Game screen on click. Also causes the game to load all the files, show text for the first scene,
        /// update the statistics TextBlock as appropriate and bind the inventory list to the actual ListBox.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            gridMenu.Visibility = Visibility.Collapsed;
            gridGame.Visibility = Visibility.Visible;

            try
            {
                _game.ReadScenes();
            } catch(Exception ex)
            {
                throw new ArgumentException("Error loading files - improper formatting?");
            }
            
            SceneTextUpdater();
            StatUpdateEvaluate(Statistic.HEALTH);
            StatUpdateEvaluate(Statistic.WILL);
            lbxInventory.ItemsSource = _observableInventory;
        }

        /// <summary>
        /// Toggles visibility of the statistics list.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnStats_Click(object sender, RoutedEventArgs e)
        {
            if(bdStats.Visibility == Visibility.Visible)
            {
                bdStats.Visibility = Visibility.Collapsed;
            } else
            {
                bdStats.Visibility = Visibility.Visible;
            }
        }

        /// <summary>
        /// Toggles visibility of the inventory list.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnInventory_Click(object sender, RoutedEventArgs e)
        {
            if(bdInventory.Visibility == Visibility.Visible)
            {
                bdInventory.Visibility = Visibility.Collapsed;
            } else
            {
                bdInventory.Visibility = Visibility.Visible;
            }
        }

        /// <summary>
        /// Removes text from the prompt when clicked.
        /// Also changes font to Normal as placeholder text is in Italic.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tbxPrompt_GotFocus(object sender, RoutedEventArgs e)
        {
            tbxPrompt.FontStyle = FontStyles.Normal;
            tbxPrompt.Text = "";
        }

        /// <summary>
        /// Saves the current game state in a new entry when clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            // Get essential info about the state of the Game object. The name of these saves are called "Regular Save"s to distinguish them from the one generated by DbCreator.
            Savedata sv = new Savedata() { Name = "Regular Save", DateCreated = DateTime.Now, Health = _game.Health, Will = _game.Will, SceneName = _game.CurrentScene.Name };

            // Get essential info about every Item that has been interacted with in a way that would result in it getting removed from a scene.
            foreach (Item it in _game.Inventory)
            {
                SavedataItem sdi = new SavedataItem() { SItemName = it.Name, SItemType = "Inventory" };
                sv.Items.Add(sdi);
            }
            foreach (Item it in _game.UsedItems)
            {
                SavedataItem sdi = new SavedataItem() { SItemName = it.Name, SItemType = "Used" };
                sv.Items.Add(sdi);
            }

            // Add and save to the DB.
            _db.Savedatas.Add(sv);
            _db.SaveChanges();
        }

        /// <summary>
        /// Evaluates the contents of the prompt TextBox when the Send button is clicked. Displays appropriate text for the targeted Interaction.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnPrompt_Click(object sender, RoutedEventArgs e)
        {
            // Get user input from the prompt TextBox. If empty, do nothing.
            string command = tbxPrompt.Text;
            if (String.IsNullOrEmpty(command))
            {
                return;
            }

            // Parse through each segment of user input, split by a space
            command = command.ToLower();
            string[] commandComponents = command.Split(' ');
            Command cmd;

            // If first word is not a recognized command, sends an error message.
            bool exists = Enum.TryParse(commandComponents[0].ToUpper(), out cmd);
            if (!exists)
            {
                TextUpdater("Could not find command: " + commandComponents[0]);
                return;
            }

            // LOOK on its own repeats the scene description.
            if (cmd == Command.LOOK && commandComponents.Length == 1)
            {
                TextUpdater("You take a look again.");
                SceneTextUpdater();
                return;
            }

            if (cmd == Command.GO)
            {
                // GO always needs to have a direction; anything below two arguments is thus invalid.
                if (commandComponents.Length < 2)
                {
                    TextUpdater("Go where?");
                    return;
                }

                // If second word is not a recognized direction, sends an error message.
                Direction dir;
                bool exists2 = Enum.TryParse(commandComponents[1].ToUpper(), out dir);
                if (!exists2)
                {
                    TextUpdater("Not a recognized direction: " + commandComponents[1]);
                    return;
                }

                // If the current scene doesn't have a connection (link to another scene) in the wanted direction, sends an error message.
                string nextScene = _game.CurrentScene.GetConnectionName(dir);
                if (String.IsNullOrEmpty(nextScene))
                {
                    TextUpdater("You can't find a way out in that direction.");
                    return;
                }
                else
                {
                    // If no fault is caught, changes a player's target scene to the one in the appropriate direction. Scene description is sent for the new one.
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

            // Find items for any further evaluations, and record whether they were found in the inventory or not.
            Item targetItem = new Item();
            Item secondItem = new Item();
            bool tItemInInv = false;
            bool sItemInInv = false;
            try
            {
                // If an item is not found (second argument), sends an error message.
                targetItem = _game.ItemFinder(commandComponents[1], out tItemInInv);
                if (targetItem.Name == "None")
                {
                    TextUpdater("Item not found: " + commandComponents[1] + ".");
                    return;
                }

                // Only executes if the input is trying to look for two items (implied by there being more than two arguments), performs same check for the second item.
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
                // Commands other than LOOK can't be used on their own. As such, trying to retrieve items results in an IndexOutOfBoundsException otherwise -- sends an error message in lieu of that.
                TextUpdater("The command " + cmd.ToString() + " is missing a parameter.");
                return;
            }

            // LOOK and TAKE cannot target more than one time. Sends an error message if this is breached.
            if (secondItem.Name != "None" && (cmd == Command.LOOK || cmd == Command.TAKE))
            {
                TextUpdater($"The {cmd} command cannot be used with more than one item.");
                return;
            }
            else if (cmd == Command.LOOK)
            {
                // LOOK at the target item.
                TextUpdater(targetItem.Description);
                return;
            } else if(cmd == Command.TAKE)
            {
                // STATIC items are forbidden from being picked up; sends an error message if attempt is made despite.
                if(targetItem.Type == ItemType.STATIC)
                {
                    TextUpdater("You can't pick up this item!");
                    return;
                }

                // Since both scene and inventory items are being evaluated, sends an error message if an attempt is made to pick up an item from the inventory.
                if(tItemInInv)
                {
                    TextUpdater("This item is already in your inventory.");
                    return;
                } else
                {
                    // If no fault is caught, remove item from the scene and add to player's inventory.
                    _game.CurrentScene.Items.Remove(targetItem);
                    _game.Inventory.Add(targetItem);

                    // Hacky way of refreshing the ObservableCollection; the ListBox doesn't do so automatically for unknown reasons.
                    _observableInventory = new ObservableCollection<Item>(_game.Inventory);
                    lbxInventory.ItemsSource = _observableInventory;
                }
            }
            // Get requested interaction (command effects) for the item
            Interaction intr = targetItem.ReturnInteraction(cmd, secondItem);

            // Evaluate error signals from above method, then translate them into appropriate text if found.
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

            // Get the exact kind of Interaction it is and compare
            Type kindOfInteraction = intr.GetType();
            if (kindOfInteraction == typeof(AdvanceInteraction))
            {
                // As AdvanceInteractions handle movement across scenes, find the scene to transition to, change player's target scene, and display appropriate text.
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
                // For a StatInteraction, get its target and amount of change.
                Statistic stat = intr.GetStatistic();
                int pointChange = intr.GetPointPenalty();

                // Different flavour text depending on the statistic and the amount.
                if (stat == Statistic.HEALTH)
                {
                    _game.Health += pointChange;
                    if (pointChange < 0)
                    {
                        TextUpdater($"{intr.Description}\n\nYou lost {Math.Abs(pointChange)} health.");
                    }
                    else if (pointChange == 0)
                    {
                        TextUpdater($"{intr.Description}");
                    }
                    else
                    {
                        TextUpdater($"{intr.Description}\n\nYou gained {pointChange} health.");
                    }
                    StatUpdateEvaluate(stat);
                    return;
                }
                else
                {
                    _game.Will += pointChange;
                    if (pointChange < 0)
                    {
                        TextUpdater($"{intr.Description}\n\nYou lost {Math.Abs(pointChange)} will.");
                    }
                    else if (pointChange == 0)
                    {
                        TextUpdater($"{intr.Description}");
                    }
                    else
                    {
                        TextUpdater($"{intr.Description}\n\nYou gained {pointChange} will.");
                    }
                    StatUpdateEvaluate(stat);
                    return;
                }
            }
            else if(kindOfInteraction == typeof(CombineInteraction))
            {
                // For CombineInteractions, both items in question must have the exact same interaction as each other's second target. Sends an error message if this is not the case.
                Interaction intr2 = secondItem.ReturnInteraction(cmd, targetItem);
                if(intr2.Description == "1")
                {
                    TextUpdater("Combination interactions are improperly set up for this item; the second argument doesn't point back to its partner.");
                    return;
                }
                // If any of the two items is marked as Perishable, it's removed from the game.
                if(intr.IsPerishable())
                {
                    if(tItemInInv)
                    {
                        _game.Inventory.Remove(targetItem);
                        _game.UsedItems.Add(targetItem);
                    } else
                    {
                        _game.CurrentScene.Items.Remove(targetItem);
                        _game.UsedItems.Add(targetItem);
                    }
                }
                if(intr2.IsPerishable())
                {
                    if (sItemInInv)
                    {
                        _game.Inventory.Remove(secondItem);
                        _game.UsedItems.Add(secondItem);
                    }
                    else
                    {
                        _game.CurrentScene.Items.Remove(secondItem);
                        _game.UsedItems.Add(secondItem);
                    }
                }

                // Get the resulting item, show an error message if it's not found.
                Item craft = new Item();
                foreach(Item craftedItem in _game.Crafted)
                {
                    if(craftedItem.Name == intr.GetResultingItem())
                    {
                        craft = craftedItem;
                        break;
                    }
                }
                if(craft.Name == "None")
                {
                    TextUpdater("Crafted item not found. This is an error.");
                }

                // Crafted items are always added to the inventory.
                _game.Inventory.Add(craft);
                _observableInventory = new ObservableCollection<Item>(_game.Inventory);
                lbxInventory.ItemsSource = _observableInventory;
                TextUpdater($"{intr.Description}");
            }
            else
            {
                // Defaults to GenericInteraction behaviour
                TextUpdater("" + intr.Description);

                // If the GenericInteraction is for FINAL, end the game.
                if (intr.GetEffect() == GameEffect.FINAL)
                {
                    DisableCmdline();
                }
                return;
            }

        }

        /// <summary>
        /// Formats and adds text to a relevant TextBlock.
        /// </summary>
        /// <param name="feedback">Text to display on the screen.</param>
        private void TextUpdater(string feedback)
        {
            Run text = new Run("\n\n" + feedback);
            text.FontSize = 12;
            tblkGame.Inlines.Add(text);
        }

        /// <summary>
        /// Displays details about the player's current scene.
        /// </summary>
        private void SceneTextUpdater()
        {
            // Scene name should always be bigger than regular text.
            // Originally intended to be in a custom font, however, WPF doesn't support doing so programmatically without jumping through hoops.
            Run title = new Run("\n\n" + _game.CurrentScene.Heading);
            title.FontSize = 24;
            tblkGame.Inlines.Add(title);

            Run textBody = new Run(_game.CurrentScene.ReturnDynamicDescription());
            textBody.FontSize = 12;
            tblkGame.Inlines.Add(textBody);
        }

        /// <summary>
        /// Updates a relevant statistic in the statistics view and evaluates whether it has fallen below an acceptable threshold; ends the game if so.
        /// </summary>
        /// <param name="statistic">The statistic to update.</param>
        private void StatUpdateEvaluate(Statistic statistic)
        {
            // Updates and evaluates Will by default
            switch(statistic)
            {
                case Statistic.HEALTH:
                    int currHealth = _game.Health;
                    rnHealth.Text = "\nHealth: " + currHealth;
                    if(currHealth <= 0)
                    {
                        TextUpdater("You ran out of health! Game over.");
                        DisableCmdline();
                    }
                    break;
                default:
                    int currWill = _game.Will;
                    rnWill.Text = "\nWill: " + currWill;
                    if(currWill <= 0)
                    {
                        TextUpdater("You no longer have the willpower to go on! Game over.");
                        DisableCmdline();
                    }
                    break;
            }
        }

        /// <summary>
        /// Disables the command prompt and the send button, signifying a game over. If done through a FINAL interaction, such should be signified in the command output itself.
        /// </summary>
        private void DisableCmdline()
        {
            tbxPrompt.IsEnabled = false;
            btnPrompt.IsEnabled = false;
        }

        /// <summary>
        /// On click, displays the smaller ThemeSelection window on top of the main one and blocks the main window.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnTheme_Click(object sender, RoutedEventArgs e)
        {
            ThemeSelection themeWindow = new ThemeSelection();
            themeWindow.ShowDialog();
        }

        /// <summary>
        /// Changes Main screen to the Load screen. Also fetches and displays all save entries in the database.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnLoad_Click(object sender, RoutedEventArgs e)
        {
            gridMenu.Visibility = Visibility.Collapsed;

            var query = from s in _db.Savedatas
                        select s;

            lbxSaves.ItemsSource = query.ToList();

            gridLoad.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// Changes Load screen to Main screen.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnReturnSaves_Click(object sender, RoutedEventArgs e)
        {
            gridLoad.Visibility = Visibility.Collapsed;
            gridMenu.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// When clicked, it provides an alternative way of loading the game based on the save selected. Also changes the Load screen to the Game screen.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <exception cref="ArgumentException"></exception>
        private void btnLoadSaves_Click(object sender, RoutedEventArgs e)
        {
            // Get save; do nothing if none selected.
            Savedata selected = (Savedata)lbxSaves.SelectedItem;
            if(selected == null)
            {
                return;
            }
            gridLoad.Visibility = Visibility.Collapsed;
            gridGame.Visibility = Visibility.Visible;

            // Use method overloads specifically geared towards loading files with restrictions from saving.
            try
            {
                _game.ReadScenes(selected);
                _game.UpdateStatsFromSave(selected);
            }
            catch (Exception ex)
            {
                throw new ArgumentException("Error loading files - improper formatting?");
            }

            SceneTextUpdater();
            StatUpdateEvaluate(Statistic.HEALTH);
            StatUpdateEvaluate(Statistic.WILL);

            // Hacky way of refreshing the ObservableCollection; the ListBox doesn't do so automatically for unknown reasons.
            _observableInventory = new ObservableCollection<Item>(_game.Inventory);
            lbxInventory.ItemsSource = _observableInventory;
        }
    }
}
