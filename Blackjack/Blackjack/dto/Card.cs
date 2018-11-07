using Blackjack.dto.types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blackjack.dto
{
    class Card
    {
        public Value value { get; set; }
        public Suit suit { get; set; }
        public String cardImage { get; set; }

        public Card(Value value, Suit suit)
        {
            this.value = value;
            this.suit = suit;
            this.setCardImage();
        }

        /**
         * getNumericValue - returns the integer equivalent of the card value 
         */
        public int getNumericValue()
        {
            switch(this.value)
            {
                case Value.TWO:
                    return 2;
                case Value.THREE:
                    return 3;
                case Value.FOUR:
                    return 4;
                case Value.FIVE:
                    return 5;
                case Value.SIX:
                    return 6;
                case Value.SEVEN:
                    return 7;
                case Value.EIGHT:
                    return 8;
                case Value.NINE:
                    return 9;
                case Value.TEN:
                    return 10;
                case Value.JACK:
                    return 10;
                case Value.QUEEN:
                    return 10;
                case Value.KING:
                    return 10;
                case Value.ACE:
                    return 1;
                default:
                    return 0;
            }
        }

        /**
         * getSuitValue - returns the string equivalent of the card suit 
         */
        public String getSuitValue()
        {
            switch(this.suit)
            {
                case Suit.HEART:
                    return "HEART";
                case Suit.DIAMOND:
                    return "DIAMOND";
                case Suit.CLUB:
                    return "CLUB";
                case Suit.SPADE:
                    return "SPADE";
                default:
                    return "";
            }
        }

        /**
         * setCardImage - sets the image path of the card 
         */
        private void setCardImage()
        {
            //this is just a guess so we will need to confirm the card image path when we get the images loaded.
            String path = "/Images/";

            path = path + getNumericValue().ToString() + getSuitValue() + ".png";
            this.cardImage = path;
        }
        
    }
}
