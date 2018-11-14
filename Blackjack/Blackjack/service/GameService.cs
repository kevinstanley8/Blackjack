using Blackjack.dto;
using Blackjack.dto.types;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Blackjack.service
{
    class GameService
    {
        private Deck deck { get; set; }
        public enum PlayerType { PLAYER, DEALER };
        private Player player { get; set; }
        private Dealer dealer { get; set; }
        private Grid table { get; set; }

        public GameService()
        {
            this.deck = new Deck();
            this.player = new Player();
            this.dealer = new Dealer();
            deck.ShuffleDeck();
        }


        /**
         * startGame - Will start the blackjack game by shuffling the deck and dealing out the initial hands. 
         */
        public void StartGame(Grid table)
        {
            this.table = table;
            RemoveCardsfromScreen();
            this.AddCardToHand(PlayerType.PLAYER, true);
            this.AddCardToHand(PlayerType.PLAYER, true);
            this.AddCardToHand(PlayerType.DEALER, false);
            this.AddCardToHand(PlayerType.DEALER, true);
        }

        /**
         * addCardToHand - Adds a new card to hand of player or dealer
         */
        public void AddCardToHand(PlayerType playerType, Boolean isFaceUp)
        {
            Card nextCard = this.deck.GetNextCard();
            nextCard.faceUp = isFaceUp;
            nextCard.SetCardImage();

            if (playerType == PlayerType.PLAYER)
            {
                this.player.hand.Add(nextCard);
            }
            else if (playerType == PlayerType.DEALER)
            {
                this.dealer.hand.Add(nextCard);
            }

            this.AddCardToScreen(playerType, nextCard);
        }

        /**
         * addCardToScreen - Adds new card image to screen
         */
        public void AddCardToScreen(PlayerType playerType, Card card)
        {
            int imageMarginLeft = 0;
            Image newCard = new Image();
            ImageSource cardImage = new BitmapImage(new Uri(card.cardImage, UriKind.Relative));
            newCard.Source = cardImage;
            newCard.Width = 120;
            newCard.Height = 183;
            newCard.HorizontalAlignment = HorizontalAlignment.Center;
            newCard.VerticalAlignment = VerticalAlignment.Center;


            //stagger cards so they can all be visible based on how many cards are already out there
            if (playerType == PlayerType.PLAYER)
            {
                imageMarginLeft = (40 * this.player.hand.Count());
                Grid.SetRow(newCard, 1);
            }
            else if (playerType == PlayerType.DEALER)
            {
                imageMarginLeft = (40 * this.dealer.hand.Count());
                Grid.SetRow(newCard, 0);
            }
            newCard.Margin = new Thickness(imageMarginLeft, 0, 0, 0);

            // Add card image to table's Grid view.
            table.Children.Add(newCard);
        }

        // Clears Card Images from screen and clears player+dealer hands.
        public void RemoveCardsfromScreen()
        {
            player.ClearHand();
            dealer.ClearHand();
            table.Children.Clear();
        }

        // Flips all face-down cards in the dealer's hand to face-up.
        public void RevealDealerHand()
        {
            foreach (Card card in dealer.hand)
            {
                card.faceUp = true;
                card.SetCardImage();
            }
        }

        // Calculates the hand value of the playertype parameter. Takes into account Aces = 1 or 11.
        public int CalculateHandValue(PlayerType playerType)
        {
            int tempValue = 0;
            int numAces = 0;
            List<Card> hand;

            if (playerType == PlayerType.DEALER)
                hand = dealer.hand;
            else if (playerType == PlayerType.PLAYER)
                hand = player.hand;
            else hand = null;
            
            foreach (Card card in hand)
            {
                tempValue += card.GetNumericValue();
                if (card.value == Value.ACE)
                    numAces++;
            }
            while (numAces > 0)
            {
                if (tempValue > 21)
                {
                    tempValue -= 10;
                    numAces--;
                }
                else break;
            }
            return tempValue;
        }
    }

}
}
