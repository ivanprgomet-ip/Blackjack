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
        public AiDealer dealer = new AiDealer();
        public List<IPlayer> players = new List<IPlayer>();

        Bank bank = new Bank();
        Deck deck;

        public Blackjack()
        {

        }

        public void AddPlayer(IPlayer player)
        {
            players.Add(player);
        }
        public void RemovePlayer(IPlayer player)
        {
            bank.RemoveBetAndBalance(player.Id);
            players.Remove(player);    
        }
        public void FirstDeal()
        {
            for (int i = 0; i < 2; i++)
            {
                foreach (IPlayer player in players)
                {
                    DealCardTo(player);
                }
                DealCardTo(dealer);
            }
        }
        public void DealCardTo(IPlayer player)
        {
            player.Hand.AddCardToHand(deck.ReturnCard());
        }
        public PlayerDecision ReturnDecision(IPlayer player)
        {
            return player.MakeDecision(player.Hand);
        }
        public void AddMoney(IPlayer player,int playerMoney)
        {
            bank.AddMoneyToPlayer(player.Id, playerMoney);
        }
        public void RemoveMoney(IPlayer player, int playerMoney)
        {
            bank.RemoveMoneyFromPlayer(player.Id, playerMoney);
        }

        public void ResetDeck()
        {
            deck = new Deck();
            deck.Shuffle();
        }
        public bool ValidateBet(IPlayer player)
        {
            int betToTry = player.MakeBet();//returns int betwen 1-10 (validation control keeps human players in check | logic keeps computer players in check)
            bool betIsValid = bank.ValidateBet(player, betToTry);//validates by checking balance

            if (betIsValid)
            {
                bank.AddBet(player.Id, betToTry);//bet went through to the bank
                return true;
            }
            else
                return false;//bet failed and never got to the bank
        }

        public void ClearBets()
        {
            bank.ClearBets();
        }
        public string GetHand(IPlayer player)
        {
            string playerHand = string.Empty;
            foreach (Card c in player.Hand.Cards)
            {
                playerHand += c.ToString() + " ";
            }
            //also adds the players current total score next to handvalue
            playerHand += GetHandValue(player);
            return playerHand;
        }
        private string GetHandValue(IPlayer player)
        {
            return string.Format($"({Rules.GethandValue(player.Hand)})");
        }
        private bool isBankrupt(IPlayer player)
        {
            //checks if player balance is 0
            if (Bank.GetPlayerMoney(player.Id) == 0)
                return true;
            else
                return false;
        }
        public List<IPlayer> ReturnBankrupt()
        {
            List<IPlayer> bankruptPlayers = new List<IPlayer>();

            foreach (IPlayer player in players)
            {
                if (isBankrupt(player))
                {
                    bankruptPlayers.Add(player);
                }
            }
            if (isBankrupt(dealer))
                bankruptPlayers.Add(dealer);

            return bankruptPlayers;
        }     
        public Winninghand ReturnWinner(IPlayer player, AiDealer dealer)
        {
            //returns winner: a player OR the dealer:
            Winninghand winner = Rules.EvaluateWinner(player.Hand, dealer.Hand);
            return winner;
        }
        public bool CheckGameOver()
        {
            var DealerCash = Bank.GetPlayerMoney(dealer.Id);
            if (DealerCash <= 0)
                return true;
            else
                return false;
        }
        public void NewGame()
        {
            List<IPlayer> ToBeRemoved = new List<IPlayer>();

            foreach (IPlayer player in players)
            {
                ToBeRemoved.Add(player);
            }
            ToBeRemoved.Add(dealer);

            foreach(IPlayer player in ToBeRemoved)
            {
                RemovePlayer(player);
            }

        }
    }
}
