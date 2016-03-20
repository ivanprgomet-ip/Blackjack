using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleGui
{
    /// <summary>
    /// Interface implemented by HumanPlayers and 
    /// AiPlayers for making bets.
    /// </summary>
    public interface IGambler
    {
        int MakeBet();
        int Bet { get; set; }
    }
}
