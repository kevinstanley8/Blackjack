using Blackjack.dto;
using Blackjack.dto.types;
using System;
using System.Collections.Generic;
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
            deck.shuffleDeck();
        }


        /**
         * startGame - Will start the blackjack game by shuffling the deck and dealing out the initial hands. 
         */
        public void startGame(Grid table)
        {
            this.table = table;
            removeCardsfromScreen();
            this.addCardToHand(PlayerType.PLAYER, true);
            this.addCardToHand(PlayerType.PLAYER, true);
            this.addCardToHand(PlayerType.DEALER, false);
            this.addCardToHand(PlayerType.DEALER, true);
        }

        /**
         * addCardToHand - Adds a new card to hand of player or dealer
         */
        public void addCardToHand(PlayerType playerType, Boolean isFaceUp)
        {
            Card nextCard = this.deck.getNextCard();
            nextCard.faceUp = isFaceUp;
            nextCard.setCardImage();

            if (playerType == PlayerType.PLAYER)
            {
                this.player.hand.Add(nextCard);
            }
            else if(playerType == PlayerType.DEALER)
            {
                this.dealer.hand.Add(nextCard);
            }

            this.addCardToScreen(playerType, nextCard);
        }

        /**
         * addCardToScreen - Adds new card image to screen
         */
        public void addCardToScreen(PlayerType playerType, Card card)
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
            else if(playerType == PlayerType.DEALER)
            {
                imageMarginLeft = (40 * this.dealer.hand.Count());
                Grid.SetRow(newCard, 0);
            }
            newCard.Margin = new Thickness(imageMarginLeft, 0, 0, 0);

            // Add card image to table's Grid view.
            table.Children.Add(newCard);
        }

        public void removeCardsfromScreen()
        {
            player.clearHand();
            dealer.clearHand();
            table.Children.Clear();
        }
    }
}
