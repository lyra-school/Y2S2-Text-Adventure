using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Y2S2_Text_Adventure
{
    /// <summary>
    /// Allowed directions for the GO command.
    /// </summary>
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
    /// <summary>
    /// Representation of an area in the game. Holds its description, items within, and connections with other areas.
    /// </summary>
    internal class Scene
    {
        /// <summary>
        /// Object private to a given Scene. Stores direction and the name of the scene it connects to.
        /// </summary>
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
        public List<Item> Items { get; set; }

        /// <summary>
        /// Instantiates an empty Scene with the name "None".
        /// </summary>
        public Scene()
        {
            Name = "None";
            Items = new List<Item>();
        }

        /// <summary>
        /// Instantiates a Scene with the given information.
        /// </summary>
        /// <param name="name">Internal name of the scene.</param>
        /// <param name="heading">Name of the scene as it shows up in the TextBlock.</param>
        /// <param name="description">Flavour text (minus the in-scene descriptions of non-static items).</param>
        public Scene(string name, string heading, string description)
        {
            Name = name;
            Heading = heading;
            Description = description;
            Items = new List<Item>();
        }

        /// <summary>
        /// Creates a link between the current Scene and another one.
        /// </summary>
        /// <param name="sceneName">The internal name of the connecting Scene.</param>
        /// <param name="sceneDirection">The Direction in which it can be found.</param>
        /// <exception cref="ArgumentException"></exception>
        public void AddConnection(string sceneName, Direction sceneDirection)
        {
            // Throws an exception if a direction is already in use; error with configuration.
            if(_connections.Any(connection => connection.Direction == sceneDirection))
            {
                throw new ArgumentException("Duplicate direction found for " + Name + ": " + sceneDirection);
            }
            _connections.Add(new SceneConnection(sceneName, sceneDirection));
        }

        /// <summary>
        /// Returns the internal name of a connecting scene based on the given Direction.
        /// </summary>
        /// <param name="direction">Direction to look for.</param>
        /// <returns>Internal name of a connecting scene.</returns>
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

        /// <summary>
        /// Returns a "dynamic" description of the scene. This includes the "baseline" description as well as that of the items that can be picked up.
        /// </summary>
        /// <returns></returns>
        public string ReturnDynamicDescription()
        {
            StringBuilder sb = new StringBuilder();

            // Add the description of each item that can be picked up.
            foreach (Item item in Items)
            {
                if(item.Type == ItemType.INVENTORY)
                {
                    sb.Append(item.InSceneDescription);
                    sb.Append(" ");
                }
            }

            // To avoid an unnecessary newline, check that item descriptions were added at all.
            if(sb.Length == 0)
            {
                return $"\n\n{Description}";
            } else
            {
                return $"\n\n{Description}\n\n{sb}";
            }
        }
    }
}
