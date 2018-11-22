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
        private Double originalBet { get; set; }
        private Boolean audioEnabled { get; set; }
        private Boolean isSplit { get; set; }
        private int currentHand { get; set; }
        public Boolean processedHandOne = false;
        public Boolean processedHandTwo = false;
        Boolean[] processedHand = { false, false };

        public Table()
        {
            InitializeComponent();
            currentBet = 1;
            originalBet = 1;
            UpdateBetText();
            this.audioEnabled = true;
            this.isSplit = false;
            this.currentHand = 0;
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
            if (this.isSplit && this.currentHand == 0)
                this.currentHand++;
            else if(this.isSplit)
            {
                //this is a split hand and player has lost of stay on last hand
                SetPlayBtnsEnabled(false);
                gameService.BeginDealerDraw();

                //check 1st hand
                if(!this.processedHand[0])
                    this.stayHand(0);

                //check second hand
                if (!this.processedHand[1])
                    this.stayHand(1);
            }
            else
            {
                SetPlayBtnsEnabled(false);
                gameService.BeginDealerDraw();
                this.stayHand(0);
            }
        }

        private void stayHand(int handIndex)
        {
            if (gameService.CheckDraw(handIndex))
                DisplayDrawDialog(handIndex);
            else if (gameService.CheckBust(GameService.PlayerType.DEALER, handIndex) || gameService.CheckWin(handIndex))
                this.winHand(HandResult.WIN, handIndex);
            else
                this.loseHand(handIndex);
        }

        private void winHand(HandResult handResult, int handIndex)
        {
            this.processedHand[handIndex] = true;
            this.gameService.ProcessHandResult(handResult, this.currentBet);
            this.RefreshBankAmountOnScreen();
            DisplayWinDialog(handIndex);
        }

        private void loseHand(int handIndex)
        {
            this.processedHand[handIndex] = true;
            this.gameService.ProcessHandResult(HandResult.LOSE, this.currentBet);
            this.RefreshBankAmountOnScreen();
            DisplayLoseDialog(handIndex);
        }

        private void PlayerHit(int handIndex)
        {
            this.gameService.AddCardToHand(GameService.PlayerType.PLAYER, true, this.isSplit, this.currentHand);
            if (gameService.CheckBust(GameService.PlayerType.PLAYER, this.currentHand))
                this.loseHand(handIndex);
            if(this.isSplit)
                this.CheckBlackjack(handIndex);
        }

        private void PlayerDoubleDown()
        {
            if (currentBet * 2 > gameService.bank.amount)
            {
                MessageBox.Show("You don't have enough money!");
                return;
            }

            if (this.isSplit)
            {
                MessageBox.Show("You can't Double Down on a Split hand!");
                return;
            }

            SetPlayBtnsEnabled(false);
            this.originalBet = this.currentBet;
            this.SetBet(currentBet * 2);
            this.PlayerHit(this.currentHand);

            //if player didn't already lose
            if(!btnNewHand.IsEnabled)
                this.PlayerStay();

            //reset original bid so it doesn't keep doubling
            this.SetBet(this.originalBet);
        }

        private void btnHit_Click(object sender, RoutedEventArgs e)
        {
            this.PlayerHit(this.currentHand);
        }

        private void btnStay_Click(object sender, RoutedEventArgs e)
        {
            // Temporary Logic
            this.PlayerStay();
        }

        private void btnSplit_Click(object sender, RoutedEventArgs e)
        {
            if (this.gameService.canSplit())
            {
                this.isSplit = true;
                this.gameService.SplitHand();
            }
            else
                MessageBox.Show("You can't Split on this hand!  Card must be the same to Split!");
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
            this.isSplit = false;
            this.currentHand = 0;
            this.processedHand[0] = false;
            this.processedHand[1] = false;
            gameService.StartGame(gridInnerCenter);
            this.CheckBlackjack(this.currentHand);
        }

        /**
         * CheckBlackjack - Check to see if player has Blackjack.  If both dealer and player have Blackjack then it is a draw.
         */
        private void CheckBlackjack(int handIndex)
        {
            if (gameService.CheckBlackjack(GameService.PlayerType.PLAYER, handIndex) && gameService.CheckBlackjack(GameService.PlayerType.DEALER, handIndex))
            {
                this.gameService.RevealDealerHand();
                this.DisplayDrawDialog(handIndex);
            }
            else if (gameService.CheckBlackjack(GameService.PlayerType.PLAYER, handIndex))
            {
                this.gameService.RevealDealerHand();
                this.winHand(HandResult.BLACKJACK, handIndex);
            }
            else if (gameService.CheckBlackjack(GameService.PlayerType.DEALER, handIndex))
            {
                this.gameService.RevealDealerHand();
                this.loseHand(handIndex);
            }
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

        public void DisplayLoseDialog(int handIndex)
        {
            this.PlayLoseAudio();
            if(this.isSplit)
                MessageBox.Show("You Lose hand " + (handIndex + 1) + "!");
            else
                MessageBox.Show("You Lose!");

            if (!this.isSplit || this.currentHand >= 1)
            {
                btnNewHand.IsEnabled = true;
                this.SetBetBtnsEnabled(true);
            }
            else if (this.currentHand == 0)
                this.currentHand++;
        }

        public void DisplayWinDialog(int handIndex)
        {
            this.PlayWinAudio();
            if (this.isSplit)
                MessageBox.Show("You Win hand " + (handIndex + 1) + "!");
            else
                MessageBox.Show("You Win!");

            if (!this.isSplit || this.currentHand >= 1)
            {
                btnNewHand.IsEnabled = true;
                this.SetBetBtnsEnabled(true);
            }
            else
                this.currentHand++;
        }

        public void DisplayDrawDialog(int handIndex)
        {
            if (this.isSplit)
                MessageBox.Show("Draw hand " + (handIndex + 1) + "!");
            else
                MessageBox.Show("Draw!");

            if (!this.isSplit || this.currentHand >= 1)
            {
                btnNewHand.IsEnabled = true;
                this.SetBetBtnsEnabled(true);
            }
            else
                this.currentHand++;
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
