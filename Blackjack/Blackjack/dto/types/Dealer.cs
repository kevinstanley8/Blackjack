using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blackjack.dto.types
{
    public class Dealer
    {
        public List<Card> hand { get; set; }

        public Dealer()
        {
            this.hand = new List<Card>();
        }

        public void ClearHand()
        {
            hand.Clear();
        }
    }
}
