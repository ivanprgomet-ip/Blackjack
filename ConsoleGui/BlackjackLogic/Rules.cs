using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CardLogic;

namespace BlackjackLogic
{
    public enum Winninghand
    {
        Dealer,
        Player,
        Draw
    }

    public class Rules
    {
        public static bool isBust(Hand pHand)
        {
            return GethandValue(pHand) > 21 ? true : false;
        }
        public static Winninghand EvaluateWinner(Hand player, Hand dealer)
        {
            int dHandValue = GethandValue(dealer);
            int pHandValue = GethandValue(player);

            if (pHandValue>21)
                return dHandValue > 21 ? Winninghand.Draw : Winninghand.Dealer;
            else if (dHandValue > 21)
                return pHandValue > 21 ? Winninghand.Draw : Winninghand.Player;
            else
            {
                if (dHandValue == pHandValue)
                    return Winninghand.Draw;
                else
                    return pHandValue > dHandValue ? Winninghand.Player : Winninghand.Dealer;
            }
        }
        public static int GethandValue(Hand hand)
        {
            var sum = 0;
            foreach (var card in hand.Cards)
            {
                if (card.Value >= 2 && card.Value <= 10)
                    sum += card.Value;
                // Faces (J, Q, K) have value 10
                else if (card.Value > 10)
                    sum += 10;

                // Decide if an Ace is worth 1 or 11
                if (card.Value == 1)
                {
                    // This gives Black Jack (sum = 21) 
                    // and counts towards highest possible value
                    if (sum <= 10)
                        sum += 11;
                    else
                        sum += 1;
                }
            }
            return sum;
        }
    }
}
