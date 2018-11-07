using Blackjack.dto.types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blackjack.dto
{
    class Deck
    {
        public List<Card> cards { get; set; }

        public Deck()
        {
            this.initializeDeck();
        }

        private void initializeDeck()
        {
            this.cards = new List<Card>();

            foreach (Suit suit in Enum.GetValues(typeof(Suit)))
            {
                foreach (Value value in Enum.GetValues(typeof(Value)))
                {
                    this.cards.Add(new Card(value, suit));
                }
            }
        }
    }
}
