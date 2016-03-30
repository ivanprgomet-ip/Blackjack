using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlackjackLogic;
using CardLogic;

namespace ConsoleGui
{
    class Gui
    {
        private Blackjack game = new Blackjack();
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
                //Console.Clear();

                while (!GameIsOver)
                {
                    NewRound();
                    PrintBalances();

                    ProcessBets();
                    PrintBets();

                    Console.WriteLine(">> press any button to deal cards <<");
                    Console.ReadKey();
                    Console.Clear();

                    PrintBalances();
                    game.FirstDeal();
                    PrintHands();

                    while (pActive)
                    {
                        pActive = ProcessDecisions();
                        if (!pActive)
                            break;
                    }

                    Console.WriteLine(">> press any button for results <<");
                    Console.ReadKey();
                    Console.Clear();////////////////
                    PrintBalances();///////////////
                    PrintHandsLast();////////////////
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
                    Console.Clear();
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
                                Console.Write("Enter name for ai player >> ");
                                string aiName = Console.ReadLine();
                                AiPlayer myNewAi = new AiPlayer(aiName);
                                Console.Write($"Enter start balance for {aiName}: >> ");
                                int startBalance = int.Parse(Console.ReadLine());
                                game.AddMoney(myNewAi, startBalance);
                                game.AddPlayer(myNewAi);
                                Console.Clear();
                                break;
                            case "2":
                                Console.Write("Enter name for human player >> ");
                                string humanName = Console.ReadLine();
                                HumanPlayer myNewHuman = new HumanPlayer(humanName);
                                Console.Write($"Enter start balance for {humanName}: >> ");
                                int startbalance2 = int.Parse(Console.ReadLine());
                                game.AddMoney(myNewHuman, startbalance2);
                                game.AddPlayer(myNewHuman);
                                Console.Clear();
                                break;
                            case "3":
                                Console.Write("Enter dealer startbalance >> ");
                                int DealerInitial = int.Parse(Console.ReadLine());
                                game.dealer.Balance = 0;
                                game.AddMoney(game.dealer, DealerInitial);
                                Console.Clear();
                                break;
                            case "4":
                                active = false;
                                PrintJoinedPlayers();
                                Console.ReadKey();
                                Console.Clear();
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
            Console.WriteLine("Revert back to main Menu? y/n");
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
        private void RemoveBankrupt()
        {
            List<RegularPlayer> bankruptPlayers = game.ReturnBankrupt();

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
            RemoveBankrupt();//extract and remove bankrupt players before every round

            //clearing bets
            foreach (RegularPlayer player in game.players)
            {
                player.Bet = 0;
            }

            //clearing hands
            foreach (RegularPlayer player in game.players)
            {
                player.Hand.Clear();
            }
            game.dealer.Hand.Clear();

            //initializing and shuffling deck
            game.InitializeDeck();
        }
        private string MakeBet(RegularPlayer player)
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
        private void ProcessBets()
        {
            foreach (RegularPlayer player in game.players)
            {
                MakeBet(player);
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
            return Rules.isBust(player.Hand) ? "BUST" : "";
        }
        private void PrintHands()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            foreach (RegularPlayer player in game.players)
            {
                Console.Write(player.Name + ": ");
                foreach(Card c in player.Hand.Cards)
                {
                    Console.Write(c.ToString() + " ");
                }
                Console.Write($"({Rules.GethandValue(player.Hand)}) {ReturnIfBust(player)}");
                Console.WriteLine();
            }

            int dealerCount = 0;
            Console.Write(game.dealer.Name + " ");
            foreach(Card c in game.dealer.Hand.Cards)
            {
                dealerCount++;
                if (dealerCount == 2)
                    c.IsHidden = true;
                Console.Write(c.ToString() + " ");
            }
            Console.WriteLine();
            Console.ResetColor();
        }
        private void PrintHandsLast()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            foreach (RegularPlayer player in game.players)
            {
                Console.Write(player.Name + ": ");
                foreach (Card c in player.Hand.Cards)
                {
                    Console.Write(c.ToString() + " ");
                }
                Console.Write($"({Rules.GethandValue(player.Hand)}) {ReturnIfBust(player)}");
                Console.WriteLine();
            }
            Console.Write(game.dealer.Name + " ");
            foreach (Card c in game.dealer.Hand.Cards)
            {
                c.IsHidden = false;
                Console.Write(c.ToString() + " ");
            }
            Console.Write($"({Rules.GethandValue(game.dealer.Hand)}) {ReturnIfBust(game.dealer)}");
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
            Console.ForegroundColor = ConsoleColor.Black;
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
                Winninghand currentWinner = game.ReturnWinner(player);
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
        private void MakeDecision(Player player)
        {
            bool bustOrStay = false;
            while (!bustOrStay)
            {
                if (Rules.GethandValue(player.Hand) > 21)
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
        private bool ProcessDecisions()
        {
            foreach(RegularPlayer player in game.players)
            {
                MakeDecision(player);
            }
            MakeDecision(game.dealer);
            return false;//returns false to say there is no more active players
        }
    }
}
