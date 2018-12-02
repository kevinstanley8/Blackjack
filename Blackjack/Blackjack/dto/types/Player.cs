using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blackjack.dto.types
{
    public class Player
    {
        public List<List<Card>> hand { get; set; }

        public Player()
        {
            this.hand = new List<List<Card>>();
            this.hand.Add(new List<Card>());
        }

        public void ClearHand()
        {
            this.hand.Clear();
            this.hand.Add(new List<Card>());
        }

        public void splitHand()
        {
            List<Card> newHand = new List<Card>();
            newHand.Add(this.hand[0][1]);
            this.hand[0].RemoveAt(1);
            this.hand.Add(newHand);
        }

    }
}
