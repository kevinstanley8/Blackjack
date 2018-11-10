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
        }


        /**
         * startGame - Will start the blackjack game by dealing out the initial hands. 
         */
        public void startGame(Grid table)
        {
            this.table = table;
            this.addCardToHand(PlayerType.PLAYER);
            this.addCardToHand(PlayerType.PLAYER);
            this.addCardToHand(PlayerType.DEALER);
            this.addCardToHand(PlayerType.DEALER);
        }

        /**
         * addCardToHand - Adds a new card to hand of player or dealer
         */
        public void addCardToHand(PlayerType playerType)
        {
            Card nextCard = this.deck.getNextCard();

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
            int imageTop = 172;
            int imageLeft = 500;

            //stagger cards so they can all be visible based on how many cards are already out there
            if(playerType == PlayerType.PLAYER)
            {
                imageTop = 300;
                imageLeft = imageLeft + (20 * this.player.hand.Count());
            }
            else if(playerType == PlayerType.DEALER)
            {
                imageTop = 100;
                imageLeft = imageLeft + (20 * this.dealer.hand.Count());
            }

            //add card to screen
            Image newCard = new Image();
            newCard.Width = 130;
            newCard.Height = 130;
            newCard.Margin = new Thickness(imageLeft, imageTop, 0, 0);
            newCard.HorizontalAlignment = HorizontalAlignment.Left;
            newCard.VerticalAlignment = VerticalAlignment.Top;
            ImageSource cardImage = new BitmapImage(new Uri(card.cardImage, UriKind.Relative));
            newCard.Source = cardImage;
            this.table.Children.Add(newCard);
        }
    }
}
