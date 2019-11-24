using Hearts.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Hearts.Core
{
    public abstract class Player
    {
        public int Points { get; private set; }

        public List<Trick> TricksWon { get; private set; } = ListPool<Trick>.Obtain();

        public List<Card> Hand { get;  set; } = ListPool<Card>.Obtain();

        /// <summary>
        /// Where to put cards that the player will recieve in the passing phase
        /// This is done here so the hand is not polluted. Players cards cannot give away
        /// cards that they will receive
        /// </summary>
        public List<Card> QueuedCards { get; private set; } = ListPool<Card>.Obtain();

        /// <summary>
        /// Queues cards for this player to recieve
        /// </summary>
        /// <param name="newCards">Cards to recieve</param>
        public void QueueRecieveCards( List<Card> newCards )
        {
            Debug.Assert( newCards.Count == 3 );
            QueuedCards.AddRange( newCards );
        }

        /// <summary>
        /// To be called after the passing phase
        /// </summary>
        public void AddQueuedCards()
        {
            Hand.AddRange( QueuedCards );
            QueuedCards.Clear();
        }

        /// <summary>
        /// This will be set by the game object
        /// </summary>
        public bool CanLeadHearts { get; set; }

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

        public bool HasSuit( Suit suit )
        {
            foreach ( var card in Hand )
            {
                if ( card.Suit == suit )
                    return true;
            }
            return false;
        }

        public abstract void PassCards( int roundNumber, Player otherPlayer );

        /// <summary>
        /// Do not add directly to trick.
        /// Returns card that this AI will play
        /// </summary>
        /// <param name="currentTrick"></param>
        public abstract Card GetPlayCard( Trick currentTrick );


    }
}
