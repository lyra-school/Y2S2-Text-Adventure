using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Y2S2_Text_Adventure
{
    internal enum Effect
    {
        NONE,
        STAT_CHANGE,
        ADVANCE
    }

    internal enum Statistic
    {
        HEALTH,
        WILL
    }
    internal enum ItemType
    {
        INVENTORY,
        STATIC
    }
    internal class Item
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string InSceneDescription { get; set; }
        public ItemType Type { get; set; }
        public HashSet<Interaction> Interactions { get; set; }

        private static Interaction _fallbackInteractionSingle = new Interaction(Command.ATTACK, "0", "");
        private static Interaction _fallbackInteractionMultiple = new Interaction(Command.ATTACK, "1", "");

        public Item()
        {
            Name = "None";
            Interactions = new HashSet<Interaction>();
        }
        public Item(string name, string desc, string insc, ItemType type)
        {
            Name = name;
            Description = desc;
            InSceneDescription = insc;
            Type = type;
            Interactions = new HashSet<Interaction>();
        }
        public void AddInteraction(Command cmd, string desc, string sndItem)
        {
            Interactions.Add(new Interaction(cmd, desc, sndItem));
        }
        public void AddInteraction(Command cmd, string desc, string sndItem, string target)
        {
            Interactions.Add(new AdvanceInteraction(cmd, desc, sndItem, target));
        }
        public void AddInteraction(Command cmd, string desc, string sndItem, Statistic stat, int amount, double chance)
        {
            Interactions.Add(new StatInteraction(cmd, desc, sndItem, stat, amount, chance));
        }

        public Interaction ReturnInteraction(Command cmd, Item secondItem)
        {
            if(secondItem.Name != "None")
            {
                foreach(Interaction interaction in Interactions)
                {
                    if(interaction.AssociatedCommand == cmd && interaction.SecondItem == secondItem.Name)
                    {
                        return interaction;
                    }
                }
                return _fallbackInteractionMultiple;
            } else
            {
                foreach (Interaction interaction in Interactions)
                {
                    if (interaction.AssociatedCommand == cmd)
                    {
                        return interaction;
                    }
                }
                return _fallbackInteractionSingle;
            }
        }
        public override string ToString()
        {
            return Name;
        }
    }
}
