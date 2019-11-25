using System;
using System.Collections.Generic;
using System.Text;

namespace Hearts.Core
{
    public class Card : IEquatable<Card>, IComparable<Card>
    {
        public const int MIN_CARD_RANK = 2;
        public const int MAX_CARD_RANK = 14; //Ace

        public const int JOKER = 11;
        public const int QUEEN = 12;
        public const int KING = 13;
        public const int ACE = 14;

        public readonly int CardRank;
        public readonly Suit Suit;

        public Card( int cardRank, Suit suit)
        {
            if ( !( MIN_CARD_RANK <= cardRank && cardRank <= MAX_CARD_RANK ) )
            {
                throw new IndexOutOfRangeException( "CardRank should be between 1 and 14" );
            }
            CardRank = cardRank;
            Suit = suit;
        }

        public Card( Suit suit, int cardRank ) : this( cardRank, suit ) { }


        public override bool Equals( object obj )
        {
            return obj is Card card && this.Equals( card );
        }

        public override int GetHashCode()
        {
            var hashCode = 746792037;
            hashCode = hashCode * -1521134295 + CardRank.GetHashCode();
            hashCode = hashCode * -1521134295 + Suit.GetHashCode();
            return hashCode;
        }

        #region IEquatable
        public bool Equals( Card other )
        {
            return CardRank == other.CardRank && Suit == other.Suit;
        }
        #endregion

        #region IComparable
        int IComparable<Card>.CompareTo( Card other )
        {
            return this.CardRank.CompareTo( other.CardRank );
        }
        #endregion

        public override string ToString()
        {
            string cardRankString;
            switch ( CardRank )
            {
                case 11:
                    cardRankString = "Jack";
                    break;
                case 12:
                    cardRankString = "Queen";
                    break;
                case 13:
                    cardRankString = "King";
                    break;
                case 14:               
                    cardRankString = "Ace";
                    break;
                default:
                    cardRankString = $"{CardRank}";
                    break;
            }

            return $"{cardRankString} of {Suit}";
        }
    }
}
