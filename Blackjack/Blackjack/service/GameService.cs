using Blackjack.dto;
using Blackjack.dto.types;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.IO;
using Newtonsoft.Json;

namespace Blackjack.service
{
    class GameService
    {
        public Deck deck { get; set; }
        public enum PlayerType { PLAYER, DEALER };
        public Player player { get; set; }
        public Dealer dealer { get; set; }
        public Grid table { get; set; }
        public Bank bank { get; set; }

        public GameService()
        {
            this.deck = new Deck();
            this.player = new Player();
            this.dealer = new Dealer();
            this.InitalizeBank();
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
            
            //stagger cards so they can all be visible based on how many cards are already out there
            if (playerType == PlayerType.PLAYER)
            {
                imageMarginLeft = (40 * this.player.hand.Count());
                Grid.SetRow(card.cardImage, 1);
            }
            else if (playerType == PlayerType.DEALER)
            {
                imageMarginLeft = (40 * this.dealer.hand.Count());
                Grid.SetRow(card.cardImage, 0);
            }
            card.cardImage.Margin = new Thickness(imageMarginLeft, 0, 0, 0);

            // Add card image to table's Grid view.
            table.Children.Add(card.cardImage);
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

        public Boolean CheckDraw()
        {
            return CalculateHandValue(PlayerType.PLAYER) == CalculateHandValue(PlayerType.DEALER);
        }

        public Boolean CheckWin()
        {
            return CalculateHandValue(PlayerType.PLAYER) > CalculateHandValue(PlayerType.DEALER);
        }

        public Boolean CheckBust(PlayerType playerType)
        {
            return CalculateHandValue(playerType) > 21;
        }

        public void BeginDealerDraw()
        {
            RevealDealerHand();
            while (CalculateHandValue(PlayerType.DEALER) < 17)
            {
                AddCardToHand(PlayerType.DEALER, true);
            }
        }

        public void ProcessHandResult(HandResult handResult, double betAmount)
        {
            switch(handResult)
            {
                case HandResult.WIN:
                    this.bank.add(betAmount);
                    break;
                case HandResult.LOSE:
                    this.bank.subtract(betAmount);
                    break;
                case HandResult.DRAW:
                    //do nothing on DRAW.  Player gets money back so no addition or subtraction needed
                    break;
                case HandResult.BLACKJACK:
                    //if player gets Blackjack they get 3:2 payout
                    this.bank.add(betAmount * 1.5);
                    break;
            }

            WriteBankToJsonFile();
        }


        /**
         * WriteBankToJsonFile - Using Newtonsoft.Json package to serialize Bank obj to JSON txt file and write the file to hard drive.
         */
        public void WriteBankToJsonFile()
        {
            try
            {
                String projectDir = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;

                //using Newtonsoft.Json package to manage JSON
                using (StreamWriter file = File.CreateText(projectDir + "/bank.txt"))
                {
                    JsonSerializer serializer = new JsonSerializer();
                    serializer.Serialize(file, this.bank);
                }
            } catch(Exception e)
            {

            }
        }

        /**
         * InitalizeBank - Initalize Bank either from a preexisting JSON txt file or setup default bank if JSON file doesn't exist. 
         */
        public void InitalizeBank()
        {
            try
            {
                String projectDir = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;

                using (StreamReader file = File.OpenText(projectDir + "/bank.txt"))
                {
                    JsonSerializer serializer = new JsonSerializer();
                    this.bank = (Bank)serializer.Deserialize(file, typeof(Bank));
                }
            } catch(Exception e)
            {
                //no bank exists yet
                this.bank = new Bank(100.00);
            }
        }
    }

}
