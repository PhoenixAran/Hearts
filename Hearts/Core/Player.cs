using System;
using System.Collections.Generic;
using System.Text;

namespace Hearts.Core
{
    public abstract class Player
    {
        public int Points { get; private set; }

        public List<Card> CardsWon { get; set; }

        public List<Card> Hand { get; private set; } = new List<Card>();

        public List<Card> TransferList { get; private set; } = new List<Card>();

        public void ReceiveCards( List<Card> newCards )
        {
            for( int i = newCards.Count; i >= 0; --i )
            {
                Hand.Add( newCards[i] );
                newCards.RemoveAt( i );
            }
        }

        /// <summary>
        /// If this player has the two of clubs
        /// </summary>
        /// <returns></returns>
        public bool ShouldLead( int numPlayers )
        {
            if ( !(numPlayers == 3) || (numPlayers == 4) )
            {
                throw new ArgumentOutOfRangeException( "Only 3 or 4 players are supported" );
            }

            var leadSuit = numPlayers == 3 ? Suit.Diamonds : Suit.Clubs;

            foreach( var card in Hand)
            {
                if ( numPlayers == 4 )
                {
                    if ( card.CardRank == 2 && card.Suit == leadSuit )
                        return true;
                }
            }
            return false;
        }

        public abstract List<Card> PassCards( int roundNumber, Player otherPlayer );

        public abstract Card GetPlayCard( List<Card> currentTrick );
    }
}
