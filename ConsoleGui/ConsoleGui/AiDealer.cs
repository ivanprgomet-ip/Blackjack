using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleGui
{
    public class AiDealer : IPlayer
    {
        public Guid Id { get; }
        public Hand Hand { get; }
        public string Name { get; }
        public AiDealer(string name="dealer")
        {
            Name = name;
            Id = Guid.NewGuid();
            Hand = new Hand();
        }

        public PlayerDecision MakeDecision(Hand hand)
        {
            if(Rules.GethandValue(Hand) >= 17)
                return PlayerDecision.Stay;
            return PlayerDecision.Hit;
        }
        public int MakeBet()
        {
            var cash = Bank.GetPlayerMoney(Id);

            if (cash <= 5)
                return 1;//ai will only bet 1$ until bankrupt 
            if (cash <= 20)
                return 5;
            if (cash <= 50)
                return 7;
            if (cash <= 100)
                return 10;
            return 10;
        }
    }
}
