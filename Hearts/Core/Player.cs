using Hearts.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hearts.Core
{
    public abstract class Player
    {
        public int Points { get; private set; }

        public List<Trick> TricksWon { get; set; } = ListPool<Trick>.Obtain();

        public List<Card> Hand { get; private set; } = ListPool<Card>.Obtain();

        public void ReceiveCards( IEnumerable<Card> newCards )
        {
            foreach ( var card in newCards )
            {
                Hand.Add( card );
            }
        }

        /// <summary>
        /// If this player has the two of clubs
        /// </summary>
        /// <returns></returns>
        public virtual bool ShouldLead()
        {
            foreach( var card in Hand)
            {
                if ( card.CardRank == 2 && card.Suit == Suit.Diamonds )
                    return true;                
            }
            return false;
        }

        public void EmptyHandAndTricks()
        {
            Hand.Clear();
            foreach ( var trick in TricksWon)
            {
                Pool<Trick>.Free( trick );
            }
        }

        public abstract void PassCards( int roundNumber, Player otherPlayer );

        /// <summary>
        /// Do not add directly to trick.
        /// Return card.
        /// </summary>
        /// <param name="currentTrick"></param>
        /// <returns></returns>
        public abstract Card GetPlayCard( Trick currentTrick );
    }
}
