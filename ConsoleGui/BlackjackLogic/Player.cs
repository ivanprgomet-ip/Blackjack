using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleGui
{
    public enum PlayerDecision
    {
        Hit,
        Stay
    }
    public abstract class Player
    {
        protected Hand _Hand { get; }
        protected string Name { get; }
        internal int Balance { get; }

        public Player(string name)
        {
            Name = name;
            _Hand = new Hand();
        }
        public Player()
        {
            _Hand = new Hand();
        }
        public abstract PlayerDecision MakeDecision();
    }
}
