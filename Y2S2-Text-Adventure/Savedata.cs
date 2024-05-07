using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Y2S2_Text_Adventure
{
    /// <summary>
    /// Main table for storing information about each save.
    /// </summary>
    public class Savedata
    {
        // Define ID and additional attributes. Name is used to indicate type of save, while the date is used for display purposes.
        public int SavedataId { get; set; }
        public string Name { get; set; }
        public DateTime DateCreated { get; set; }

        // Data specific to the state of the game.
        public int Health {  get; set; }
        public int Will { get; set; }
        public string SceneName { get; set; }

        // Link to the items table.
        public virtual List<SavedataItem> Items { get; set; }

        /// <summary>
        /// Instantiates a new list of connected items on creation.
        /// </summary>
        public Savedata()
        {
            Items = new List<SavedataItem>();
        }

        /// <summary>
        /// Used for display in a ListBox.
        /// </summary>
        /// <returns>ID, name and date when the save was created.</returns>
        public override string ToString()
        {
            return $"{SavedataId} - {Name} - {DateCreated}";
        }
    }
}
