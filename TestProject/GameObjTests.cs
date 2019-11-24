using System;
using Xunit;
using Moq;
using System.Collections.Generic;
using Hearts.Core;
using System.Linq;

namespace TestProject
{
    public class GameObjTests
    {
        [Fact]
        public void GameInitializeTest()
        {
            var game = new HeartsGame();
            for ( int i = 0; i < 4; ++i )
            {
                
                var moqP = new Mock<Player>();
                switch ( i )
                {
                    case 0:
                        moqP.Setup( p => p.ShouldLead() )
                            .Returns( false );

                        moqP.Setup( p => p.GetPlayCard( new Trick() ) )
                            .Returns( new Card( 3, Suit.Clubs ) );
                        break;
                    case 1:
                        moqP.Setup( p => p.ShouldLead() )
                            .Returns( true );

                        moqP.Setup( p => p.GetPlayCard( new Trick() ) )
                            .Returns( new Card( 2, Suit.Clubs ) );
                        break;
                    case 2:
                        moqP.Setup( p => p.ShouldLead() )
                            .Returns( false );

                        moqP.Setup( p => p.GetPlayCard( new Trick() ) )
                            .Returns( new Card( 5, Suit.Clubs ) );
                        break;
                    case 3:
                        moqP.Setup( p => p.ShouldLead() )
                            .Returns( false );

                        moqP.Setup( p => p.GetPlayCard( new Trick() ) )
                            .Returns( new Card( 10, Suit.Clubs ) );
                        break;

                }
            

                game.Players.Add( moqP.Object );
            }

            game.StartGame();
            Assert.Equal( 4 , game.Players.Where( p => p.Hand.Count == 13 ).Count());
            
        }
        
        [Fact]
        public void PlayTest()
        {
            var game = new HeartsGame();
            for ( int i = 0; i < 4; ++i )
            {

                var moqP = new Mock<Player>();
                switch ( i )
                {
                    case 0:
                        moqP.Setup( p => p.ShouldLead() )
                            .Returns( false );

                        moqP.Setup( p => p.GetPlayCard( It.IsAny<Trick>() ) )
                            .Returns( new Card( 3, Suit.Clubs ) );
                        break;
                    case 1:
                        moqP.Setup( p => p.ShouldLead() )
                            .Returns( true );

                        moqP.Setup( p => p.GetPlayCard( It.IsAny<Trick>() ) )
                            .Returns( new Card( 2, Suit.Clubs ) );
                        break;
                    case 2:
                        moqP.Setup( p => p.ShouldLead() )
                            .Returns( false );

                        moqP.Setup( p => p.GetPlayCard( It.IsAny<Trick>() ) )
                            .Returns( new Card( 5, Suit.Clubs ) );
                        break;
                    case 3:
                        moqP.Setup( p => p.ShouldLead() )
                            .Returns( false );

                        moqP.Setup( p => p.GetPlayCard( It.IsAny<Trick>() ) )
                            .Returns( new Card( 10, Suit.Clubs ) );
                        break;

                }


                game.Players.Add( moqP.Object );
            }

            game.StartGame();
            game.PlayTrick();

            Assert.Single( game.Players[3].TricksWon);
            var trick = game.Players[3].TricksWon[0];
            Assert.Equal( 4 , trick.Cards.Count);
        }

        
        [Fact]
        public void FirstTrickTest()
        {
            var turnHistory = new List<int>();
            var game = new HeartsGame();
            List<Trick> player4Tricks = new List<Trick>();

            var players = new List<Mock<Player>>();
            var player1 = new Mock<Player>();
            var player2 = new Mock<Player>();
            var player3 = new Mock<Player>();
            var player4 = new Mock<Player>();

            players.Add( player1 );
            players.Add( player2 );
            players.Add( player3 );
            players.Add( player4 );

            game.Players.AddRange( players.Select( m => m.Object ) );
;
            for ( int i = 0; i < 4; ++i )
            {
                var player = players[i];
                if ( i == 3 )
                {
                    player.Setup( p => p.ShouldLead() )
                          .Returns( true );
                }
                else
                {
                    player.Setup( p => p.ShouldLead() )
                            .Returns( false );
                }
            }
            player4.Setup( p => p.GetPlayCard( It.IsAny<Trick>() ) )
                   .Returns( new Card( 2, Suit.Clubs ) )
                   .Callback( () =>
                   {
                       turnHistory.Add( 4 );
                   });

            player3.Setup( p => p.GetPlayCard( It.IsAny<Trick>() ) )
                   .Returns( new Card( 3, Suit.Clubs ) )
                   .Callback( () =>
                   {
                       turnHistory.Add( 3 );
                   });

            player2.Setup( p => p.GetPlayCard( It.IsAny<Trick>() ) )
                   .Returns( new Card( 10, Suit.Clubs ) )
                   .Callback( () =>
                   {
                       turnHistory.Add( 2 );
                   });

            player1.Setup( p => p.GetPlayCard( It.IsAny<Trick>() ) )
                   .Returns( new Card( 14, Suit.Clubs ) )
                   .Callback( () =>
                   {
                       turnHistory.Add( 1 );
                   });

            game.PlayTrick();

            Assert.Equal( new List<int> { 4, 1, 2, 3 }, turnHistory );


       
        }

        
    }
}
