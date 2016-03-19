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
        private Rules rules = new Rules();
        public AiDealer dealer = new AiDealer("dealer");
        public List<IGambler> players = new List<IGambler>();

        Deck deck;

       
        public void AddPlayer(IGambler player)
        {
            players.Add(player);
        }
        public void RemovePlayer(IGambler player)
        {
            players.Remove(player);    
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
            player.Hand.AddCardToHand(deck.ReturnCard());
        }
        public PlayerDecision ReturnDecision(Player player)
        {
            return player.MakeDecision(player.Hand);
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
        public bool ValidateBet(IGambler player)
        {
            //TODO rules class should be the validator of bets
            int betToTry = player.MakeBet();
            bool betIsValid = bank.ValidateBet(player, betToTry);

            if (betIsValid)
            {
                player.Bet = betToTry;
                return true;
            }
            else
                return false;
        }

        public void ClearBets()
        {
            foreach(IGambler player in players)
            {
                player.Bet = 0;
            }
        }
        public string GetHand(Player player)
        {
            //TODO decide where to have this method (in Hand?)
            string pHand = string.Empty;
            foreach (Card c in player.Hand.Cards)
            {
                pHand += c.ToString() + " ";
            }
            //also adds the players current total score next to handvalue
            pHand += GetHandValue(player);
            return pHand;
        }
        private string GetHandValue(Player player)
        {
            return string.Format($"({rules.GethandValue(player.Hand)})");
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
            /*The List "bankruptPlayers" holds the general Player type, 
            which means it is able to store both regular players and dealers*/
            List<Player> bankruptPlayers = new List<Player>();

            foreach (Player player in players)
            {
                if (isBankrupt(player))
                    bankruptPlayers.Add(player);
            }

            //TODO maybe check dealers balance some other way?
            if (isBankrupt(dealer))
                bankruptPlayers.Add(dealer);

            return bankruptPlayers;
        }     
        public Winninghand ReturnWinner(Player player, AiDealer dealer)
        {
            //returns winner: a player OR dealer:
            Winninghand winner =  rules.EvaluateWinner(player.Hand, dealer.Hand);
            return winner;
        }
        public bool isBankrupt2()
        {
            var DealerCash = dealer.Balance;
            if (DealerCash <= 0)
                return true;
            else
                return false;
        }
        public void NewGame()
        {
            List<Player> ToBeCleared = new List<Player>();

            foreach (Player player in players)
            {
                ToBeCleared.Add(player);
            }
            ToBeCleared.Add(dealer);

            foreach(Player player in ToBeCleared)
            {
                //TODO solve this
                RemovePlayer(player);
            }

        }
    }
}
