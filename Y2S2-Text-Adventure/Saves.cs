using System;
using System.Data.Entity;
using System.Linq;

namespace Y2S2_Text_Adventure
{
    public class Saves : DbContext
    {
        public DbSet<Savedata> Savedatas { get; set; }
        public DbSet<SavedataItem> SavedataItems { get; set; }

        public Saves(string databaseName) : base(databaseName)
        {
       
        }
        public Saves() : base("TextAdventureSaves")
        {

        }
    }
}
