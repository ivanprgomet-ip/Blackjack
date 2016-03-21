using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CardLogic;

namespace BlackjackLogic
{
    public abstract class RegularPlayer:Player
    {
        public int Bet { get; set; }
        public abstract int MakeBet();

        public RegularPlayer(string name):base(name)
        {

        }
    }
}
