﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleGui
{
    public class HumanPlayer : IPlayer
    {
        public Guid Id { get; }

        public Hand Hand { get; }

        public string Name { get;}

        public HumanPlayer(string name)
        {
            Name = name;
            Id = Guid.NewGuid();
            Hand = new Hand();
        }

        public PlayerDecision MakeDecision(Hand hand)
        {
            if (Rules.GethandValue(Hand) >= 21)
                return PlayerDecision.Stay;

            Console.Write($"{Name}: hit/stay (h/s)? >> ");
            var choice = Console.ReadLine();

            return choice == "h" ? PlayerDecision.Hit : PlayerDecision.Stay;
        }

        // will only return bets between 1 and 10.
        public int MakeBet()
        {
            //Betting process for human players:
            var invalidBet = true;
            var bet = 0;

            do
            {
                Console.Write($"{Name}: enter bet between 1-10$ >> ");

                while (!int.TryParse(Console.ReadLine(), out bet))
                    Console.WriteLine("Please enter bet between 1-10$ >> ");
                if (bet < 1 || bet > 10)
                {
                    Console.WriteLine("Bet not accepted. Place bet between 1-10$ >> ");
                    invalidBet = true;
                }
                else
                    invalidBet = false;
            } while (invalidBet);
            return bet;
        }
    }
}