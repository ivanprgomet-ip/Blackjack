using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CardLogic;

namespace BlackjackLogic
{


    /// <summary>
    /// external API to handle a game of blackjack.
    /// the main terminal core of the blackjack program.
    /// whenever a button is pressed on the gui type, it goes
    /// first through this class, then out to the rest of the
    /// classes and methods. 
    /// </summary>
    public class Blackjack
    {
        public AiDealer dealer = new AiDealer();
        public List<RegularPlayer> players = new List<RegularPlayer>();

        Deck deck;

       
        public void AddPlayer(RegularPlayer player)
        {
            players.Add(player);
        }

        public void NewGame()
        {
            /*resets the dealer balance to 0$, later dealer 
            gets + the initial money*/
            ResetDealerBalance();

            /*Removes all players in list, and later adds 
            the user specified players that will be in the game*/
            List<Player> ToBeCleared = new List<Player>();
            foreach (Player player in players)
            {
                ToBeCleared.Add(player);
            }
            foreach (RegularPlayer player in ToBeCleared)
            {
                players.Remove(player);
            }
        }
        public void FirstDeal()
        {
            for (int i = 0; i < 2; i++)
            {
                foreach (Player player in players)
                {
                    DealCardTo(player);
                }
                DealCardTo(dealer);
            }
        }
        public void DealCardTo(Player player)
        {
            player.Hand.AddCard(deck.ReturnCard());
        }
        public PlayerDecision ReturnDecision(Player player)
        {
            return player.MakeDecision();
        }
        public void AddMoney(Player player,int amount)
        {
            player.Balance += amount;
        }
        public void ResetDealerBalance()
        {
            dealer.Balance = 0;
        }
        public void InitializeDeck()
        {
            deck = new Deck();
            deck.Shuffle();
        }
        private bool ValidateBet(RegularPlayer player, int bet)
        {
            if (player.Balance < bet)
                return false;
            else
                return true;
        }
        public bool BetIsValid(RegularPlayer player)
        {
            //TODO rules class should be the validator of bets
            int betToTry = player.MakeBet();
            bool betIsValid = ValidateBet(player, betToTry);

            if (betIsValid)
            {
                player.Bet = betToTry;
                return true;
            }
            else
                return false;
        }
        private bool isBankrupt(Player player)
        {
            if (player.Balance==0)
                return true;
            else
                return false;
        }
        public bool DealerIsBankrupt()
        {
            if (dealer.Balance <= 0)
                return true;
            else
                return false;
        }
        public List<RegularPlayer> ReturnBankrupt()
        {
            //TODO decide if this method is needed in here? maybe GUI class?
            List<RegularPlayer> bankruptPlayers = new List<RegularPlayer>();

            foreach (RegularPlayer player in players)
            {
                if (isBankrupt(player))
                    bankruptPlayers.Add(player);
            }
            return bankruptPlayers;
        }     
        public Winninghand ReturnWinner(RegularPlayer player)
        {
            //returns winner: a regular player OR dealer:
            Winninghand winner =  Rules.EvaluateWinner(player.Hand, dealer.Hand);
            return winner;
        }

    }
}
