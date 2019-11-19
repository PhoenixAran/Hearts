using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hearts.Core
{
    public class Trick
    {
        Dictionary<Card, Player> _cards;
        Suit _leadSuit;

        public void AddCard( Card card, Player player )
        {
            if ( _cards.Count == 0 )
                _leadSuit = card.Suit;
            _cards.Add( card, player );
        }

        public Player GetWinner()
        {
            return _cards.Where( kv => kv.Key.Suit == _leadSuit )
                  .OrderByDescending( kv => kv.Key.CardRank )
                  .First().Value;
        }


        public void Reset() => _cards.Clear();
       
    }
}
