using Hearts.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Hearts.Core
{
    public class HeartsGame
    {
        #region Declarations
        const int HAND_SIZE = 13;

        /*
         *      0
         *  3       1
         *      2
         */
        public List<Player> Players { get; set; } = ListPool<Player>.Obtain();
        public int TurnNumber { get; private set; } = 1;
        public int RoundNumber { get; private set; } = 1;
        public bool CanLeadWithHearts { get; set; } = true;
        int _leadPlayerIdx;
        Deck _deck = new Deck();
        #endregion

        #region Public Methods
        public void Reset()
        {
            foreach ( var player in Players )
            {
                player.EmptyHandAndTricks();
                player.CanLeadHearts = false;
            }
            TurnNumber = 1;
            _deck.Shuffle();
        }

        /// <summary>
        /// Will deal the deck out to the players
        /// </summary>
        public void DealToPlayers()
        {
            Debug.Assert( Players.Count == 4 );

            _deck.Shuffle();
            for( var i = 0; i < Players.Count; ++i )
            {
                var player = Players[i];
                for ( int j = 0; j < HAND_SIZE; ++j )
                {
                    player.Hand.Add( _deck.RemoveTopCard() );
                }
            }
        }

        public void PlayTrick()
        {

            var trick = Pool<Trick>.Obtain();
            // how many players to get cards from
            int limit = 4;
            if ( RoundNumber == 1 )
            {
                PassPhase();
                _leadPlayerIdx = FindInitialLeadPlayerIndex();
                var leadPlayer = Players[_leadPlayerIdx];
                var card = leadPlayer.Hand.First( c => c.CardRank == 2 && c.Suit == Suit.Clubs );
                _leadPlayerIdx = ( _leadPlayerIdx + 1 ) % 4;
                trick.AddCard( card, leadPlayer );
                limit = 3;

            }   

            for ( int i = 0, idx = _leadPlayerIdx; i < limit; ++i, idx = ( idx + 1 ) % 4 )
            {
                var currentPlayer = Players[idx];
                var playCard = currentPlayer.GetPlayCard( TurnNumber, trick );
                currentPlayer.Hand.Remove( playCard );
                if ( !this.ValidPlayCard( playCard, currentPlayer, trick ) )
                {
                    throw new ArgumentException( $"Invalid card: {playCard} \nplayed for the trick: {trick}" );
                }

                //Player has voided the suit and can therefore play Hearts
                if ( !CanLeadWithHearts && playCard.Suit == Suit.Hearts )
                {
                    this.NotifyPlayersCanLeadHearts();
                }

                trick.AddCard( playCard, currentPlayer );
            }

            _leadPlayerIdx = Players.IndexOf( trick.GetWinner() );
            var winner = Players[_leadPlayerIdx];
            winner.TricksWon.Add( trick );
            TurnNumber += 1;
        }

        public void PlayRound(bool handsDealt = false)
        {
            if ( !handsDealt )
            {
                DealToPlayers();
            }

            for( int i = 0; i < HAND_SIZE; ++i )
            {
                PlayTrick();
            }
            
            //Get the cards back into the deck
            _deck.ResetDeck();
        }



        public bool IsGameOver()
        {
            const int POINT_LIMIT = 100;

            foreach ( var player in Players )
            {
                if ( player.Points == POINT_LIMIT )
                {
                    return true;
                }
            }

            return false;
        }
        #endregion

        #region Private Methods
        private void PassPhase()
        {
            int mod = RoundNumber % 4;

            switch ( mod )
            {
                case 0:
                    break;
                case 1:
                    Players[0].PassCards( TurnNumber, Players[1] );
                    Players[1].PassCards( TurnNumber, Players[2] );
                    Players[2].PassCards( TurnNumber, Players[3] );
                    Players[3].PassCards( TurnNumber, Players[0] );
                    break;
                case 2:
                    Players[0].PassCards( TurnNumber, Players[3] );
                    Players[1].PassCards( TurnNumber, Players[0] );
                    Players[2].PassCards( TurnNumber, Players[1] );
                    Players[3].PassCards( TurnNumber, Players[2] );
                    break;
                case 3:
                    Players[0].PassCards( TurnNumber, Players[2] );
                    Players[1].PassCards( TurnNumber, Players[3] );
                    Players[2].PassCards( TurnNumber, Players[0] );
                    Players[3].PassCards( TurnNumber, Players[1] );
                    break;
            }

            if ( mod != 0 )
            {
                foreach (var player in Players )
                {
                    player.AddQueuedCardsToHand();
                }
            }
        }

        /// <summary>
        /// Finds the index of the player that should lead the initial trick
        /// </summary>
        /// <returns>Index of lead player</returns>
        private int FindInitialLeadPlayerIndex()
        {
            int idx = -1;
            for ( int i = 0; i < Players.Count; ++i )
            {
                if ( Players[i].ShouldLead() )
                {
                    idx = i;
                    break;
                }
            }
            Debug.Assert( idx != -1 );
            return idx;
        }

        private bool ValidPlayCard(Card card, Player player, Trick trick)
        {

            if ( trick.Count == 0 )
            {
                if ( TurnNumber == 1 )
                {
                    return ( card.Suit == Suit.Clubs && card.CardRank == 2 );
                }
                else
                {
                    if ( card.Suit == Suit.Hearts || ( card.Suit == Suit.Spades && card.CardRank == Card.QUEEN ) )
                    {
                        return CanLeadWithHearts;
                    }
                }
            }

            //If they match the suit its valid
            if ( card.Suit == trick.LeadSuit )
            {
                return true;
            }

            //If they don't match the suit and their hand is not void of the suit, its a bad play
            if ( player.HasSuit( trick.LeadSuit ) )
            {
                return false;
            }

            return true;

        }

        private void NotifyPlayersCanLeadHearts()
        {
            CanLeadWithHearts = true;
            foreach ( var player in Players )
            {
                player.CanLeadHearts = true;
            }
        }

        #endregion
    }
}
