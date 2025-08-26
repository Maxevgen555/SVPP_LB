using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using K19GameLibrary;

namespace K19
{
    public partial class MainWindow : Window
    {
        private ShipTorpedoGame game;

        public MainWindow()
        {
            InitializeComponent();

            // Создаем экземпляр игры и устанавливаем как контекст данных
            game = new ShipTorpedoGame();
            game.GameMessage += Game_GameMessage;
            DataContext = game;
        }

        private void BtnStart_Click(object sender, RoutedEventArgs e)
        {
            game.StartGame();
            txtInfo.Text = "Игра запущена!";
        }

        private void BtnPause_Click(object sender, RoutedEventArgs e)
        {
            game.PauseGame();
            txtInfo.Text = "Игра на паузе";
        }

        private void BtnReset_Click(object sender, RoutedEventArgs e)
        {
            game.ResetGame();
            txtInfo.Text = "Игра сброшена";
        }

        private void BtnLeft_Click(object sender, RoutedEventArgs e)
        {
            game.MovePeriscopeLeft();
        }

        private void BtnRight_Click(object sender, RoutedEventArgs e)
        {
            game.MovePeriscopeRight();
        }

        private void BtnFire_Click(object sender, RoutedEventArgs e)
        {
            game.LaunchTorpedo();
            txtInfo.Text = "Торпеда выпущена!";
        }

        private void Game_GameMessage(string message)
        {
            txtInfo.Text = message;
        }
    }
}