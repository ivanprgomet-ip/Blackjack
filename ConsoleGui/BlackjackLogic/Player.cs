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
        //protected internal: makes _Hand accessible in Blackjack class and for HumanPlayer which is in another assembly
        protected internal Hand _Hand { get; }
        public string Name { get; }
        internal int Balance { get; set; }

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
