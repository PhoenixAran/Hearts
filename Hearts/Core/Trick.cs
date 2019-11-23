using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hearts.Core
{
    public class Trick : IPoolable
    {
        public Dictionary<Card, Player> Cards = new Dictionary<Card, Player>();
        Suit _leadSuit;

        public void AddCard( Card card, Player player )
        {
            if ( Cards.Count == 0 )
                _leadSuit = card.Suit;
            Cards.Add( card, player );
        }

        public Player GetWinner()
        {
            return Cards.Where( kv => kv.Key.Suit == _leadSuit )
                  .OrderByDescending( kv => kv.Key.CardRank )
                  .First().Value;
        }


        public void Reset()
        {
            Cards.Clear();
            _leadSuit = default( Suit );
        }
       
    }
}
