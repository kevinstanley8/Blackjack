using Blackjack.service;
using System;
using System.Collections.Generic;
using System.Globalization;
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
    /// Interaction logic for Table.xaml
    /// </summary>
    public partial class Table : Window
    {
        private GameService gameService { get; set; }
        private Double currentBet { get; set; }

        public Table()
        {
            InitializeComponent();
            currentBet = 1;
            UpdateBetText();
            this.gameService = new GameService();
            this.SetPlayBtnsEnabled(false);
        }

        // Sets the Hit, Stay, Split, and Double Down buttons enabled/disabled based on the bool parameter
        public void SetPlayBtnsEnabled(Boolean enabledStatus)
        {
            btnHit.IsEnabled = enabledStatus;
            btnStay.IsEnabled = enabledStatus;
            btnSplit.IsEnabled = enabledStatus;
            btnDoubleDown.IsEnabled = enabledStatus;
        }

        public void UpdateBetText()
        {
            lblBetAmount.Content = String.Format("${0:n0}", currentBet);
        }

        private void btnHit_Click(object sender, RoutedEventArgs e)
        {
            this.gameService.AddCardToHand(GameService.PlayerType.PLAYER, true);
            if (gameService.CheckBust(GameService.PlayerType.PLAYER))
                DisplayLoseDialog();
        }

        private void btnStay_Click(object sender, RoutedEventArgs e)
        {
            // Temporary Logic
            SetPlayBtnsEnabled(false);
            gameService.BeginDealerDraw();
            if (gameService.CheckDraw())
                DisplayDrawDialog();
            else if (gameService.CheckBust(GameService.PlayerType.DEALER) || gameService.CheckWin())
                DisplayWinDialog();
            else
                DisplayLoseDialog();
        }

        private void btnSplit_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnDoubleDown_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnNewHand_Click(object sender, RoutedEventArgs e)
        {
            // Temporary Logic
            SetPlayBtnsEnabled(true);
            btnNewHand.IsEnabled = false;
            gameService.StartGame(gridInnerCenter);
        }

        private void btnBetMinus_Click(object sender, RoutedEventArgs e)
        {
            currentBet = (Double)Decimal.Parse((string)lblBetAmount.Content, NumberStyles.AllowCurrencySymbol | NumberStyles.Number);
            if (currentBet <= 1) return;
            else currentBet -= 1;
            lblBetAmount.Content = String.Format("${0:n0}", currentBet);
        }

        private void btnBetPlus_Click(object sender, RoutedEventArgs e)
        {
            currentBet = (Double)Decimal.Parse((string)lblBetAmount.Content, NumberStyles.AllowCurrencySymbol | NumberStyles.Number);
            if (currentBet >= 50) return;
            else currentBet += 1;
            lblBetAmount.Content = String.Format("${0:n0}", currentBet);
        }

        public void DisplayLoseDialog()
        {
            MessageBox.Show("You Lose!");
            btnNewHand.IsEnabled = true;
        }

        public void DisplayWinDialog()
        {
            MessageBox.Show("You Win!");
            btnNewHand.IsEnabled = true;
        }

        public void DisplayDrawDialog()
        {
            MessageBox.Show("Draw!");
            btnNewHand.IsEnabled = true;
        }
    }
}
