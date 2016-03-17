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
                PlayerTurn();
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
            }
            Console.WriteLine(game.PlaceBet(game.dealer));
            Console.WriteLine("------------------------------");
        }

        private void PrintHand(IPlayer player)
        {
            Console.Write($"{player.Name}: {game.GetHand(player)}");
        }
        private void PrintHands()
        {
            //TODO create enum and later print special card combos in prints after playerhand
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            foreach(IPlayer player in game.players)
            {
                var playerBalance = GetBalance(player);
                var playerHand = game.GetHand(player);
                Console.Write($"{player.Name}: {playerHand}");
                Console.WriteLine();
            }
            Console.ForegroundColor = ConsoleColor.DarkMagenta;
            var dealerBalance = GetBalance(game.dealer);
            var dealerHand = game.GetHand(game.dealer);
            Console.Write($"{game.dealer.Name}: {dealerHand}");
            Console.WriteLine();
            Console.WriteLine("------------------------------");
            Console.ResetColor();
        }
        private void PlayerTurn()
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
