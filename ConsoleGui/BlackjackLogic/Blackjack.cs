using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleGui
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
        public AiDealer dealer = new AiDealer("Dealer");
        public List<RegularPlayer> players = new List<RegularPlayer>();

        Deck deck;

       
        public void AddPlayer(RegularPlayer player)
        {
            players.Add(player);
        }
        public void RemovePlayer(RegularPlayer player)
        {
            //AiDealer not removable with this method
            players.Remove(player);    
        }
        public void NewGame()
        {
            List<Player> ToBeCleared = new List<Player>();

            foreach (Player player in players)
            {
                ToBeCleared.Add(player);
            }
            ToBeCleared.Add(dealer);

            for(int i=0;i<ToBeCleared.Count;i++)
            {
                ToBeCleared.Remove(ToBeCleared[i]);
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
            player._Hand.AddCard(deck.ReturnCard());
        }
        public PlayerDecision ReturnDecision(Player player)
        {
            return player.MakeDecision();
        }
        public void AddMoney(Player player,int amount)
        {
            player.Balance += amount;
        }
        public void RemoveMoney(Player player, int amount)
        {
            player.Balance -= amount;
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

        private void ClearHand(Player player)
        {
            player._Hand.Clear();
        }
        public void ClearHands()
        {
            foreach (RegularPlayer player in players)
            {
                player._Hand.Clear();
            }
            dealer._Hand.Clear();
        }
        private void ClearBet(RegularPlayer player)
        {
            player.Bet = 0;
        }
        public void ClearBets()
        {
            foreach(RegularPlayer player in players)
            {
                ClearBet(player);
            }
        }
        public string GetHand(Player player)
        {
            //TODO decide where to have this method (in Hand?)
            string pHand = string.Empty;
            foreach (Card c in player._Hand.Cards)
            {
                pHand += c.ToString() + " ";
            }
            //also adds the players current total score next to handvalue
            pHand += GetHandValue(player);
            return pHand;
        }
        private string GetHandValue(Player player)
        {
            return string.Format($"({Rules.GethandValue(player._Hand)})");
        }
        private bool isBankrupt(Player player)
        {
            if (player.Balance==0)
                return true;
            else
                return false;
        }
        public List<Player> ReturnBankrupt()
        {
            //TODO decide if this method is needed in here? maybe GUI class?
            List<Player> bankruptPlayers = new List<Player>();

            foreach (RegularPlayer player in players)
            {
                if (isBankrupt(player))
                    bankruptPlayers.Add(player);
            }

            if (isBankrupt(dealer))
                bankruptPlayers.Add(dealer);

            return bankruptPlayers;
        }     
        public Winninghand ReturnWinner(RegularPlayer player, AiDealer dealer)
        {
            //returns winner: a regular player OR dealer:
            Winninghand winner =  Rules.EvaluateWinner(player._Hand, dealer._Hand);
            return winner;
        }
        public bool isBankrupt2()
        {
            if (dealer.Balance <= 0)
                return true;
            else
                return false;
        }
    }
}
