using Y2S2_Text_Adventure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbCreator
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Saves sav = new Saves("TextAdventureSaves");
            using (sav)
            {
                Savedata testSave = new Savedata() { Name = "Sample Save", DateCreated = DateTime.Now, Health = 10, Will = 10, SceneName = "sceneexample" };
                SavedataItem testItem = new SavedataItem() { SItemName = "invexample", SItemType = "Inventory" };
                testSave.Items.Add(testItem);
                sav.Savedatas.Add(testSave);
                sav.SaveChanges();
            }
        }
    }
}
