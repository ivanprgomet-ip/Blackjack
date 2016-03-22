using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CardLogic;

namespace BlackjackLogic
{
    public enum PlayerDecision
    {
        Hit,
        Stay
    }
    public abstract class Player
    {
        //protected internal: makes _Hand accessible in Blackjack class and for HumanPlayer which is in another assembly
        public Hand Hand { get; }
        public string Name { get; }
        public int Balance { get; set; }

        public Player(string name)
        {
            Name = name;
            Hand = new Hand();
        }
        public Player()
        {
            Hand = new Hand();
        }

        public abstract PlayerDecision MakeDecision();
    }
}
