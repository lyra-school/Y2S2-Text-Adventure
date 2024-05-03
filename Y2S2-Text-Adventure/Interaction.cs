using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Y2S2_Text_Adventure
{
    internal abstract class Interaction
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
        
        public virtual string GetTargetScene()
        {
            return "";
        }
        public virtual Statistic GetStatistic()
        {
            return Statistic.HEALTH;
        }
        public virtual int GetPointPenalty()
        {
            return 0;
        }
        public virtual Effect GetEffect()
        {
            return Effect.NONE;
        }
        public virtual bool IsPerishable()
        {
            return false;
        }
        public virtual string GetResultingItem()
        {
            return "";
        }
    }
    internal class GenericInteraction : Interaction
    {
        private Effect _associatedEffect;
        public GenericInteraction(Command associatedCommand, string description, string secondItem, Effect effect) : base(associatedCommand, description, secondItem)
        {
            _associatedEffect = effect;
        }
        public override Effect GetEffect()
        {
            return _associatedEffect;
        }
    }
    internal class AdvanceInteraction : Interaction
    {
        private string _targetScene;
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

        private static Random _rnd = new Random();

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
