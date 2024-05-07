using System;
using System.Data.Entity;
using System.Linq;

namespace Y2S2_Text_Adventure
{
    /// <summary>
    /// Database object used for saved games.
    /// </summary>
    public class Saves : DbContext
    {
        // Define tables.
        public DbSet<Savedata> Savedatas { get; set; }
        public DbSet<SavedataItem> SavedataItems { get; set; }

        /// <summary>
        /// Creates a new database with the given name.
        /// </summary>
        /// <param name="databaseName">Name of the new database.</param>
        public Saves(string databaseName) : base(databaseName)
        {
       
        }

        /// <summary>
        /// Connects to the database named "TextAdventureSaves".
        /// </summary>
        public Saves() : base("TextAdventureSaves")
        {

        }
    }
}
