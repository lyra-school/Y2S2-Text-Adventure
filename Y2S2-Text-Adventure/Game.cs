using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
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
        private HashSet<Scene> _scenes = new HashSet<Scene>();
        private Scene _currentScene;
        public Game() {
            
        }

        public void ReadScenes()
        {
            foreach(string filename in Directory.GetFiles("../../data/gamedata/scenes")) {
                SceneData deserializedScene = JsonConvert.DeserializeObject<SceneData>(filename);
                Scene sc = new Scene(deserializedScene.Name, deserializedScene.Heading, deserializedScene.Description);
                _scenes.Add(sc);
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
            string[] filenames = Directory.GetFiles("../../data/gamedata/items");
            for (int i = 0; i < filenames.Length; i++)
            {
                JObject item = JObject.Parse(filenames[i]);
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
                        LoadInteraction(it, Command.ATTACK, item["AttackInteraction"]);
                        LoadInteraction(it, Command.EAT, item["EatInteraction"]);
                        LoadInteraction(it, Command.TAKE, item["TakeInteraction"]);
                        LoadInteraction(it, Command.USE, item["UseInteraction"]);
                        List<JToken> results = item["SpecialInteractions"].Children().ToList();
                        foreach(JToken token in results)
                        {
                            LoadSpecialInteraction(it, token);
                        }
                    }
                }
            }
        }
        public void LoadInteraction(Item it, Command cmd, JToken interaction)
        {
            string cons = interaction["Consequence"].ToString();
            Effect ef;
            bool success = Enum.TryParse(cons, out ef);
            if(!success)
            {
                throw new ArgumentException("Unrecognized item effect: " + cons);
            }

        }
        public void LoadSpecialInteraction(Item it, JToken interaction)
        {

        }
        /*
        public string CommandFeedback(string command)
        {
            string[] commandComponents = command.Split(' ');
            Command cmd;
            bool exists = Enum.TryParse(commandComponents[0].ToUpper(), out cmd);
            if(!exists)
            {
                return "Could not find command: " + commandComponents[0];
            }
            switch(cmd)
            {

            }
        }
        */
    }
}
