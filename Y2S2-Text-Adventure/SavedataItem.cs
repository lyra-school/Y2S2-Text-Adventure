using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Y2S2_Text_Adventure
{
    public class SavedataItem
    {
        public int SavedataItemId {  get; set; }
        public string SItemName {  get; set; }
        public string SItemType { get; set; }

        public int SavedataId { get; set; }
        public virtual Savedata Save { get; set; }
    }
}
