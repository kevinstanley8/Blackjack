using Blackjack.dto.types;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blackjack.dto
{
    public class Deck
    {
        public List<Card> cards { get; set; }
        private int deckIndex { get; set; }
        private int shuffleAmount;
        private Random rand;

        public Deck()
        {
            this.InitializeDeck();
            this.deckIndex = -1;
            shuffleAmount = 1000;
            rand = new Random();
        }

        private void InitializeDeck()
        {
            this.cards = new List<Card>();

            this.cards.Add(new Card(Value.ACE, Suit.SPADE));
            this.cards.Add(new Card(Value.KING, Suit.SPADE));
            this.cards.Add(new Card(Value.TWO, Suit.HEART));
            this.cards.Add(new Card(Value.KING, Suit.HEART));

            foreach (Suit suit in Enum.GetValues(typeof(Suit)))
            {
                foreach (Value value in Enum.GetValues(typeof(Value)))
                {
                    this.cards.Add(new Card(value, suit));
                }
            }
        }

        public Card GetNextCard()
        {
            //if getting near the end of the deck go ahead and shuffle and start deck again to ensure you don't run out of cards
            if (this.deckIndex >= 45)
                this.ShuffleDeck();

            this.deckIndex++;
            return this.cards[deckIndex];
        }

        public void ShuffleDeck()
        {
            // Internal variable declaration for deck length
            int deckLength = cards.Count;

            // Set deck index to -1 to signify start of the deck, since pulling next card increments deckIndex before pulling card
            this.deckIndex = -1;

            // Perform shuffle code "shuffleAmount" times
            for(int i=0; i<shuffleAmount; i++)
            {
                // Shuffle Code which swaps a card at curIndex with a randomly selected index after curIndex
                for(int curIndex=0; curIndex < deckLength; curIndex++)
                {
                    int randIndex = curIndex + rand.Next(deckLength - curIndex);
                    Card tempCard = cards[randIndex];
                    cards[randIndex] = cards[curIndex];
                    cards[curIndex] = tempCard;
                }
            }

        }
    }
}
