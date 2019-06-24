using ModelGry;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ZaDuzoZaMalo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Game game;
        System.Windows.Threading.DispatcherTimer gameTimer; // move to ModelGry

        public MainWindow()
        {
            InitializeComponent();
        }

        private void NewGame()
        {
            this.tbFrom.IsEnabled = true;
            this.tbTo.IsEnabled = true;
            this.tbNumberToGuess.IsEnabled = true;
            this.btnStart.IsEnabled = true;
        }

        private void GameTimer_Tick(object sender, EventArgs e)
        {
            gameTimer.Stop();
            int number = this.game.MakeAttempt();
            this.lblComputerAttempt.Content = string.Format("Komputer zgaduje liczbę: {0}", number);
        }

        private void CheckParametersAndStartGame()
        {
            int from;
            int to;
            int numberToGuess;
            bool fromSuccess = int.TryParse(this.tbFrom.Text, out from);
            bool toSuccess = int.TryParse(this.tbTo.Text, out to);
            bool numberToGuessSuccess = int.TryParse(this.tbTo.Text, out numberToGuess);
            if (fromSuccess && toSuccess && numberToGuessSuccess)
            {
                if (from <= numberToGuess && numberToGuess <= to)
                {
                    this.game = new Game(from, to);

                    this.btnToLow.IsEnabled = true;
                    this.btnToHigh.IsEnabled = true;
                    this.btnCorrect.IsEnabled = true;

                    this.gameTimer = new System.Windows.Threading.DispatcherTimer();
                    gameTimer.Tick += GameTimer_Tick;
                    gameTimer.Interval = new TimeSpan(0, 0, 1);
                    gameTimer.Start();
                }
                else
                {
                    MessageBox.Show("Zakresy oraz liczba do odgnadnięcia muszą spełniac nierówność: OD <= LICZBA <= DO", "Błąd");
                }
            }
            else
            {
                MessageBox.Show("Podaj poprawne zakresy oraz liczbę do odgadnięcia", "Błąd");
            }
        }

        private void ShowHistory()
        {
            if (this.game != null)
            {
                StringBuilder sb = new StringBuilder();
                foreach (Attempt attempt in this.game.History)
                {
                    sb.AppendLine(string.Format("Propozycja: {0} -> {1}", attempt.Number, attempt.Answer));
                }
                MessageBox.Show(sb.ToString(), "Historia");
            }
        }

        #region GUI Events

        private void btnNewGame_Click(object sender, RoutedEventArgs e)
        {
            NewGame();
        }

        private void btnHistory_Click(object sender, RoutedEventArgs e)
        {
            ShowHistory();
        }  

        private void btnInfo_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Gra 'Za duzo za mało' - Wersja: WPF, zgaduje komputer\r\n\r\nAutor: Krzysztof Szczudło", "Info");
        }

        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            CheckParametersAndStartGame();
        }

        private void btnToLow_Click(object sender, RoutedEventArgs e)
        {
            this.game.RegisterAttempt(Answer.ToLow);
            this.gameTimer.Start();
        }

        private void btnToHigh_Click(object sender, RoutedEventArgs e)
        {
            this.game.RegisterAttempt(Answer.ToHigh);
            this.gameTimer.Start();
        }

        private void btnCorrect_Click(object sender, RoutedEventArgs e)
        {
            this.game.RegisterAttempt(Answer.Correct);

            this.gameTimer.Stop();
            this.tbFrom.IsEnabled = false;
            this.tbTo.IsEnabled = false;
            this.tbNumberToGuess.IsEnabled = false;
            this.btnStart.IsEnabled = false;
            this.btnToLow.IsEnabled = false;
            this.btnToHigh.IsEnabled = false;
            this.btnCorrect.IsEnabled = false;

            ShowHistory();
        }

        #endregion
    }
}
