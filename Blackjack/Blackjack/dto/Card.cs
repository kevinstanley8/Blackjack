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
        public Boolean faceUp { get; set; }

        public Card(Value value, Suit suit)
        {
            this.value = value;
            this.suit = suit;
            faceUp = true;
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
         * getNumericValue - returns the String equivalent of the card value so we can get the card image
         */
        public String getStringValue()
        {
            if (this.value == Value.ACE || this.value == Value.KING || this.value == Value.QUEEN || this.value == Value.JACK)
            {
                switch (this.value)
                {
                    case Value.JACK:
                        return "J";
                    case Value.QUEEN:
                        return "Q";
                    case Value.KING:
                        return "K";
                    case Value.ACE:
                        return "A";
                    default:
                        return "";
                }
            }
            else
            {
                return this.getNumericValue().ToString();
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
                    return "H";
                case Suit.DIAMOND:
                    return "D";
                case Suit.CLUB:
                    return "C";
                case Suit.SPADE:
                    return "S";
                default:
                    return "";
            }
        }

        /**
         * setCardImage - sets the image path of the card 
         */
        public void setCardImage()
        {
            String path = "../Images/";
            if (faceUp)
                path = path + getStringValue() + getSuitValue() + ".png";
            else
                path = path + "gray_back" + ".png";
            this.cardImage = path;
        }

        public override string ToString()
        {
            return getStringValue() + getSuitValue();
        }

    }
}
