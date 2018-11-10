using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blackjack.dto.types
{
    class Player
    {
        public List<Card> hand { get; set; }

        public Player()
        {
            this.hand = new List<Card>();
        }
    }
}
