using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Y2S2_Text_Adventure
{

    /// <summary>
    /// Data class used for quick Json.NET deserialization.
    /// </summary>
    internal class SceneData
    {
        public string Name { get; set; }
        public string Heading { get; set; }
        public string Description { get; set; }
        public string[] NeighboringScenes { get; set; }
        public string[] Directions { get; set; }
        public string[] Items { get; set; }
    }
}
