﻿using Y2S2_Text_Adventure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbCreator
{
    internal class Program
    {
        /// <summary>
        /// Small console program used to generate the database. Adds a sample save to the game (with all correct data!)
        /// Run this before running the main WPF program.
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            Saves sav = new Saves("TextAdventureSaves");
            using (sav)
            {
                // Populate row with details, then an additional row containing information about the item data for the save (part of another table that
                // links back to Savedata with a FK.)
                Savedata testSave = new Savedata() { Name = "Sample Save", DateCreated = DateTime.Now, Health = 10, Will = 10, SceneName = "sceneexample" };
                SavedataItem testItem = new SavedataItem() { SItemName = "invexample", SItemType = "Inventory" };
                testSave.Items.Add(testItem);
                sav.Savedatas.Add(testSave);
                sav.SaveChanges();
            }
        }
    }
}
