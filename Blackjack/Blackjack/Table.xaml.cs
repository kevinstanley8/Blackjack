using Blackjack.dto.types;
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
        private Boolean audioEnabled { get; set; }

        public Table()
        {
            InitializeComponent();
            currentBet = 1;
            UpdateBetText();
            this.audioEnabled = true;
            this.gameService = new GameService();
            this.SetPlayBtnsEnabled(false);
            this.RefreshBankAmountOnScreen();
        }

        // Sets the Hit, Stay, Split, and Double Down buttons enabled/disabled based on the bool parameter
        public void SetPlayBtnsEnabled(Boolean enabledStatus)
        {
            btnHit.IsEnabled = enabledStatus;
            btnStay.IsEnabled = enabledStatus;
            btnSplit.IsEnabled = enabledStatus;
            btnDoubleDown.IsEnabled = enabledStatus;
        }

        /**
         * SetBetBtnsEnabled - sets bet plus and minus buttons enabled/disabled
         */
        public void SetBetBtnsEnabled(Boolean enabled)
        {
            this.btnBetPlus.IsEnabled = enabled;
            this.btnBetMinus.IsEnabled = enabled;
        }

        public void UpdateBetText()
        {
            lblBetAmount.Content = String.Format("${0:n0}", currentBet);
        }

        /**
         * PlayerStay - holds all logic that will be used when a player stays.  This logic will also be reused for when a player Doubles Down.
         */
        private void PlayerStay()
        {
            SetPlayBtnsEnabled(false);
            gameService.BeginDealerDraw();
            if (gameService.CheckDraw())
                DisplayDrawDialog();
            else if (gameService.CheckBust(GameService.PlayerType.DEALER) || gameService.CheckWin())
            {
                if(gameService.CalculateHandValue(GameService.PlayerType.PLAYER) == 21)
                    this.gameService.ProcessHandResult(HandResult.BLACKJACK, currentBet);
                else
                    this.gameService.ProcessHandResult(HandResult.WIN, this.currentBet);
                this.RefreshBankAmountOnScreen();
                DisplayWinDialog();
            }
            else
            {
                this.gameService.ProcessHandResult(HandResult.LOSE, this.currentBet);
                this.RefreshBankAmountOnScreen();
                DisplayLoseDialog();
            }
        }

        private void PlayerHit()
        {
            this.gameService.AddCardToHand(GameService.PlayerType.PLAYER, true);
            if (gameService.CheckBust(GameService.PlayerType.PLAYER))
            {
                this.gameService.ProcessHandResult(HandResult.LOSE, this.currentBet);
                this.RefreshBankAmountOnScreen();
                DisplayLoseDialog();
            }
        }

        private void PlayerDoubleDown()
        {
            if (currentBet * 2 > gameService.bank.amount)
            {
                MessageBox.Show("You don't have enough money!");
                return;
            }
            SetPlayBtnsEnabled(false);
            this.SetBet(currentBet * 2);
            this.PlayerHit();

            //if player didn't already lose
            if(!btnNewHand.IsEnabled)
                this.PlayerStay();
        }

        private void btnHit_Click(object sender, RoutedEventArgs e)
        {
            this.PlayerHit();
        }

        private void btnStay_Click(object sender, RoutedEventArgs e)
        {
            // Temporary Logic
            this.PlayerStay();
        }

        private void btnSplit_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void btnDoubleDown_Click(object sender, RoutedEventArgs e)
        {
            this.PlayerDoubleDown();
        }

        private void btnNewHand_Click(object sender, RoutedEventArgs e)
        {
            this.mediaWin.Stop();
            this.mediaLose.Stop();
            if (currentBet > gameService.bank.amount)
            {
                MessageBox.Show("You don't have enough money!");
                return;
            }
            SetPlayBtnsEnabled(true);
            this.SetBetBtnsEnabled(false);
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
            if (currentBet >= gameService.bank.amount || currentBet >= 50)
                return;
            else currentBet += 1;
            lblBetAmount.Content = String.Format("${0:n0}", currentBet);
        }

        private void SetBet(Double amount)
        {
            currentBet = amount;
            if (currentBet <= 1)
                return;
            lblBetAmount.Content = String.Format("${0:n0}", currentBet);
        }

        public void DisplayLoseDialog()
        {
            this.PlayLoseAudio();
            MessageBox.Show("You Lose!");
            btnNewHand.IsEnabled = true;
            this.SetBetBtnsEnabled(true);
        }

        public void DisplayWinDialog()
        {
            this.PlayWinAudio();
            MessageBox.Show("You Win!");
            btnNewHand.IsEnabled = true;
            this.SetBetBtnsEnabled(true);
        }

        public void DisplayDrawDialog()
        {
            MessageBox.Show("Draw!");
            btnNewHand.IsEnabled = true;
            this.SetBetBtnsEnabled(true);
        }

        public void RefreshBankAmountOnScreen()
        {
            this.lblBankAmount.Content = "$" + this.gameService.bank.getBankAmount();
        }

        public void PlayWinAudio()
        {
            if (this.audioEnabled)
            {
                this.mediaWin.Play();
            }
        }

        public void PlayLoseAudio()
        {
            if (this.audioEnabled)
            {
                this.mediaLose.Play();
            }
        }

        private void imgAudioOn_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if(this.audioEnabled)
            {
                this.audioEnabled = false;
                ImageSource imgSource = new BitmapImage(new Uri("../Images/audio_off.png", UriKind.Relative));
                this.imgAudio.Source = imgSource;
            }
            else
            {
                this.audioEnabled = true;
                ImageSource imgSource = new BitmapImage(new Uri("../Images/audio_on.png", UriKind.Relative));
                this.imgAudio.Source = imgSource;
            }
        }
    }
}
