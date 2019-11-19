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
            var game = new Game();
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
        
        public void TurnOrderTest()
        {
            var game = new Game();
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
        }


    }
}
