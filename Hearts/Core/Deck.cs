using Hearts.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hearts.Core
{
    public class Deck
    {
        public int Size => _cards.Count;

        private List<Card> _removedCards = ListPool<Card>.Obtain();

        private List<Card> _cards = ListPool<Card>.Obtain();

        private Suit[] _suits;

        private Random _rng;

        public int NumPlayers => 4;

        public Deck()
        {
            _suits = new[] { Suit.Clubs, Suit.Diamonds, Suit.Hearts, Suit.Spades };
            _rng = new Random();

            for ( int i = Card.MIN_CARD_RANK; i <= Card.MAX_CARD_RANK; ++i )
            {
                for ( int j = 0; j < _suits.Length; ++j )
                {
                    _cards.Add( new Card( i, _suits[j] ) );
                }
            }
        }

        public void Shuffle ()
        {
            int index = _cards.Count;
            while ( index > 0 )
            {
                --index;
                int j = _rng.Next( index + 1 );
                var card = _cards[j];
                _cards[j] = _cards[index];
                _cards[index] = card;
            }
        }

        public void ResetDeck()
        {
            _cards.AddRange( _removedCards );
            _removedCards.Clear();
        }

        public Card RemoveTopCard()
        {
            var card = _cards[_cards.Count - 1];

            _removedCards.Add( card );
            _cards.RemoveAt( _cards.Count - 1 );

            return card;
        }

    }
}
