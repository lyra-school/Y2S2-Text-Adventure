using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Y2S2_Text_Adventure
{
    /// <summary>
    /// Table for storing data about Items that are part of each save.
    /// </summary>
    public class SavedataItem
    {
        // Define ID and additional attributes.
        public int SavedataItemId {  get; set; }
        public string SItemName {  get; set; }
        public string SItemType { get; set; }

        // FK and reference to the Savedata table
        public int SavedataId { get; set; }
        public virtual Savedata Save { get; set; }
    }
}
