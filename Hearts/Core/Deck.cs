using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hearts.Core
{
    public class Deck
    {

        private List<Card> _cards;

        private Suit[] _suits;

        private Random _rng;

        public int NumPlayers { get; set; }



        public Deck( int numPlayers )
        {
            _cards = new List<Card>();
            _suits = new[] { Suit.Clubs, Suit.Diamonds, Suit.Hearts, Suit.Spades };
            _rng = new Random();

            NumPlayers = numPlayers;

            for ( int i = Card.MIN_CARD_RANK; i <= Card.MAX_CARD_RANK; ++i )
            {
                for ( int j = 0; j < _suits.Length; ++j )
                {
                    _cards.Add( new Card( i, _suits[j] ) );
                }
            }

            switch ( numPlayers )
            {
                case 3:
                    _cards.Remove( _cards.First( c => c.CardRank == 2 && c.Suit == Suit.Clubs ) );
                    break;
                case 4:    
                    break;
                default:
                    throw new ArgumentOutOfRangeException( $"Only 3 or 4 players are supported" );
            }

        }

        public void Shuffle ()
        {
            int index = _cards.Count;
            while ( index >= 0 )
            {
                --index;
                int j = _rng.Next( index + 1 );
                var card = _cards[j];
                _cards[j] = _cards[index];
                _cards[index] = card;
            }
        }


    }
}
