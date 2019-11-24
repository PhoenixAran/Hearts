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

        public List<Card> Hand { get;  set; } = ListPool<Card>.Obtain();


        public List<Trick> TricksWon = ListPool<Trick>.Obtain();

        /// <summary>
        /// Where to put cards that the player will recieve in the passing phase
        /// This is done here so the hand is not polluted. Players cards cannot give away
        /// cards that they will receive
        /// </summary>
        private List<Card> _queuedCards = ListPool<Card>.Obtain();


        /// <summary>
        /// Cards won from the tricks
        /// </summary>
        protected List<Card> _cardsWon = ListPool<Card>.Obtain();

        /// <summary>
        /// Queues cards for this player to recieve
        /// </summary>
        /// <param name="newCards">Cards to recieve</param>
        public void QueueRecieveCards( List<Card> newCards )
        {
            Debug.Assert( newCards.Count == 3 );
            _queuedCards.AddRange( newCards );
        }

        /// <summary>
        /// To be called after the passing phase
        /// </summary>
        public void AddQueuedCards()
        {
            Hand.AddRange( _queuedCards );
            _queuedCards.Clear();
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
            _cardsWon.Clear();
            for ( int i = TricksWon.Count - 1; i >= 0; --i )
            {
                var trick = TricksWon[i];
                TricksWon.RemoveAt( i );
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

        public void WinTrick( Trick trick )
        {
            TricksWon.Add( trick );
            _cardsWon.AddRange( trick.OrderedCards );
            Points += trick.GetPenaltyPoints();
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
