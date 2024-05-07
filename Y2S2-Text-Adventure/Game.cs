using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Y2S2_Text_Adventure
{
    /// <summary>
    /// Allowed commands for the user to send.
    /// </summary>
    internal enum Command
    {
        ATTACK,
        EAT,
        GO,
        LOOK,
        TAKE,
        TALK,
        USE
    }

    /// <summary>
    /// Object that powers the game. Stores the loaded scenes, items, player's statistics and inventory.
    /// </summary>
    internal class Game
    {
        // Define properties. All in a HashSet never needs to be sorted and is forbidden from being duplicated.
        public HashSet<Scene> Scenes { get; set; }
        public int Health { get; set; }
        public int Will { get; set; }
        public Scene CurrentScene { get; set; }
        public HashSet<Item> Inventory { get; set; }
        public HashSet<Item> Crafted { get; set; }

        // This collection of "discarded items" exists for the sake of making Savedatas.
        public HashSet<Item> UsedItems { get; set; }

        /// <summary>
        /// Instantiates a game. All games start with 10 Health and 10 Will; everything else is empty by default.
        /// </summary>
        public Game() {
            Health = 10;
            Will = 10;
            Scenes = new HashSet<Scene>();
            Inventory = new HashSet<Item>();
            Crafted = new HashSet<Item>();
            UsedItems = new HashSet<Item>();
            CurrentScene = new Scene();
        }

        /// <summary>
        /// Reads all scenes into the game as-is.
        /// </summary>
        /// <exception cref="ArgumentException"></exception>
        public void ReadScenes()
        {
            // Get data paths of scenes and items.
            string[] paths = Directory.GetFiles(Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName, @"data\gamedata\scenes"));
            string[] itemPaths = Directory.GetFiles(Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName, @"data\gamedata\items"));
            List<string> jsons = new List<string>();
            
            // Read all items.
            foreach (string path in itemPaths)
            {
                using (StreamReader r = new StreamReader(path))
                {
                    string json = r.ReadToEnd();
                    jsons.Add(json);
                }
            }

            // Read all scenes and deserialize into SceneData.
            foreach (string filename in paths) {
                SceneData deserializedScene;
                using (StreamReader r = new StreamReader(filename))
                {
                    string json = r.ReadToEnd();
                    deserializedScene = JsonConvert.DeserializeObject<SceneData>(json);
                }
                Scene sc = new Scene(deserializedScene.Name, deserializedScene.Heading, deserializedScene.Description);

                // Add scene and check for hardcoded starting scenes; matching name = this is set as the first scene in the game.
                Scenes.Add(sc);
                if(sc.Name == "sceneexample")
                {
                    CurrentScene = sc;
                }

                // Add all of the connections. The index of the NeighboringScenes and Directions arrays correspond with each other by design.
                for(int i = 0; i < deserializedScene.NeighboringScenes.Length; i++)
                {
                    Direction dir;
                    bool success = Enum.TryParse(deserializedScene.Directions[i], out dir);
                    if(!success)
                    {
                        throw new ArgumentException("Found unrecognized direction: " + deserializedScene.Directions[i]);
                    }
                    sc.AddConnection(deserializedScene.NeighboringScenes[i], dir);
                }

                // Read all scene-specific items.
                ReadAppropriateItems(sc, jsons, deserializedScene.Items);
            }

            // Read all outlying items.
            ReadOutsideItems(jsons);
        }

        /// <summary>
        /// Read all scenes into the game based on saved information.
        /// </summary>
        /// <param name="sd">The savedata to pull information from.</param>
        /// <exception cref="ArgumentException"></exception>
        public void ReadScenes(Savedata sd)
        {
            // Get data paths of scenes and items.
            string[] paths = Directory.GetFiles(Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName, @"data\gamedata\scenes"));
            string[] itemPaths = Directory.GetFiles(Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName, @"data\gamedata\items"));
            List<string> jsons = new List<string>();

            // Read all items.
            foreach (string path in itemPaths)
            {
                using (StreamReader r = new StreamReader(path))
                {
                    string json = r.ReadToEnd();
                    jsons.Add(json);
                }
            }

            // Read all scenes and deserialize into SceneData.
            foreach (string filename in paths)
            {
                SceneData deserializedScene;
                using (StreamReader r = new StreamReader(filename))
                {
                    string json = r.ReadToEnd();
                    deserializedScene = JsonConvert.DeserializeObject<SceneData>(json);
                }
                Scene sc = new Scene(deserializedScene.Name, deserializedScene.Heading, deserializedScene.Description);
                Scenes.Add(sc);
                // Add scene and check for the name of the current scene in the save; matching name = this is set as the first scene in the game.
                if (sc.Name == sd.SceneName)
                {
                    CurrentScene = sc;
                }

                // Add all of the connections. The index of the NeighboringScenes and Directions arrays correspond with each other by design.
                for (int i = 0; i < deserializedScene.NeighboringScenes.Length; i++)
                {
                    Direction dir;
                    bool success = Enum.TryParse(deserializedScene.Directions[i], out dir);
                    if (!success)
                    {
                        throw new ArgumentException("Found unrecognized direction: " + deserializedScene.Directions[i]);
                    }
                    sc.AddConnection(deserializedScene.NeighboringScenes[i], dir);
                }
                // Read all scene-specific items with restrictions from the savedata.
                ReadAppropriateItems(sc, jsons, deserializedScene.Items, sd.Items);
            }

            // Read all outlying items with restrictions from the savedata.
            ReadOutsideItems(jsons, sd.Items);
        }

        /// <summary>
        /// Reads all scene-specific items and adds it to a Scene.
        /// </summary>
        /// <param name="sc">Parent scene of the items.</param>
        /// <param name="jsons">Item JSON files.</param>
        /// <param name="itemKeys">Items requested by the scene.</param>
        /// <exception cref="ArgumentException"></exception>
        public void ReadAppropriateItems(Scene sc, List<string> jsons, string[] itemKeys)
        {
            for (int i = 0; i < jsons.Count; i++)
            {
                // This method uses "manual" deserialization whereby properties are being accessed from a JOBject, instead of
                // being dumped into a data class. This is because interactions are tricky to deserialize.
                JObject item = (JObject)JsonConvert.DeserializeObject(jsons[i]);

                // Check all items against all names in the Scene's item list.
                for(int j = 0; j < itemKeys.Length; j++)
                {
                    if (item["Name"].ToString().Equals(itemKeys[j]))
                    {
                        string name = item["Name"].ToString();
                        string desc = item["Description"].ToString();
                        string type = item["Type"].ToString();
                        string insc = item["InSceneDescription"].ToString();
                        ItemType ttype;

                        // Do not accept the item if the ItemType is faulty.
                        bool success = Enum.TryParse(type, out ttype);
                        if(!success)
                        {
                            throw new ArgumentException("Unrecognized item type: " + type);
                        }

                        // Add item to the Scene and then add Interactions to it.
                        Item it = new Item(name, desc, insc, ttype);
                        sc.Items.Add(it);
                        List<JToken> results = item["Interactions"].Children().ToList();
                        foreach(JToken token in results)
                        {
                            LoadInteraction(it, token);
                        }
                    }
                }
            }
        }
        /// <summary>
        /// Reads all scene-specific items and adds it to a Scene, with restrictions from a given save data.
        /// </summary>
        /// <param name="sc">Parent scene of the items.</param>
        /// <param name="jsons">Item JSON files.</param>
        /// <param name="itemKeys">Items requested by the scene.</param>
        /// <param name="sdi">All items that are part of a savedata.</param>
        /// <exception cref="ArgumentException"></exception>
        public void ReadAppropriateItems(Scene sc, List<string> jsons, string[] itemKeys, List<SavedataItem> sdi)
        {
            for (int i = 0; i < jsons.Count; i++)
            {
                // This method uses "manual" deserialization whereby properties are being accessed from a JOBject, instead of
                // being dumped into a data class. This is because interactions are tricky to deserialize.
                JObject item = (JObject)JsonConvert.DeserializeObject(jsons[i]);

                for (int j = 0; j < itemKeys.Length; j++)
                {
                    // Check all items against all names in the Scene's item list.
                    if (item["Name"].ToString().Equals(itemKeys[j]))
                    {
                        string name = item["Name"].ToString();
                        string desc = item["Description"].ToString();
                        string type = item["Type"].ToString();
                        string insc = item["InSceneDescription"].ToString();
                        ItemType ttype;

                        // Do not accept the item if the ItemType is faulty.
                        bool success = Enum.TryParse(type, out ttype);
                        if (!success)
                        {
                            throw new ArgumentException("Unrecognized item type: " + type);
                        }

                        // Check if item is found in a Savedata.
                        Item it = new Item(name, desc, insc, ttype);
                        bool found = false;
                        foreach(SavedataItem sit in sdi)
                        {
                            // Add either to inventory or UsedItems if there is a match (defaults to UsedItems.)
                            if(name == sit.SItemName)
                            {
                                string itemT = sit.SItemType;
                                if(itemT == "Inventory")
                                {
                                    Inventory.Add(it);
                                } else
                                {
                                    UsedItems.Add(it);
                                }
                                found = true;
                            }
                        }

                        // If found, don't add to the scene.
                        if(!found)
                        {
                            sc.Items.Add(it);
                        }

                        // Add interactions to the item.
                        List<JToken> results = item["Interactions"].Children().ToList();
                        foreach (JToken token in results)
                        {
                            LoadInteraction(it, token);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Reads all items into a separate collection that aren't part of a Scene, denoted by the PartOfScene field.
        /// </summary>
        /// <param name="jsons">Item JSON files.</param>
        /// <exception cref="ArgumentException"></exception>
        public void ReadOutsideItems(List<string> jsons)
        {
            for(int i = 0; i < jsons.Count;i++)
            {
                // Manually deserialize and skip if PartOfScene is set to true.
                JObject item = (JObject)JsonConvert.DeserializeObject(jsons[i]);
                if (!(bool)item["PartOfScene"])
                {
                    string name = item["Name"].ToString();
                    string desc = item["Description"].ToString();
                    string type = item["Type"].ToString();
                    string insc = item["InSceneDescription"].ToString();
                    ItemType ttype;

                    // Do not accept the item if the ItemType is faulty.
                    bool success = Enum.TryParse(type, out ttype);
                    if (!success)
                    {
                        throw new ArgumentException("Unrecognized item type: " + type);
                    }

                    // Add to the list of not-yet existent Items and add Interactions to it.
                    Item it = new Item(name, desc, insc, ttype);
                    Crafted.Add(it);
                    List<JToken> results = item["Interactions"].Children().ToList();
                    foreach (JToken token in results)
                    {
                        LoadInteraction(it, token);
                    }
                }
            }
        }

        /// <summary>
        /// Reads all items into a separate collection that aren't part of a Scene, denoted by the PartOfScene field.
        /// With restrictions based on a save.
        /// </summary>
        /// <param name="jsons">Item JSON files.</param>
        /// <param name="sdi">All items that are part of a savedata.</param>
        /// <exception cref="ArgumentException"></exception>
        public void ReadOutsideItems(List<string> jsons, List<SavedataItem> sdi)
        {
            for (int i = 0; i < jsons.Count; i++)
            {
                // Manually deserialize and skip if PartOfScene is set to true.
                JObject item = (JObject)JsonConvert.DeserializeObject(jsons[i]);
                if (!(bool)item["PartOfScene"])
                {
                    string name = item["Name"].ToString();
                    string desc = item["Description"].ToString();
                    string type = item["Type"].ToString();
                    string insc = item["InSceneDescription"].ToString();
                    ItemType ttype;

                    // Do not accept the item if the ItemType is faulty.
                    bool success = Enum.TryParse(type, out ttype);
                    if (!success)
                    {
                        throw new ArgumentException("Unrecognized item type: " + type);
                    }

                    // Check if item is found in a Savedata.
                    Item it = new Item(name, desc, insc, ttype);
                    bool found = false;
                    foreach (SavedataItem sit in sdi)
                    {
                        // Add either to inventory or UsedItems if there is a match (defaults to UsedItems.)
                        if (name == sit.SItemName)
                        {
                            string itemT = sit.SItemType;
                            if (itemT == "Inventory")
                            {
                                Inventory.Add(it);
                            }
                            else
                            {
                                UsedItems.Add(it);
                            }
                            found = true;
                        }
                    }

                    // If found, don't add to the list of not-yet existent items.
                    if (!found)
                    {
                        Crafted.Add(it);
                    }

                    // Add Interactions to it.
                    List<JToken> results = item["Interactions"].Children().ToList();
                    foreach (JToken token in results)
                    {
                        LoadInteraction(it, token);
                    }
                }
            }
        }

        /// <summary>
        /// Adds an Interaction to an Item based on deserialized data.
        /// </summary>
        /// <param name="it">The Item to add the Interaction to.</param>
        /// <param name="interaction">A JToken representing a single Interaction.</param>
        /// <exception cref="ArgumentException"></exception>
        public void LoadInteraction(Item it, JToken interaction)
        {

            // Manually deserialize.
            string cons = interaction["Consequence"].ToString();
            GameEffect ef;

            // Invalid Consequence and Action cause the program to end.
            bool success = Enum.TryParse(cons, out ef);
            if(!success)
            {
                throw new ArgumentException("Unrecognized item effect: " + cons);
            }
            Command act;
            string action = interaction["Action"].ToString();
            bool success2 = Enum.TryParse(action, out act);
            if(!success2)
            {
                throw new ArgumentException("Unrecognized action/command: " + action);
            }

            // Creates the type of Interaction based on the data within.
            switch(ef)
            {
                // "Generic" Interactions only need a Description, a SecondTarget, and the effect itself.
                case GameEffect.NONE:
                case GameEffect.FINAL:
                    string snd = VerifySecondItem(interaction);
                    it.AddInteraction(act, interaction["Description"].ToString(), snd, ef);
                    return;

                // Advance Interactions need a Description, a SecondTarget, and the destination.
                case GameEffect.ADVANCE:
                    string snd2 = VerifySecondItem(interaction);
                    it.AddInteraction(act, interaction["Description"].ToString(), snd2, interaction["Penalty"].ToString());
                    return;

                // StatInteractions need a Description, a SecondTarget, affected Statistic, amount and the chance.
                case GameEffect.STAT_CHANGE:

                    // Invalid Penalty causes the program to end.
                    Statistic stat;
                    bool success4 = Enum.TryParse(interaction["Penalty"].ToString(), out stat);
                    if (!success4)
                    {
                        throw new ArgumentException("Unrecognized status effect:" + interaction["Penalty"].ToString());
                    }
                    int amt2;
                    string a2;

                    // If there's no amount given, default to 0.
                    if(interaction["PenaltyAmount"] == null)
                    {
                        a2 = "0";
                    } else
                    {
                        a2 = interaction["PenaltyAmount"].ToString();
                    }
                    amt2 = int.Parse(a2);
                    double chance2;
                    string b2;

                    // If there is no chance given, default to 1 (100%)
                    if (interaction["PenaltyChance"] == null)
                    {
                        b2 = "1";
                    }
                    else
                    {
                        b2 = interaction["PenaltyChance"].ToString();
                    }
                    chance2 = double.Parse(b2);
                    string snd3 = VerifySecondItem(interaction);
                    it.AddInteraction(act, interaction["Description"].ToString(), snd3, stat, amt2, chance2);
                    return;

                // CombineInteractions need a Description, a SecondTarget, and the resulting item.
                case GameEffect.COMBINE:

                    // End the program if Perishable is set to an invalid value.
                    bool perishStatus;
                    bool success5 = Boolean.TryParse(interaction["Perishable"].ToString(), out perishStatus);
                    if (!success5)
                    {
                        throw new ArgumentException("Item in a combination must be marked as either TRUE (deleted when combined) or FALSE in Perishable field.");
                    }
                    string snd4 = VerifySecondItem(interaction);
                    string res = interaction["ResultingItem"].ToString();
                    it.AddInteraction(act, interaction["Description"].ToString(), snd4, perishStatus, res);
                    return;
            }
        }

        /// <summary>
        /// Since SecondTarget is a frequently omitted field, check that it exists and, if not, return an empty string.
        /// </summary>
        /// <param name="jToken">The JToken that represents an Interaction.</param>
        /// <returns>Empty string.</returns>
        private string VerifySecondItem(JToken jToken)
        {
            if (jToken["SecondTarget"] == null)
            {
                return "";
            }
            else
            {
                return jToken["SecondTarget"].ToString();
            }
        }

        /// <summary>
        /// Searches for an item through both the player inventory and the collection in the current scene.
        /// </summary>
        /// <param name="target">Name of an item to search for.</param>
        /// <param name="foundInInventory">Whether the item was found in the inventory or not.</param>
        /// <returns>An Item object based on the given item name.</returns>
        public Item ItemFinder(string target, out bool foundInInventory)
        {
            // Found bool defaults to false.
            foundInInventory = false;
            Item foundItem = new Item();
            foreach (Item item in CurrentScene.Items)
            {
                if (item.Name == target)
                {
                    foundItem = item;
                    break;
                }
            }
            foreach (Item item in Inventory)
            {
                if (item.Name == target)
                {
                    // The bool is only returned as true if the Item is found while cycling through inventory. If there is a conflict,
                    // the Item in the inventory takes precedence.
                    foundItem = item;
                    foundInInventory = true;
                    break;
                }
            }
            return foundItem;
        }

        /// <summary>
        /// Updates the game's base statistics based on information from a save.
        /// </summary>
        /// <param name="sd">Savedata to pull information from.</param>
        public void UpdateStatsFromSave(Savedata sd)
        {
            Health = sd.Health;
            Will = sd.Will;
        }
    }
}
