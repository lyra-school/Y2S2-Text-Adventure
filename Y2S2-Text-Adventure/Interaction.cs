using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Y2S2_Text_Adventure
{
    internal class Interaction
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
    internal class AdvanceInteraction : Interaction
    {
        public string TargetScene { get; set; }
        public AdvanceInteraction(Command associatedCommand, string description, string secondItem, string targetScene) : base(associatedCommand, description, secondItem)
        {
            TargetScene = targetScene;
        }
    }
    internal class StatusInteraction : Interaction
    {
        public StatusEffect Status { get; set; }
        public double Amount { get; set; }
        public double Chance { get; set; }
        public int Duration { get; set; }
        public StatusInteraction(Command associatedCommand, string description, string secondItem, StatusEffect status, double amount, double chance, int duration) : base(associatedCommand, description, secondItem)
        {
            Status = status;
            Amount = amount;
            Chance = chance;
            Duration = duration;
        }
    }
    internal class StatInteraction : Interaction
    {
        public Statistic StatisticAffected { get; set; }
        public double Amount { get; set; }
        public double Chance { get; set; }

        public StatInteraction(Command associatedCommand, string description, string secondItem, Statistic statisticAffected, double amount, double chance) : base(associatedCommand, description, secondItem)
        {
            StatisticAffected = statisticAffected;
            Amount = amount;
            Chance = chance;
        }
    }
}
