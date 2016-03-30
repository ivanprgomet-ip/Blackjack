using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CardLogic;

namespace BlackjackLogic
{
    public class AiPlayer : RegularPlayer
    {
        public AiPlayer(string name):base(name)
        {

        }

        public override PlayerDecision MakeDecision()
        {
            var handValue = Rules.GethandValue(Hand);

            if (Bet > 7)
            {
                if (handValue > 16)
                    return PlayerDecision.Stay;
                else
                    return PlayerDecision.Hit;
            }
            else
            {
                if (handValue > 18)
                    return PlayerDecision.Stay;
                else
                    return PlayerDecision.Hit;
            }
        }
        public override int MakeBet()
        {
            if (Balance <= 5)
                return 1;
            if (Balance <= 20)
                return 2;
            if (Balance <= 50)
                return 7;
            if (Balance <= 100)
                return 10;
            return 10;
        }
    }
}
