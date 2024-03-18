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
        ADVANCE,
        STATUS
    }

    internal enum StatusEffect
    {
        POISON,
        CRIPPLED
    }

    internal enum Statistic
    {
        HEALTH,
        WILL
    }
    internal class Item
    {
        public class Interaction
        {
            public Command AssociatedCommand { get; set; }
            public string Description { get; set; }
            public string SecondItem { get; set; }
            public Interaction(Command associatedCommand, string description, string secondItem)
            {
                AssociatedCommand = associatedCommand;
                Description = description;
                SecondItem = secondItem;
            }
        }
        public class AdvanceInteraction : Interaction
        {
            public string TargetScene {  get; set; }
            public AdvanceInteraction(Command associatedCommand, string description, string secondItem, string targetScene) :base(associatedCommand, description, secondItem)
            {
                TargetScene = targetScene;
            }
        }
        public class StatusInteraction : Interaction
        {
            public StatusEffect Status { get; set; }
            public double? Amount {  get; set; }
            public double Chance { get; set; }
            public int Duration { get; set; }
            public StatusInteraction(Command associatedCommand, string description, string secondItem, StatusEffect status, double? amount, double chance, int duration):base(associatedCommand, description, secondItem)
            {
                Status = status;
                Amount = amount;
                Chance = chance;
                Duration = duration;
            }
        }
        public class StatInteraction : Interaction
        {
            public Statistic StatisticAffected {  get; set; }
            public double Amount {  get; set; }
            public double Chance { get; set; }

            public StatInteraction(Command associatedCommand, string description, string secondItem, Statistic statisticAffected, double amount, double chance):base(associatedCommand, description, secondItem)
            {
                StatisticAffected = statisticAffected;
                Amount = amount;
                Chance = chance;
            }
        }

        public string Name { get; set; }
        public string Description { get; set; }
        public string InSceneDescription { get; set; }
        public HashSet<Interaction> Interactions { get; set; }

        public void AddInteraction(Command cmd, string desc, string sndItem)
        {
            Interactions.Add(new Interaction(cmd, desc, sndItem));
        }
        public void AddInteraction(Command cmd, string desc, string sndItem, string target)
        {
            Interactions.Add(new AdvanceInteraction(cmd, desc, sndItem, target));
        }
        public void AddInteraction(Command cmd, string desc, string sndItem, StatusEffect status, double? amount, double chance, int duration)
        {
            Interactions.Add(new StatusInteraction(cmd, desc, sndItem, status, amount, chance, duration));
        }
        public void AddInteraction(Command cmd, string desc, string sndItem, Statistic stat, double amount, double chance)
        {
            Interactions.Add(new StatInteraction(cmd, desc, sndItem, stat, amount, chance));
        }
    }
}
