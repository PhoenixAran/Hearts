using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Hearts.Core
{
    public class Game
    {
        /*
         *      0
         *  3       1
         *      2
         */

        public List<Player> Players { get; set; } = new List<Player>();
        public int TurnNumber { get; private set; } = 1;

        int _leadPlayerIdx;
        Deck _deck = new Deck();
        
        public void StartGame()
        {
            Debug.Assert( Players.Count == 4 );

            const int HAND_SIZE = 13;
            _deck.Shuffle();

            foreach( var player in Players )
            {
                player.ReceiveCards( _deck.Cards.Take( HAND_SIZE ) );
            }

        }

        private void PassPhase()
        {
            int mod = TurnNumber % 4;

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

        public void PlayRound()
        {
            if ( TurnNumber == 1 )
                _leadPlayerIdx = FindLeadPlayer();
            
           
            var trick = new Trick();
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
        

    }
}
