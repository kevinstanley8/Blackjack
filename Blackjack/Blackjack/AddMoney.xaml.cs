using Blackjack.dto.types;
using Blackjack.service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Blackjack
{
    /// <summary>
    /// Interaction logic for AddMoney.xaml
    /// </summary>
    public partial class AddMoney : Window
    {
        public GameService gameService;

        public AddMoney(GameService gameService)
        {
            this.gameService = gameService;
            InitializeComponent();
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            int amount = 0;
            try
            {
                Int32.TryParse(this.txtAmount.Text, out amount);
                this.gameService.bank.add(amount);
                ((Table)this.Owner).RefreshBankAmountOnScreen();
                this.Close();
            } catch(Exception)
            {

            }
            
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
