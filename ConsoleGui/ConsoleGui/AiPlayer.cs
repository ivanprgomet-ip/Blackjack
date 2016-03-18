using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleGui
{
    public class AiPlayer : IPlayer
    {
        public Guid Id { get; }

        public Hand Hand { get; }
        public string Name { get; }
        public AiPlayer(string name)
        {
            Name = name;
            Id = Guid.NewGuid();
            Hand = new Hand();
        }

        public PlayerDecision MakeDecision(Hand hand)
        {
            /*
            TODO: decide if Hand parameter is 
            neccessary

            ComputerPlayer betting-logic is 
            based on bet amount and Hand value:
            */
            var handValue = Rules.GethandValue(Hand);

            if (Bank.GetPlayerBet(Id) > 7)
            {
                if (handValue > 16)
                    return PlayerDecision.Stay;
                else
                    return PlayerDecision.Hit;
            }
            else
            {
                if (handValue > 18)
                    return PlayerDecision.Stay;
                else
                    return PlayerDecision.Hit;
            }
        }
        public int MakeBet()
        {
            var cash = Bank.GetPlayerMoney(Id);

            if (cash <= 5)
                return 1;//ai will only bet 1$ until bankrupt 
            if (cash <= 20)
                return 2;
            if (cash <= 50)
                return 7;
            if (cash <= 100)
                return 10;
            return 10;
        }
    }
}
