using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Hearts.Core
{
    public class Game
    {

        const int HAND_SIZE = 13;

        /*
         *      0
         *  3       1
         *      2
         */
        public List<Player> Players { get; set; } = new List<Player>();
        public int TurnNumber { get; private set; } = 1;
        public int RoundNumber { get; private set; } = 1;

        int _leadPlayerIdx;
        Deck _deck = new Deck();
        
        public void StartGame()
        {
            Debug.Assert( Players.Count == 4 );

            _deck.Shuffle();

            for( var i = 0; i < Players.Count; ++i )
            {
                var player = Players[i];
                player.ReceiveCards( _deck.Cards.GetRange(i * HAND_SIZE, HAND_SIZE));
            }

        }

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
        }

        private int FindLeadPlayer()
        {
            return Players.IndexOf( Players.First( p => p.ShouldLead() ) );
        }

        private void Reset()
        {
            foreach( var player in Players )
            {
                player.EmptyHandAndTricks();
            }
            TurnNumber = 1;
        }

        public void PlayTrick()
        {
            if ( RoundNumber == 1 )
            {
                PassPhase();
                _leadPlayerIdx = FindLeadPlayer();
            }


            var trick = Pool<Trick>.Obtain();
            var leadPlayer = Players[_leadPlayerIdx];

            for ( int i = 0, idx = _leadPlayerIdx; i < 4; ++i, idx = ( idx + 1 ) % 4 )
            {
                var currentPlayer = Players[idx];
                trick.AddCard( currentPlayer.GetPlayCard( trick ), currentPlayer );
            }

            _leadPlayerIdx = Players.IndexOf( trick.GetWinner() );
            var winner = Players[_leadPlayerIdx];
            winner.TricksWon.Add( trick );
        }

        public void PlayRound()
        {
            for( int i = 0; i < HAND_SIZE; i = i + 1 )
            {
                PlayTrick();
            }
            
        }
    }
}
