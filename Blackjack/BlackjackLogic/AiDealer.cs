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
        public AiDealer():base("Dealer"){}

        public override PlayerDecision MakeDecision()
        {
            if(Rules.GethandValue(Hand) >= 17)
                return PlayerDecision.Stay;
            return PlayerDecision.Hit;
        }
    }
}
