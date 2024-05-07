using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Y2S2_Text_Adventure
{
    /// <summary>
    /// An Interaction is a result of any given command. It stores the specific command it's bound to, flavour text, and any other associated behavior depending on
    /// the type.
    /// </summary>
    internal abstract class Interaction
    {
        // Define properties
        public Command AssociatedCommand { get; set; }
        public string Description { get; set; }
        public string SecondItem { get; set; }

        /// <summary>
        /// Instantiates an Interaction.
        /// </summary>
        /// <param name="associatedCommand">The command that this interaction is bound to.</param>
        /// <param name="description">Flavour text resulting from the interaction.</param>
        /// <param name="secondItem">The other item in a multi-target interaction.</param>
        public Interaction(Command associatedCommand, string description, string secondItem)
        {
            AssociatedCommand = associatedCommand;
            Description = description;
            SecondItem = secondItem;
        }
        
        /// <summary>
        /// Returns an Interaction's target scene.
        /// </summary>
        /// <returns>Name of the target scene. Empty string if not properly implemented.</returns>
        public virtual string GetTargetScene()
        {
            return "";
        }

        /// <summary>
        /// Return a statistic affected by a given Interaction.
        /// </summary>
        /// <returns>A Statistic. Defaults to HEALTH if not properly implemented.</returns>
        public virtual Statistic GetStatistic()
        {
            return Statistic.HEALTH;
        }

        /// <summary>
        /// If applicable, returns an amount that an Interaction's target Statistic should change by.
        /// </summary>
        /// <returns>Integer amount. Defaults to 0 if not properly implemented, or if the call fails a Random chance.</returns>
        public virtual int GetPointPenalty()
        {
            return 0;
        }

        /// <summary>
        /// Returns an Interaction's target GameEffect.
        /// </summary>
        /// <returns>GameEffect. Defaults to NONE if not properly implemented.</returns>
        public virtual GameEffect GetEffect()
        {
            return GameEffect.NONE;
        }

        /// <summary>
        /// Returns an Interaction's Perishable status.
        /// </summary>
        /// <returns>Boolean. Defaults to false if not properly implemented.</returns>
        public virtual bool IsPerishable()
        {
            return false;
        }

        /// <summary>
        /// Returns the resulting item from an Interaction in cases of combinations.
        /// </summary>
        /// <returns>Name of the resulting item. Empty string if not properly implemented.</returns>
        public virtual string GetResultingItem()
        {
            return "";
        }
    }
    internal class GenericInteraction : Interaction
    {
        private GameEffect _associatedEffect;

        /// <summary>
        /// Instantiates a "generic" Interaction, which is the kind that only uses its description and second item to achieve an effect.
        /// </summary>
        /// <param name="associatedCommand">The command that this interaction is bound to.</param>
        /// <param name="description">Flavour text resulting from the interaction.</param>
        /// <param name="secondItem">The other item in a multi-target interaction.</param>
        /// <param name="effect">A generic GameEffect that slightly changes the behaviour of the program.</param>
        public GenericInteraction(Command associatedCommand, string description, string secondItem, GameEffect effect) : base(associatedCommand, description, secondItem)
        {
            _associatedEffect = effect;
        }
        public override GameEffect GetEffect()
        {
            return _associatedEffect;
        }
    }
    internal class AdvanceInteraction : Interaction
    {
        private string _targetScene;

        /// <summary>
        /// Instantiates an Interaction responsible for advancing through scenes independent of the GO command. 
        /// </summary>
        /// <param name="associatedCommand">The command that this interaction is bound to.</param>
        /// <param name="description">Flavour text resulting from the interaction.</param>
        /// <param name="secondItem">The other item in a multi-target interaction.</param>
        /// <param name="targetScene">Name of the scene to move to.</param>
        public AdvanceInteraction(Command associatedCommand, string description, string secondItem, string targetScene) : base(associatedCommand, description, secondItem)
        {
            _targetScene = targetScene;
        }

        public override string GetTargetScene()
        {
            return _targetScene;
        }
    }
    internal class StatInteraction : Interaction
    {
        private Statistic _statisticAffected;
        private int _amount;
        private double _chance;

        // Shared Random object across all instances
        private static Random _rnd = new Random();

        /// <summary>
        /// Instantiates an Interaction responsible for changing a player's statistics.
        /// </summary>
        /// <param name="associatedCommand">The command that this interaction is bound to.</param>
        /// <param name="description">Flavour text resulting from the interaction.</param>
        /// <param name="secondItem">The other item in a multi-target interaction.</param>
        /// <param name="statisticAffected">Statistic that the interaction affects.</param>
        /// <param name="amount">Integer amount by which the affected Statistic should change.</param>
        /// <param name="chance">A percent chance for this change to take place.</param>
        public StatInteraction(Command associatedCommand, string description, string secondItem, Statistic statisticAffected, int amount, double chance) : base(associatedCommand, description, secondItem)
        {
            _statisticAffected = statisticAffected;
            _amount = amount;
            _chance = chance;
        }

        public override int GetPointPenalty()
        {
            double luck = _rnd.NextDouble();
            if(luck <= _chance)
            {
                return _amount;
            }
            return 0;
        }

        public override Statistic GetStatistic()
        {
            return _statisticAffected;
        }
    }
    internal class CombineInteraction : Interaction
    {
        private bool _perishable;
        private string _result;

        /// <summary>
        /// Instantiates an Interaction responsible for the creation of new items.
        /// </summary>
        /// <param name="associatedCommand">The command that this interaction is bound to.</param>
        /// <param name="description">Flavour text resulting from the interaction.</param>
        /// <param name="secondItem">The other item in a multi-target interaction.</param>
        /// <param name="perishable">Whether the item should be "consumed" in execution.</param>
        /// <param name="result">The name of the item to give to the player on execution.</param>
        public CombineInteraction(Command associatedCommand, string description, string secondItem, bool perishable, string result) : base(associatedCommand, description, secondItem)
        {
            _perishable = perishable;
            _result = result;
        }
        public override bool IsPerishable()
        {
            return _perishable;
        }
        public override string GetResultingItem()
        {
            return _result;
        }
    }
}
