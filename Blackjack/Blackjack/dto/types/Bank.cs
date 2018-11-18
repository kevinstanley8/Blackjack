using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blackjack.dto.types
{
    class Bank
    {
        private double amount { get; set; }

        public Bank(double amount)
        {
            this.amount = amount;
        }

        public void add(double amt)
        {
            this.amount += amt;
        }

        public void subtract(double amt)
        {
            this.amount -= amt;
        }
    }
}