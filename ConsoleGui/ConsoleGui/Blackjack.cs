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
                    DealCardTo(player, deck);
                }
                DealCardTo(dealer, deck);
            }
        }
        private void DealCardTo(IPlayer player, Deck deck)
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
                        DealCardTo(player, deck);
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
        public string PlaceBet(IPlayer player)
        {
            int bet = player.MakeBet();//returns int betwen 1-10 (validation control keeps human players in check | logic keeps computer players in check)
            BetValidity validity = bank.CheckBet(player, bet);//return valid if bet is lower than or equal to current balance
            
            switch(validity)
            {
                case BetValidity.BalanceTooLow:
                    //TODO prompt player to do bet again
                    return string.Format($"{player.Name}: bet higher than current balance");
                case BetValidity.BetValid:
                    bank.AddBet(player.Id, bet);// At this point bet is valid and has ben added to bank and amount removed from balance
                    return string.Format($"{player.Name}: bet {bet}$");
                default:
                    return string.Format($"{player.Name}: something went wrong");
            }            
        }
        public void ClearBets()
        {
            bank.ClearBets();
        }
        public string EvaluateWinner(IPlayer player, AiDealer dealer)
        {
            //TODO transform into switch statements instead
            Winninghand winner = Rules.EvaluateWinner(dealer.Hand, player.Hand);
            var dealerBet = Bank.GetPlayerBet(dealer.Id);
            var PlayerBet = Bank.GetPlayerBet(player.Id);

            switch(winner)
            {
                case Winninghand.Dealer:
                    AddMoney(dealer, PlayerBet);//adds the players bet amount to the dealer balance (who won)
                    return string.Format($"{dealer.Name} won {PlayerBet}$ against {player.Name}");
                case Winninghand.Player:
                    AddMoney(player, dealerBet);//adds the dealers bet amount to the player balance (who won)
                    return string.Format($"{dealer.Name} lost {dealerBet}$ against {player.Name}");
                case Winninghand.Draw:
                    AddMoney(player, PlayerBet);//money back to player
                    AddMoney(dealer, dealerBet);//money back to dealer
                    return string.Format($"Draw between {dealer.Name} and {player.Name}");
            }
            return string.Format("something went horribly wrong in the switch");


            //if (winner == Winninghand.Dealer)
            //{
            //    AddMoney(dealer, PlayerBet);//adds the players bet amount to the dealer balance (who won)
            //    return string.Format($"{dealer.Name} won {PlayerBet}$ against {player.Name}");
            //}
            //else if (winner == Winninghand.Player)
            //{
            //    AddMoney(player, dealerBet);//adds the dealers bet amount to the player balance (who won)
            //    return string.Format($"{dealer.Name} lost {dealerBet}$ against {player.Name}");
            //}
            //else /*(winner == Winninghand.Draw)*/
            //{
            //    AddMoney(player, PlayerBet);//money back to player
            //    AddMoney(dealer, dealerBet);//money back to dealer
            //    return string.Format($"Draw between {dealer.Name} and {player.Name}");
            //}
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
    }
}
