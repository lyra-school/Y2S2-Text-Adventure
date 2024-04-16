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
    internal class Game
    {
        public HashSet<Scene> Scenes { get; set; }
        private int _health = 10;
        private int _will = 10;
        public Scene CurrentScene { get; set; }
        private HashSet<Item> _inventory = new HashSet<Item>();
        public Game() {
            
        }

        public void ReadScenes()
        {
            string[] paths = Directory.GetFiles(Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName, @"data\gamedata\scenes"));
            foreach (string filename in paths) {
                SceneData deserializedScene;
                using (StreamReader r = new StreamReader(filename))
                {
                    string json = r.ReadToEnd();
                    deserializedScene = JsonConvert.DeserializeObject<SceneData>(json);
                }
                Scene sc = new Scene(deserializedScene.Name, deserializedScene.Heading, deserializedScene.Description);
                Scenes.Add(sc);
                if(sc.Name == "SceneExample")
                {
                    CurrentScene = sc;
                }
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
                ReadAppropriateItems(sc, deserializedScene.Items);
            }
        }
        public void ReadAppropriateItems(Scene sc, string[] itemKeys)
        {
            string[] paths = Directory.GetFiles(Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName, @"data\gamedata\items"));
            List<string> jsons = new List<string>();
            foreach(string path in paths)
            {
                using (StreamReader r = new StreamReader(path))
                {
                    string json = r.ReadToEnd();
                    jsons.Add(json);
                }
            }
            for (int i = 0; i < jsons.Count; i++)
            {
                JObject item = JObject.Parse(jsons[i]);
                for(int j = 0; j < itemKeys.Length; j++)
                {
                    if (item["Name"].Equals(itemKeys[j]))
                    {
                        string name = item["Name"].ToString();
                        string desc = item["Description"].ToString();
                        string type = item["Type"].ToString();
                        string insc = item["InSceneDescription"].ToString();
                        ItemType ttype;
                        bool success = Enum.TryParse(type, out ttype);
                        if(!success)
                        {
                            throw new ArgumentException("Unrecognized item type: " + type);
                        }
                        Item it = new Item(name, desc, insc, ttype);
                        if(it.Type == ItemType.INVENTORY)
                        {
                            sc.Items.Add(it);
                        }
                        else
                        {
                            sc.StaticItems.Add(it);
                        }
                        List<JToken> results = item["Interactions"].Children().ToList();
                        foreach(JToken token in results)
                        {
                            LoadInteraction(it, token);
                        }
                    }
                }
            }
        }
        public void LoadInteraction(Item it, JToken interaction)
        {
            string cons = interaction["Consequence"].ToString();
            Effect ef;
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
            switch(ef)
            {
                case Effect.NONE:
                    it.AddInteraction(act, interaction["Description"].ToString(), interaction["SecondTarget"].ToString());
                    return;
                case Effect.ADVANCE:
                    it.AddInteraction(act, interaction["Description"].ToString(), interaction["SecondTarget"].ToString(), interaction["Penalty"].ToString());
                    return;
                case Effect.STAT_CHANGE:
                    Statistic stat;
                    bool success4 = Enum.TryParse(interaction["Penalty"].ToString(), out stat);
                    if (!success4)
                    {
                        throw new ArgumentException("Unrecognized status effect:" + interaction["Penalty"].ToString());
                    }
                    int amt2;
                    string a2 = interaction["PenaltyAmount"].ToString();
                    if (String.IsNullOrEmpty(a2))
                    {
                        a2 = "0";
                    }
                    amt2 = int.Parse(a2);
                    double chance2;
                    string b2 = interaction["PenaltyChance"].ToString();
                    if (String.IsNullOrEmpty(b2))
                    {
                        b2 = "1";
                    }
                    chance2 = double.Parse(b2);
                    it.AddInteraction(act, interaction["Description"].ToString(), interaction["SecondTarget"].ToString(), stat, amt2, chance2);
                    return;
            }
        }
        public void StatUpdater(Statistic stat, int amount)
        {
            if()
        }

        public Item ItemFinder(string target)
        {
            Item foundItem = new Item();
            foreach (Item item in CurrentScene.Items)
            {
                if (item.Name == target)
                {
                    foundItem = item;
                }
            }
            foreach (Item item in _inventory)
            {
                if (item.Name == target)
                {
                    foundItem = item;
                }
            }
            return foundItem;
        }
    }
}
