using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleGui
{
    /// <summary>
    /// humanplayer needs a graphical interface 
    /// to function properly due to the need of
    /// user input. the humanplayer implementation 
    /// will look diferent depending on the type of
    /// graphical interface. in this case it is 
    /// adapted for the console window.
    /// hint: lägg märke till that the other player 
    /// types (aidelaer and aiplayer) must be public 
    /// classes so that the gui class within the guiconsole
    /// project can access the players hand properties.
    /// due to humanplayer actually already beeing inside the 
    /// consolegui project, the class does not need to 
    /// be public, and can therefore be internal insted.
    /// </summary>
    class HumanPlayer :Player,IGambler
    {
        public int Bet { get; private set; }

        public HumanPlayer(string name):base(name)
        {

        }

        public override PlayerDecision MakeDecision()
        {
            if (Rules.GethandValue(_Hand) >= 21)
                return PlayerDecision.Stay;
            
            Console.Write($"{Name}: hit/stay (h/s)? >> ");
            var choice = Console.ReadLine();
            return choice == "h" ? PlayerDecision.Hit : PlayerDecision.Stay;
        }
        public int MakeBet()
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
