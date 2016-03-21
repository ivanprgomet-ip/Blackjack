using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlackjackLogic;

namespace ConsoleGui
{
    class Gui
    {
        private Blackjack game = new Blackjack();
        //TODO fix dealer bankrupcy so when i restart game restarts
        //TODO fix regular player bankrupcy so when someone goes broke, he leaves
        private bool pActive = true;//true if someone is still active
        private bool GameIsOver;//true if dealer is bankrupt = game is over
        private bool newGame = true;

        public void Run()
        {
            while (newGame)
            {
                game.NewGame();
                GameIsOver = false;

                ////////
                game.AddPlayer(new HumanPlayer("ivan"));
                game.AddPlayer(new AiPlayer("james"));
                game.AddPlayer(new AiPlayer("dalius"));
                InitialMoney(5, 100);
                ////////

                while (!GameIsOver)
                {
                    NewRound();
                    PrintBalances();

                    MakeBets();
                    PrintBets();

                    Console.WriteLine(">> press any button to deal cards <<");
                    Console.ReadKey();
                    Console.Clear();

                    PrintBalances();
                    game.FirstDeal();
                    PrintHands();

                    while (pActive)
                    {
                        pActive = PlayersHitOrStay();
                        if (!pActive)
                            break;
                    }

                    Console.WriteLine(">> press any button for results <<");
                    Console.ReadKey();
                    UpdateWinners();

                    GameIsOver = game.isBankrupt2();
                    if (GameIsOver)
                        break;

                    Console.WriteLine(">> press any button for next round <<");
                    Console.ReadKey();
                    Console.Clear();
                }
                StartNewGame();
                Console.Clear();
            }
        }
        public void PlayerSetup()
        {
            //todo
        }
        private void StartNewGame()
        {
            Console.WriteLine(">>> GAME OVER <<<");
            Console.WriteLine("The house went bankrupt");
            Console.WriteLine("Start New Game? y/n");
            string decision = Console.ReadLine().ToLower();

            if (decision.ToLower() == "y")
                newGame = true;
            else
                newGame = false;
        }

        private void InitialMoney(int playersMoney,int dealerMoney)
        {
            foreach (RegularPlayer player in game.players)
            {
                game.AddMoney(player, playersMoney);
            }
            game.AddMoney(game.dealer, dealerMoney);
        }
        private void RemoveBankruptPlayers()
        {
            List<Player> bankruptPlayers = game.ReturnBankrupt();
            
            foreach (Player player in bankruptPlayers)
            {
                bankruptPlayers.Remove(player);
                Console.WriteLine($"{player.Name} left");
                Console.ReadKey();
                Console.Clear();
            }
        }
        private void NewRound()
        {
            pActive = true;//resetting general player active variable
            RemoveBankruptPlayers();//extract and remove bankrupt players before every round

            game.ClearBets();
            game.ClearHands();
            
            game.InitializeDeck();//52cards refreshed and shuffled
        }

        private string ValidateBet(RegularPlayer player)
        {
            var betIsValid = game.BetIsValid(player);
            var pName = player.Name;

            while (!betIsValid)
            {
                Console.BackgroundColor = ConsoleColor.DarkRed;
                Console.WriteLine(($"{pName}: Balance lower then bet amount!"));
                Console.ResetColor();
                betIsValid = game.BetIsValid(player);//player re-enters bet

                if (betIsValid)
                    break;
            }
            return ($"{pName} bet {player.Bet}$");
        }
        private void MakeBets()
        {
            foreach (RegularPlayer player in game.players)
            {
                ValidateBet(player);
            }
            Console.WriteLine();
        }
        private void PrintBet(RegularPlayer player)
        {
            Console.WriteLine($"{player.Name} bet {player.Bet}$");
        }
        private void PrintBets()
        {
            foreach (RegularPlayer player in game.players)
            {
                PrintBet(player);
            }
            Console.WriteLine();
        }

        private string RtrnIfBust(Player player)
        {
            return Rules.isBust(player._Hand) ? "BUST" : "";
        }
        private string RtrnHand(Player player)
        {
            return string.Format(($"{player.Name}: {game.GetHand(player)}"));
        }
        private void PrintHands()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            foreach(RegularPlayer player in game.players)
            {
                Console.Write(RtrnHand(player)+" "+RtrnIfBust(player));
                Console.WriteLine();
            }
            Console.Write(RtrnHand(game.dealer) + " " + RtrnIfBust(game.dealer));
            Console.WriteLine();
            Console.ResetColor();
        }
        private void PrintBalance(Player player)
        {
            //var regPlayer2 = player as RegularPlayer;

            if(player is RegularPlayer)
            {
                var regPlayer = (RegularPlayer)player;
                Console.Write($"{regPlayer.Name}: {regPlayer.Balance}$ ({regPlayer.Bet}$) | ");

            }
            else
                Console.Write($"{player.Name}: {player.Balance}$ | ");
        }
        private void PrintBalances()
        { 
            Console.BackgroundColor = ConsoleColor.DarkGreen;
            foreach (RegularPlayer player in game.players)
            {
                PrintBalance(player);
            }
            PrintBalance(game.dealer);
            Console.WriteLine();
            Console.ResetColor();
        }
        private void HitOrStay(Player player)
        {
            bool bustOrStay = false;
            while (!bustOrStay)
            {
                if (Rules.GethandValue(player._Hand) > 21)
                    bustOrStay = true;
                else
                {
                    PlayerDecision pDecision = game.ReturnDecision(player);
                    if (pDecision == PlayerDecision.Hit)
                    {
                        game.DealCardTo(player);
                        Console.Clear();
                        PrintBalances();
                        PrintHands();
                    }
                    else
                        bustOrStay = true;
                }
            }
        }
        public bool PlayersHitOrStay()
        {
            foreach(RegularPlayer player in game.players)
            {
                HitOrStay(player);
            }
            HitOrStay(game.dealer);
            return false;//returns false to say there is no more active players
        }
        private void UpdateWinners()
        {
            //all regular players evaluated against dealer
            foreach(RegularPlayer player in game.players)
            {
                int pBet = player.Bet;
                string pName = player.Name;
                string dName = game.dealer.Name;
                Winninghand currentWinner = game.ReturnWinner(player, game.dealer);
                Console.ForegroundColor = ConsoleColor.Yellow;

                if (currentWinner == Winninghand.Dealer)
                {
                    game.AddMoney(game.dealer, pBet);//add bet $ to dealer winner
                    game.RemoveMoney(player, pBet);//remove lost $ from loser
                    Console.WriteLine($"{dName} +{pBet}$ | {pName} -{pBet}$");
                }
                if (currentWinner == Winninghand.Player)
                {
                    game.AddMoney(player, pBet);//add bet $ to player winner
                    game.RemoveMoney(game.dealer, pBet);//remove lost $ from loser
                    Console.WriteLine($"{dName} -{pBet}$ | {pName} +{pBet}$");
                }
                if (currentWinner == Winninghand.Draw)
                {
                    //game.AddMoney(player, pBet);//player reclaim bet $
                    Console.WriteLine($"Draw between {dName} and {pName} (No $ lost for player)");
                }
                Console.ResetColor();
            }
        }
    }
}
