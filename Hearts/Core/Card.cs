using System;
using System.Collections.Generic;
using System.Text;

namespace Hearts.Core
{
    public class Card : IEquatable<Card>, IComparable<Card>
    {
        public const int MIN_CARD_RANK = 2;
        public const int MAX_CARD_RANK = 14; //Ace

        public int CardRank { get; }
        public Suit Suit { get; }

        public Card( int cardRank, Suit suit)
        {
            if ( !( MIN_CARD_RANK <= cardRank && cardRank <= MAX_CARD_RANK ) )
            {
                throw new IndexOutOfRangeException( "CardRank should be between 1 and 14" );
            }
            CardRank = cardRank;
            Suit = suit;
        }


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
    }
}
