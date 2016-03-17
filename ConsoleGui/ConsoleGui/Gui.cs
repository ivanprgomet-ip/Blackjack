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

            InitialMoney(100,250);

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

        private string GetBalance(IPlayer player)
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

        ////////////////////////////////////////////////
        private string RtrnBet(IPlayer player)
        {
            return string.Format(game.PlaceBet(player));
        }
        private void PrintBets()
        {
            foreach (IPlayer player in game.players)
            {
                Console.WriteLine(RtrnBet(player));
            }
            Console.WriteLine(RtrnBet(game.dealer));
        }
        ////////////////////////////////////////////////
        private string RtrnHand(IPlayer player)
        {
            return string.Format(($"{player.Name}: {game.GetHand(player)}"));
        }
        private void PrintHands()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            foreach(IPlayer player in game.players)
            {
                Console.Write(RtrnHand(player));
                Console.WriteLine();
            }
            Console.Write(RtrnHand(game.dealer));
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


        private void PrintWinners()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            foreach (IPlayer player in game.players)
            {
                Console.WriteLine(game.EvaluateWinner(player, game.dealer));
            }
            Console.ResetColor();
        }

        private void NewGame()
        {
            foreach(IPlayer player in game.players)
            {
                game.RemovePlayer(player);
            }
            game.RemovePlayer(game.dealer);
        }
    }
}
