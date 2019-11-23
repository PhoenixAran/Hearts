using Hearts.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hearts.Core
{
    public class Deck
    {
        public List<Card> Cards { get; set; }
        public int Size => Cards.Count;

        private Suit[] _suits;

        private Random _rng;

        public int NumPlayers => 4;

        public Deck()
        {
            Cards = ListPool<Card>.Obtain();
            _suits = new[] { Suit.Clubs, Suit.Diamonds, Suit.Hearts, Suit.Spades };
            _rng = new Random();

            for ( int i = Card.MIN_CARD_RANK; i <= Card.MAX_CARD_RANK; ++i )
            {
                for ( int j = 0; j < _suits.Length; ++j )
                {
                    Cards.Add( new Card( i, _suits[j] ) );
                }
            }
        }

        public void Shuffle ()
        {
            int index = Cards.Count;
            while ( index > 0 )
            {
                --index;
                int j = _rng.Next( index + 1 );
                var card = Cards[j];
                Cards[j] = Cards[index];
                Cards[index] = card;
            }
        }


    }
}
