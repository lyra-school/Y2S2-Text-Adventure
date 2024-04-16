using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Y2S2_Text_Adventure
{
    internal enum Direction
    {
        NORTH,
        SOUTH,
        WEST,
        EAST,
        NORTHWEST,
        NORTHEAST,
        SOUTHWEST,
        SOUTHEAST
    }
    internal class Scene
    {
        private class SceneConnection
        {
            public string SceneName { get; set; }
            public Direction Direction { get; set; }

            public SceneConnection(string sceneName, Direction direction)
            {
                SceneName = sceneName;
                Direction = direction;
            }
        }
        public string Name { get; set; }
        public string Heading { get; set; }
        public string Description { get; set; }
        private List<SceneConnection> _connections = new List<SceneConnection>();
        public HashSet<Item> StaticItems { get; set; }
        public List<Item> Items { get; set; }

        public Scene()
        {
            Name = "None";
            StaticItems = new HashSet<Item>();
            Items = new List<Item>();
        }
        public Scene(string name, string heading, string description)
        {
            Name = name;
            Heading = heading;
            Description = description;
            StaticItems = new HashSet<Item>();
            Items = new List<Item>();
        }

        public void AddConnection(string sceneName, Direction sceneDirection)
        {
            if(_connections.Any(connection => connection.Direction == sceneDirection))
            {
                throw new ArgumentException("Duplicate direction found for " + Name + ": " + sceneDirection);
            }
            _connections.Add(new SceneConnection(sceneName, sceneDirection));
        }

        public string GetConnectionName(Direction direction)
        {
            foreach(SceneConnection connection in _connections)
            {
                if(connection.Direction == direction)
                {
                    return connection.SceneName;
                }
            }
            return null;
        }

        public string ReturnDynamicDescription()
        {
            StringBuilder sb = new StringBuilder();
            foreach (Item item in Items)
            {
                sb.Append(item.InSceneDescription);
                sb.Append(" ");
            }
            return $"\n\n{Description}\n\n{sb}";
        }
    }
}
