using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleGui
{
    public enum Winninghand
    {
        Dealer,
        Player,
        Draw
    }

    public class Rules
    {
        public static Winninghand EvaluateWinner(Hand player, Hand dealer)
        {
            var dealerHandValue = GethandValue(dealer);
            var playerHandValue = GethandValue(player);

            if (playerHandValue > 21)
            {
                if (dealerHandValue > 21)
                    return Winninghand.Draw;
                else
                    return Winninghand.Dealer;
            }
            else if (dealerHandValue > 21)
            {
                if (playerHandValue > 21)
                    return Winninghand.Draw;
                else
                    return Winninghand.Player;
            }
            else
            {
                if (dealerHandValue == playerHandValue)
                {
                    return Winninghand.Draw;
                }
                else
                {
                    return playerHandValue > dealerHandValue ? Winninghand.Player : Winninghand.Dealer;
                }
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
