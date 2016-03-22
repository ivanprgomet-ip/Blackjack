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
        private bool GameIsOver;//if dealer is bankrupt, game is over
        private bool newGame = true;

        /////////////////////////////////
        public void Run()
        {
            while (newGame)
            {
                game.NewGame();
                GameIsOver = false;

                PlayerSetup();
                Console.Clear();

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

                    GameIsOver = game.DealerIsBankrupt();
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
        /////////////////////////////////
        public void PlayerSetup()
        {
            Console.WriteLine("1/ Just Deal");
            Console.WriteLine("2/ Setup");
            string choice = Console.ReadLine();

            switch(choice)
            {
                case "1":
                    game.AddPlayer(new HumanPlayer("Ivan"));
                    game.AddPlayer(new AiPlayer("Terminator"));
                    game.AddPlayer(new AiPlayer("Rocky"));
                    InitialMoney(100, 100);
                    PrintJoinedPlayers();
                    Console.ReadKey();
                    break;
                case "2":
                    bool active = true;
                    game.AddMoney(game.dealer, 100);//if dealer balance is not set, its 100$ by default

                    while(active)
                    {
                        Console.WriteLine("1/ Add AiPlayer");
                        Console.WriteLine("2/ Add HumanPlayer");
                        Console.WriteLine("3/ Set Dealer startbalance");
                        Console.WriteLine("4/ Start Game");
                        string setupChoice = Console.ReadLine();

                        switch (setupChoice)
                        {
                            case "1":
                                Console.WriteLine("Enter name for player >> ");
                                string aiName = Console.ReadLine();
                                AiPlayer myNewAi = new AiPlayer(aiName);
                                Console.WriteLine($"Enter start balance for {aiName}: >> ");
                                int startBalance = int.Parse(Console.ReadLine());
                                game.AddMoney(myNewAi, startBalance);
                                game.AddPlayer(myNewAi);
                                break;
                            case "2":
                                Console.WriteLine("Enter name for player >> ");
                                string humanName = Console.ReadLine();
                                HumanPlayer myNewHuman = new HumanPlayer(humanName);
                                Console.WriteLine($"Enter start balance for {humanName}: >> ");
                                int startbalance2 = int.Parse(Console.ReadLine());
                                game.AddMoney(myNewHuman, startbalance2);
                                game.AddPlayer(myNewHuman);
                                break;
                            case "3":
                                Console.WriteLine("Enter dealer startbalance >> ");
                                int DealerInitial = int.Parse(Console.ReadLine());
                                Console.ReadKey();
                                break;
                            case "4":
                                active = false;
                                Console.Clear();
                                PrintJoinedPlayers();
                                Console.ReadKey();
                                break;
                            default:
                                Console.WriteLine("not a valid input");
                                break;
                        }
                    }
                    break;
            }
        }
        private void PrintJoinedPlayers()
        {
            foreach(Player player in game.players)
            {
                Console.WriteLine($"{player.Name} joined");
            }
        }

        private void StartNewGame()
        {
            Console.ReadKey();
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
            game.AddMoney(game.dealer,dealerMoney);
            
        }
        private void RemoveBankruptPlayers()
        {
            List<Player> bankruptPlayers = game.ReturnBankruptRegPlayers();

            foreach (RegularPlayer player in bankruptPlayers)
            {
                game.players.Remove(player);
                Console.WriteLine($"{player.Name} left");
                Console.ReadKey();
                Console.Clear();
            }
        }
        private void NewRound()
        {
            pActive = true;//resetting general player active variable
            RemoveBankruptPlayers();//extract and remove bankrupt players before every round

            //clearing bets
            foreach (RegularPlayer player in game.players)
            {
                player.Bet = 0;
            }

            //clearing hands
            foreach (RegularPlayer player in game.players)
            {
                player._Hand.Clear();
            }
            game.dealer._Hand.Clear();

            //initializing and shuffling deck
            game.InitializeDeck();
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

        private string ReturnIfBust(Player player)
        {
            return Rules.isBust(player._Hand) ? "BUST" : "";
        }
        private string ReturnHand(Player player)
        {
            return string.Format(($"{player.Name}: {game.GetHand(player)}"));
        }
        private void PrintHands()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            foreach(RegularPlayer player in game.players)
            {
                Console.Write(ReturnHand(player)+" "+ReturnIfBust(player));
                Console.WriteLine();
            }
            Console.Write(ReturnHand(game.dealer) + " " + ReturnIfBust(game.dealer));
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
                    player.Balance -= pBet;//remove lost $ from loser
                    Console.WriteLine($"{dName} +{pBet}$ | {pName} -{pBet}$");
                }
                if (currentWinner == Winninghand.Player)
                {
                    game.AddMoney(player, pBet);//add bet $ to player winner
                    game.dealer.Balance -= pBet;//remove lost $ from loser
                    Console.WriteLine($"{dName} -{pBet}$ | {pName} +{pBet}$");
                }
                if (currentWinner == Winninghand.Draw)
                {
                    Console.WriteLine($"Draw between {dName} and {pName} (No $ lost for player)");
                }
                Console.ResetColor();
            }
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
        private bool PlayersHitOrStay()
        {
            foreach(RegularPlayer player in game.players)
            {
                HitOrStay(player);
            }
            HitOrStay(game.dealer);
            return false;//returns false to say there is no more active players
        }
    }
}
