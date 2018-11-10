using Blackjack.dto;
using Blackjack.dto.types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blackjack.service
{
    class GameService
    {
        private Deck deck { get; set; }
        public enum PlayerType { PLAYER, DEALER };
        private Player player { get; set; }
        private Dealer dealer { get; set; }

        public GameService()
        {
            this.deck = new Deck();
            this.player = new Player();
            this.dealer = new Dealer();
        }


        /**
         * startGame - Will start the blackjack game by dealing out the initial hands. 
         * 
         */
        public void startGame()
        {
            this.addCardToHand(PlayerType.PLAYER);
            this.addCardToHand(PlayerType.PLAYER);
            this.addCardToHand(PlayerType.DEALER);
            this.addCardToHand(PlayerType.DEALER);
        }

        public void addCardToHand(PlayerType playerType)
        {
            if(playerType == PlayerType.PLAYER)
            {
                this.player.hand.Add(this.deck.getNextCard());
            }
            else if(playerType == PlayerType.DEALER)
            {
                this.dealer.hand.Add(this.deck.getNextCard());
            }

            this.addCardToScreen(playerType);
        }

        public void addCardToScreen(PlayerType playerType)
        {
            //add card to screen
        }
    }
}
