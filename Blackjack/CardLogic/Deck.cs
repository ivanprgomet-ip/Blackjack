using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardLogic
{
    public class Deck
    {
        private Card[] _cards;
        private int _index;

        public Deck()
        {
            _cards = new Card[52];
            _index = 0;
            Initialize();
        }

        private void Initialize()
        {
            int count = 0;

            for (int i = 0; i < 4; i++)
            {
                int value = 1;
                for (int j = 0; j < 13; j++)
                {
                    if (i == 0)
                        _cards[count] = new Card("♣", value);
                    if (i == 1)
                        _cards[count] = new Card("♦", value);
                    if (i == 2)
                        _cards[count] = new Card("♥", value);
                    if (i == 3)
                        _cards[count] = new Card("♠", value);

                    value++;
                    count++;
                }
            }
        }

        public void Shuffle()
        {
            _index = 0;
            var rnd = new Random();

            for (int i = 0; i < _cards.Length; i++)
            {
                SwapCards(i, rnd.Next(_cards.Length));
            }
        }

        private void SwapCards(int card1, int card2)
        {
            var tmpCard = _cards[card1];
            _cards[card1] = _cards[card2];
            _cards[card2] = tmpCard;
        }
        
        public Card ReturnCard()
        {
                return _cards[_index++ % 52];
        }
    }
}
