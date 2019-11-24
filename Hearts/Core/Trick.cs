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
            return Cards.Where( kv => kv.Key.Suit == LeadSuit )
                  .OrderByDescending( kv => kv.Key.CardRank )
                  .First().Value;
        }

        public void Reset()
        {
            Cards.Clear();
            LeadSuit = default( Suit );
        }

        public override string ToString()
        {
            if ( OrderedCards.Count == 0 )
            {
                return "Empty Trick!";
            }

            string returnStr = "{\n";


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
