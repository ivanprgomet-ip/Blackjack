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
            game.AddPlayer(new AiPlayer("dalius"));
            game.AddPlayer(new AiPlayer("james"));

            InitialMoney(5,250);

            while (!isNewGame)
            {
                NewRound();
                PrintBalances();
                PrintBets();

                Console.WriteLine("press any button to deal cards");
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

        private void NewRound()
        {
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
        private void PrintBetMsg(IPlayer player)
        {
            var betIsValid = game.ValidBet(player);
            var pName = player.Name;

            while (!betIsValid)
            {
                Console.WriteLine(($"{pName}: BALANCE<BET"));
                betIsValid = game.ValidBet(player);

                if (betIsValid)
                    break;
            }
            Console.WriteLine($"{pName} bet {Bank.GetPlayerBet(player.Id)}");
        }
        private void PrintBets()
        {
            foreach (IPlayer player in game.players)
            {
                PrintBetMsg(player);
            }
            PrintBetMsg(game.dealer);
        }
        ////////////////////////////////////////////////
        private string RtrnBust(IPlayer player)
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
                Console.Write(RtrnHand(player)+" "+RtrnBust(player));
                Console.WriteLine();
            }
            Console.Write(RtrnHand(game.dealer) + " " + RtrnBust(game.dealer));
            Console.WriteLine();
            Console.ResetColor();
        }
        ////////////////////////////////////////////////
        private string RtrnBalance(IPlayer player)
        {
            return Bank.GetPlayerMoney(player.Id).ToString();
        }
        private void PrintBalances()
        {
            Console.BackgroundColor = ConsoleColor.DarkBlue;
            foreach(IPlayer player in game.players)
            {
                var PBalance= Bank.GetPlayerMoney(player.Id);
                Console.Write($"{player.Name}: {PBalance}$ | ");
            }
            var DBalance = Bank.GetPlayerMoney(game.dealer.Id);
            Console.Write($"{game.dealer.Name}: {DBalance}$ ");
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

        //private void PrintWinners()
        //{
        //    Console.ForegroundColor = ConsoleColor.Yellow;
        //    foreach (IPlayer player in game.players)
        //    {
        //        Console.WriteLine(game.EvaluateWinner(player, game.dealer));
        //    }
        //    Console.ResetColor();
        //}
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
