using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Y2S2_Text_Adventure
{
    /// <summary>
    /// All possible effects of item interactions.
    /// </summary>
    internal enum GameEffect
    {
        NONE,
        STAT_CHANGE,
        ADVANCE,
        COMBINE,
        FINAL
    }

    /// <summary>
    /// All possible statistics that the player must keep track of.
    /// </summary>
    internal enum Statistic
    {
        HEALTH,
        WILL
    }

    /// <summary>
    /// All possible item types. The type of item determines certain functionality.
    /// STATIC describes all items that cannot be picked up, INVENTORY the otherwise.
    /// </summary>
    internal enum ItemType
    {
        INVENTORY,
        STATIC
    }

    /// <summary>
    /// Object that stores essential information about items found in the game, e.g. flavour text and all possible interactions (available commands.)
    /// </summary>
    internal class Item
    {
        // Declare properties; interactions are never ordered and cannot be duplicate
        public string Name { get; set; }
        public string Description { get; set; }
        public string InSceneDescription { get; set; }
        public ItemType Type { get; set; }
        public HashSet<Interaction> Interactions { get; set; }

        // These static fields are used to signal "errors" in certain class methods. Single is used to indicate an error with a single-target command, otherwise Multiple is used.
        private static Interaction _fallbackInteractionSingle = new GenericInteraction(Command.ATTACK, "0", "", GameEffect.NONE);
        private static Interaction _fallbackInteractionMultiple = new GenericInteraction(Command.ATTACK, "1", "", GameEffect.NONE);

        /// <summary>
        /// Instantiates an item with the name "None" and otherwise empty properties.
        /// </summary>
        public Item()
        {
            Name = "None";
            Interactions = new HashSet<Interaction>();
        }

        /// <summary>
        /// Instantiates an item with the given properties.
        /// </summary>
        /// <param name="name">Name of the item.</param>
        /// <param name="desc">Description with the targeted LOOK command.</param>
        /// <param name="insc">Description as part of its parent scene (isn't used for static and crafted items.)</param>
        /// <param name="type">The type of item. Either STATIC or INVENTORY.</param>
        public Item(string name, string desc, string insc, ItemType type)
        {
            Name = name;
            Description = desc;
            InSceneDescription = insc;
            Type = type;
            Interactions = new HashSet<Interaction>();
        }
        /// <summary>
        /// Adds a "generic" interaction to the item. This is a command that, in config, only accepts flavour text, a second item, and a Consequence at most.
        /// </summary>
        /// <param name="cmd">The Command that corresponds to this interaction.</param>
        /// <param name="desc">The text that is displayed when using this interaction.</param>
        /// <param name="sndItem">The key of the second item if the interaction accepts multiple targets. Use an empty string for a single target.</param>
        /// <param name="effect">Corresponding, generic GameEffect to use with this interaction. Only NONE and FINAL are valid at this time.</param>
        public void AddInteraction(Command cmd, string desc, string sndItem, GameEffect effect)
        {
            Interactions.Add(new GenericInteraction(cmd, desc, sndItem, effect));
        }

        /// <summary>
        /// Adds an interaction to the item that is a command used to advance to a different area, independent of the directional GO command.
        /// </summary>
        /// <param name="cmd">The Command that corresponds to this interaction.</param>
        /// <param name="desc">The text that is displayed when using this interaction.</param>
        /// <param name="sndItem">The key of the second item if the interaction accepts multiple targets. Use an empty string for a single target.</param>
        /// <param name="target">The name of the scene that the setting will change to.</param>
        public void AddInteraction(Command cmd, string desc, string sndItem, string target)
        {
            Interactions.Add(new AdvanceInteraction(cmd, desc, sndItem, target));
        }

        /// <summary>
        /// Adds an interaction to the item that affects one of the player statistics.
        /// </summary>
        /// <param name="cmd">The Command that corresponds to this interaction.</param>
        /// <param name="desc">The text that is displayed when using this interaction.</param>
        /// <param name="sndItem">The key of the second item if the interaction accepts multiple targets. Use an empty string for a single target.</param>
        /// <param name="stat">The Statistic that the command targets.</param>
        /// <param name="amount">The amount with which the command affects the statistic. Positive number restores, while a negative number decreases it.</param>
        /// <param name="chance">The chance for the statistic to get affected by this interaction.</param>
        public void AddInteraction(Command cmd, string desc, string sndItem, Statistic stat, int amount, double chance)
        {
            Interactions.Add(new StatInteraction(cmd, desc, sndItem, stat, amount, chance));
        }

        /// <summary>
        /// Adds an interaction to the item that governs creation of new items.
        /// </summary>
        /// <param name="cmd">The Command that corresponds to this interaction.</param>
        /// <param name="desc">The text that is displayed when using this interaction.</param>
        /// <param name="sndItem">The key of the second item if the interaction accepts multiple targets. Use an empty string for a single target.</param>
        /// <param name="perish">Whether this item will be destroyed after the combination.</param>
        /// <param name="result">The name of the resulting item from the execution of the command.</param>
        public void AddInteraction(Command cmd, string desc, string sndItem, bool perish, string result)
        {
            Interactions.Add(new CombineInteraction(cmd, desc, sndItem, perish, result));
        }

        /// <summary>
        /// Searches through an Item's set of interactions and attempts to retrieve one appropriate for the request.
        /// </summary>
        /// <param name="cmd">The Command that corresponds to the required interaction.</param>
        /// <param name="secondItem">The key of the second item if the required interaction accepts multiple targets. Use an empty string for a single target.</param>
        /// <returns>Appropriate interaction if found, or one of the error signals depending on the type of request.</returns>
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

        /// <summary>
        /// An override used for display in a WPF ListBox.
        /// </summary>
        /// <returns>Name of the item.</returns>
        public override string ToString()
        {
            return Name;
        }
    }
}
