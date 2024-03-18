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
        public string Name { get; set; }
        public string Heading { get; set; }
        public string Description { get; set; }
        private List<SceneConnection> _connections = new List<SceneConnection>();
        private HashSet<Item> _items = new HashSet<Item>();

        public Scene(string name, string heading, string description)
        {
            Name = name;
            Heading = heading;
            Description = description;
        }

        public void AddConnection(string sceneName, Direction sceneDirection)
        {
            if(_connections.Any(connection => connection.Direction == sceneDirection))
            {
                throw new ArgumentException("Duplicate direction found for " + Name + ": " + sceneDirection);
            }
            _connections.Add(new SceneConnection(sceneName, sceneDirection));
        }
        public void AddItem(Item item)
        {
            _items.Add(item);
        }
        public void RemoveItem(Item item)
        {
            _items.Remove(item);
        }
    }
}
