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
            game.AddPlayer(new HumanPlayer("johan"));

            InitialMoney(100,250);

            while (!isNewGame)
            {
                PrintBalances();
                NewRound();
                PrintBets();

                game.FirstDeal();

                PrintHands();
                TurnRotation();
                PrintHands();//TODO remove 

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

        private void PrintBalances()
        {
            Console.BackgroundColor = ConsoleColor.DarkBlue;
            foreach(IPlayer player in game.players)
            {
                var PBalance= Bank.GetPlayerMoney(player.Id);
                Console.Write($"{player.Name}: {PBalance}$ ");
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
        private void PrintBets()
        {
            foreach (IPlayer player in game.players)
            {
                Console.WriteLine(game.PlaceBet(player));
                System.Threading.Thread.Sleep(200);
            }
            Console.WriteLine(game.PlaceBet(game.dealer));
            Console.WriteLine("------------------------------");
        }

        private void PrintHands()
        {
            //TODO create enum and later print special card combos in prints after playerhand
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            foreach(IPlayer player in game.players)
            {
                var playerBalance = GetBalance(player);
                var playerHand = game.GetHand(player);
                Console.Write($"({playerBalance}$) {player.Name}: {playerHand}");
                Console.WriteLine();
                System.Threading.Thread.Sleep(200);
            }
            Console.ForegroundColor = ConsoleColor.DarkMagenta;
            var dealerBalance = GetBalance(game.dealer);
            var dealerHand = game.GetHand(game.dealer);
            Console.Write($"({dealerBalance}$) {game.dealer.Name}: {dealerHand}");
            Console.WriteLine();
            Console.WriteLine("------------------------------");
            Console.ResetColor();
        }
        private void TurnRotation()
        {
            foreach(IPlayer player in game.players)
            {
                game.PlayerTurn(player);
            }
            game.PlayerTurn(game.dealer);
        }

        private void PrintWinners()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("------------------------------");
            foreach (IPlayer player in game.players)
            {
                Console.WriteLine(game.EvaluateWinner(player, game.dealer));
            }
            Console.WriteLine("------------------------------");
            Console.ResetColor();
        }

        private string GetBet(IPlayer player)
        {
            return Bank.GetPlayerBet(player.Id).ToString();
        }
        private string GetBalance(IPlayer player)
        {
            return Bank.GetPlayerMoney(player.Id).ToString();
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
