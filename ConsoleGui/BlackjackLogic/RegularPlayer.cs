using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleGui
{
    public abstract class RegularPlayer:Player
    {
        int Bet { get; set; }
        public abstract int MakeBet();
    }
}
