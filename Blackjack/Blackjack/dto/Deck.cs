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
        private int deckIndex { get; set; }

        public Deck()
        {
            this.initializeDeck();
            this.deckIndex = -1;
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

        public Card getNextCard()
        {
            //if getting near the end of the deck go ahead and shuffle and start deck again to ensure you don't run out of cards
            if (this.deckIndex >= 45)
                this.shuffleDeck();

            this.deckIndex++;
            return this.cards[deckIndex];
        }

        public void shuffleDeck()
        {
            //shuffle deck
            this.deckIndex = -1;
        }
    }
}
