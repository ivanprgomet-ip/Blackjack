using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleGui
{
    public interface IPlayer
    {
        Guid Id { get; }
        Hand Hand { get; }
        string Name { get; }
        int TryBet();
        PlayerDecision MakeDecision(Hand hand);
    }
}
