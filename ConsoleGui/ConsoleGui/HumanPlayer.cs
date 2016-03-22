using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlackjackLogic;

namespace ConsoleGui
{
    class HumanPlayer :RegularPlayer
    {
        public HumanPlayer(string name):base(name)
        {

        }

        public override PlayerDecision MakeDecision()
        {
            if (Rules.GethandValue(Hand) >= 21)
                return PlayerDecision.Stay;
            
            Console.Write($"{Name}: hit/stay (h/s)? >> ");
            return Console.ReadLine() == "h" ? PlayerDecision.Hit : PlayerDecision.Stay;
        }
        public override int MakeBet()
        {
            /*
            Betting process for a human player.
            Will only return bets between 1 and 10.
            */
            var invalidBet = true;
            var bet = 0;

            do
            {
                Console.Write($"{Name}: enter bet between 1-10$ >> ");
                while (!int.TryParse(Console.ReadLine(), out bet))
                    Console.WriteLine("Please enter bet between 1-10$ >> ");
                if (bet < 1 || bet > 10)
                {
                    Console.BackgroundColor = ConsoleColor.DarkRed;
                    Console.WriteLine($"{Name}: Bet is not 1-10$");
                    Console.ResetColor();
                    invalidBet = true;
                }
                else
                    invalidBet = false;
            } while (invalidBet);
            return bet;
        }
    }
}
