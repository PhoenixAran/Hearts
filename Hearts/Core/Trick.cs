using Hearts.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Hearts.Core
{
    public class Trick : IPoolable
    {
        public Dictionary<Card, Player> Cards = new Dictionary<Card, Player>();
        public List<Card> OrderedCards = ListPool<Card>.Obtain();
        public int Count
        {
            get
            {
                Debug.Assert( Cards.Count == OrderedCards.Count );
                return Cards.Count;
            }
        }

        public Suit LeadSuit;

        public void AddCard( Card card, Player player )
        {
            if ( Cards.Count == 0 )
                LeadSuit = card.Suit;
            OrderedCards.Add( card );
            Cards.Add( card, player );
        }

        public Player GetWinner()
        {
            var tempList = ListPool<Card>.Obtain();

            foreach ( var card in OrderedCards )
            {
                if ( card.Suit == LeadSuit )
                {
                    tempList.Add( card );
                }
            }

            tempList.Sort();
            var winner = Cards[tempList[tempList.Count - 1]];
            ListPool<Card>.Free( tempList );

            return winner;
        }

        public void Reset()
        {
            Cards.Clear();
            OrderedCards.Clear();
            LeadSuit = default( Suit );
        }
            
        public int GetPenaltyPoints()
        {
            const int QUEEN_PENALTY_POINTS = 13;
            int points = 0;
            foreach ( var card in OrderedCards )
            {
                if ( card.Suit == Suit.Hearts )
                {
                    ++points;
                }
                else if ( card.Suit == Suit.Spades && card.CardRank == Card.QUEEN ) //Check if it's the queen of spades
                {
                    points += QUEEN_PENALTY_POINTS;
                }
            }
            return points;
        }

        public override string ToString()
        {
            if ( OrderedCards.Count == 0 )
            {
                return "Empty Trick!";
            }

            string returnStr = "{";

            foreach( var card in OrderedCards )
            {
                returnStr += "\n";
                returnStr += card.ToString();
                
            }
            returnStr += "\n}";

            return returnStr;
        }

    }
}
