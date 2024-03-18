using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Y2S2_Text_Adventure
{
    internal class SceneConnection
    {
        public string SceneName {  get; set; }
        public Direction Direction { get; set; }

        public SceneConnection(string sceneName, Direction direction)
        {
            SceneName = sceneName;
            Direction = direction;
        }
    }
}
