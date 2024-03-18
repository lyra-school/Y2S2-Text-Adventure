using Newtonsoft.Json;
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
                    Enum.TryParse(deserializedScene.Directions[i], out dir);
                    sc.AddConnection(deserializedScene.NeighboringScenes[i], dir);
                }
            }
        }
        public void ReadAppropriateItems()
        {
            foreach(string filename in Directory.GetFiles("../../data/gamedata/items"))
            {

            }
        }
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
    }
}
