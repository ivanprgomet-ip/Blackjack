using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleGui
{
    public enum PlayerDecision
    {
        Hit,
        Stay
    }

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

        ////////////////////////////////////////////////////////////////////
        public void AddPlayer(IPlayer player)
        {
            players.Add(player);
        }
        public void RemovePlayer(IPlayer player)
        {
            bank.RemoveBetAndBalance(player.Id);
            players.Remove(player);    
        }
        ////////////////////////////////////////////////////////////////////
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
        private void DealCardTo(IPlayer player)
        {
            player.Hand.AddCardToHand(deck.ReturnCard());
        }
        ////////////////////////////////////////////////////////////////////
        public PlayerDecision ReturnDecision(IPlayer player)
        {
            return player.MakeDecision(player.Hand);
        }
        public void DecisionOutcome(IPlayer player)
        {
            bool bustOrStay = false;
            while(!bustOrStay)
            {
                if (Rules.GethandValue(player.Hand) > 21)
                    bustOrStay = true;
                else
                {
                    PlayerDecision pDecision = ReturnDecision(player);
                    if (pDecision == PlayerDecision.Hit)
                        DealCardTo(player);
                    else
                        bustOrStay = true;
                }
            }
            // TODO check here if player is bust, and if so, mark it somehow
        }
        ////////////////////////////////////////////////////////////////////
        public void AddMoney(IPlayer player,int playerMoney)
        {
            bank.AddMoneyToPlayer(player.Id, playerMoney);
        }
        public void ResetDeck()
        {
            deck = new Deck();
            deck.Shuffle();
        }//do this in the deck class?
        ////////////////////////////////////////////////////////////////////
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
        ////////////////////////////////////////////////////////////////////
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
        ////////////////////////////////////////////////////////////////////
        
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
                    //RemovePlayer(player);
                }
            }
            return bankruptPlayers;
        }
        //public void RemoveBankrupt()
        //{
        //    List<IPlayer> bankruptPlayers = ReturnBankrupt();//returns a list of bankrupt players by checking each and every players balance
            
        //    foreach(IPlayer player in bankruptPlayers)
        //    {
        //        players.Remove(player);
        //    }
        //}

        ////////////////////////////////////////////////////////////////////

        //public string EvaluateWinner(IPlayer player, AiDealer dealer)
        //{
        //    //TODO transform into switch statements instead
        //    Winninghand winner = Rules.EvaluateWinner(dealer.Hand, player.Hand);
        //    var dealerBet = Bank.GetPlayerBet(dealer.Id);
        //    var PlayerBet = Bank.GetPlayerBet(player.Id);

        //    switch(winner)
        //    {
        //        case Winninghand.Dealer:
        //            AddMoney(dealer, PlayerBet);//adds the players bet amount to the dealer balance (who won)
        //            return string.Format($"{dealer.Name} won {PlayerBet}$ against {player.Name}");
        //        case Winninghand.Player:
        //            AddMoney(player, dealerBet);//adds the dealers bet amount to the player balance (who won)
        //            return string.Format($"{dealer.Name} lost {dealerBet}$ against {player.Name}");
        //        case Winninghand.Draw:
        //            AddMoney(player, PlayerBet);//money back to player
        //            AddMoney(dealer, dealerBet);//money back to dealer
        //            return string.Format($"Draw between {dealer.Name} and {player.Name}");
        //    }
        //    return string.Format("error");
        //}
        public Winninghand ReturnWinner(IPlayer player, AiDealer dealer)
        {
            Winninghand winner = Rules.EvaluateWinner(player.Hand, dealer.Hand);
            return winner;
        }
    }
}
