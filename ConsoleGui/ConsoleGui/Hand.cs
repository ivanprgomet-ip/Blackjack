using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleGui
{
    public class Hand
    {
        public List<Card> Cards { get; }

        public Hand()
        {
            Cards = new List<Card>();
        }

        public void AddCardToHand(Card card)
        {
            Cards.Add(card);
        }

        public void Clear()
        {
            Cards.Clear();
        }
    }
}
