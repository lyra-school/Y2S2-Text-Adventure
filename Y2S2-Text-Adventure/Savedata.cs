using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Y2S2_Text_Adventure
{
    public class Savedata
    {
        public int SavedataId { get; set; }
        public string Name { get; set; }
        public DateTime DateCreated { get; set; }

        public int Health {  get; set; }
        public int Will { get; set; }
        public string SceneName { get; set; }

        public virtual List<SavedataItem> Items { get; set; }

        public Savedata()
        {
            Items = new List<SavedataItem>();
        }

        public override string ToString()
        {
            return $"{SavedataId} - {Name} - {DateCreated}";
        }
    }
}
