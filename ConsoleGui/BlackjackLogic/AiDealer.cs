using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CardLogic;

namespace BlackjackLogic
{
    public class AiDealer : Player
    {
        public AiDealer(string name):base(name="Dealer")
        {
            //TODO: see if code above is legit
        }

        public override PlayerDecision MakeDecision()
        {
            if(Rules.GethandValue(_Hand) >= 17)
                return PlayerDecision.Stay;
            return PlayerDecision.Hit;
        }
    }
}
