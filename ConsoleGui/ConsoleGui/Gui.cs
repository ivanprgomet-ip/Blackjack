using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleGui
{
    class Gui
    {
        private Blackjack game = new Blackjack();

        private bool isNewGame = false;//gets asked for new game when dealer runs out of money

        public void Run()
        {
            game.AddPlayer(new HumanPlayer("ivan"));
            game.AddPlayer(new AiPlayer("james"));
            game.AddPlayer(new HumanPlayer("dalius"));

            InitialMoney(5,250);

            while (!isNewGame)
            {
                NewRound();
                PrintBalances();

                ValidateBets();
                PrintBets();

                Console.WriteLine(">> press any button to deal cards <<");
                Console.ReadKey();
                Console.Clear();

                PrintBalances();
                game.FirstDeal();
                PrintHands();
                
                PlayerOutcomes();
                PrintHands();

                PrintWinners();
                Console.ReadKey();
                Console.Clear();
            }
        }
        private void InitialMoney(int playersMoney,int dealerMoney)
        {
            foreach (IPlayer player in game.players)
            {
                game.AddMoney(player, playersMoney);
            }
            game.AddMoney(game.dealer, dealerMoney);
        }
        private void RemoveBankruptPlayers()
        {
            //game.returnbankrupt() returns a list (of bankrupt players)
            //by checking each and every players current balance
            List<IPlayer> bankruptPlayers = game.ReturnBankrupt();

            foreach (IPlayer player in bankruptPlayers)
            {
                game.players.Remove(player);
                Console.WriteLine($"{player.Name} left");
            }
        }
        private void NewRound()
        {
            RemoveBankruptPlayers();//remove bankrupt players before every round

            game.ClearBets();
            foreach(IPlayer player in game.players)
            {
                player.Hand.Clear();
            }
            game.dealer.Hand.Clear();
            game.ResetDeck();
        }
        private void NewGame()
        {
            foreach(IPlayer player in game.players)
            {
                game.RemovePlayer(player);
            }
            game.RemovePlayer(game.dealer);
        }
        ////////////////////////////////////////////////
        private string ValidateBet(IPlayer player)
        {
            var betIsValid = game.ValidateBet(player);
            var pName = player.Name;

            while (!betIsValid)
            {
                Console.BackgroundColor = ConsoleColor.DarkRed;
                Console.WriteLine(($"{pName}: Balance lower then bet amount!"));
                Console.ResetColor();
                betIsValid = game.ValidateBet(player);

                if (betIsValid)
                    break;
            }
            return ($"{pName} bet {Bank.GetPlayerBet(player.Id)}$");
        }
        private void ValidateBets()
        {
            foreach (IPlayer player in game.players)
            {
                ValidateBet(player);
            }
            ValidateBet(game.dealer);
            Console.WriteLine();
        }

        private void PrintBet(IPlayer player)
        {
            var pBet = Bank.GetPlayerBet(player.Id);
            var pName = player.Name;
            Console.WriteLine($"{pName} bet {pBet}$");
        }
        private void PrintBets()
        {
            foreach (IPlayer player in game.players)
            {
                PrintBet(player);
            }
            PrintBet(game.dealer);
            Console.WriteLine();
        }
        ////////////////////////////////////////////////
        private string RtrnIfBust(IPlayer player)
        {
            return Rules.isBust(player.Hand) ? "BUST" : "";
        }
        private string RtrnHand(IPlayer player)
        {
            return string.Format(($"{player.Name}: {game.GetHand(player)}"));
        }
        private void PrintHands()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            foreach(IPlayer player in game.players)
            {
                Console.Write(RtrnHand(player)+" "+RtrnIfBust(player));
                Console.WriteLine();
            }
            Console.Write(RtrnHand(game.dealer) + " " + RtrnIfBust(game.dealer));
            Console.WriteLine();
            Console.ResetColor();
        }
        ////////////////////////////////////////////////
        private void PrintBalance(IPlayer player)
        {
            var PBalance = Bank.GetPlayerMoney(player.Id);
            Console.Write($"{player.Name}: {PBalance}$ | ");
        }
        private void PrintBalances()
        {
            Console.BackgroundColor = ConsoleColor.DarkGreen;
            foreach(IPlayer player in game.players)
            {
                PrintBalance(player);
            }
            PrintBalance(game.dealer);
            Console.WriteLine();
            Console.ResetColor();
        }
        ////////////////////////////////////////////////
        private void PlayerOutcomes()
        {
            foreach(IPlayer player in game.players)
            {
                game.DecisionOutcome(player);
            }
            game.DecisionOutcome(game.dealer);
        }

        private void PrintWinners()//TODO TEST METHOD
        {
            foreach(IPlayer player in game.players)
            {
                Winninghand currentWinner = game.ReturnWinner(player, game.dealer);

                if (currentWinner == Winninghand.Dealer)
                    Console.WriteLine($"{game.dealer.Name} won {Bank.GetPlayerBet(player.Id)}$ over {player.Name}");
                if (currentWinner == Winninghand.Player)
                    Console.WriteLine($"{game.dealer.Name} lost {Bank.GetPlayerBet(player.Id)}$ against {player.Name}");
                if (currentWinner == Winninghand.Draw)
                    Console.WriteLine($"Draw between {game.dealer.Name} and {player.Name}");
            }
        }
    }
}
