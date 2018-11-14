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
    /// Interaction logic for Table.xaml
    /// </summary>
    public partial class Table : Window
    {
        private GameService gameService { get; set; }

        public Table()
        {
            InitializeComponent();
            this.gameService = new GameService();
            this.setPlayBtnsEnabled(false);
        }

        // Sets the Hit, Stay, Split, and Double Down buttons enabled/disabled based on the bool parameter
        public void setPlayBtnsEnabled(Boolean enabledStatus)
        {
            btnHit.IsEnabled = enabledStatus;
            btnStay.IsEnabled = enabledStatus;
            btnSplit.IsEnabled = enabledStatus;
            btnDoubleDown.IsEnabled = enabledStatus;
        }

        private void btnHit_Click(object sender, RoutedEventArgs e)
        {
            this.gameService.addCardToHand(GameService.PlayerType.PLAYER, true);
        }

        private void btnStay_Click(object sender, RoutedEventArgs e)
        {
            // Temporary Logic
            setPlayBtnsEnabled(false);
            btnNewHand.IsEnabled = true;
            gameService.removeCardsfromScreen();
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
            setPlayBtnsEnabled(true);
            btnNewHand.IsEnabled = false;
            gameService.startGame(gridInnerCenter);
        }

        private void btnBetMinus_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnBetPlus_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
