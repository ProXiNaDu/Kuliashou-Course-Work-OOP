using System;
using System.Windows;
using OpenTK.Graphics.OpenGL;
using OpenTK.Wpf;
using GameEngineLibrary;
using GameLibrary;
using GameLibrary.Scenes;
using System.Windows.Controls;
using System.Threading;

namespace GameUserInterface
{
    /// <summary>
    /// Логика взаимодействия для главного окна игры.
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Сервер
        /// </summary>
        private Server server;

        /// <summary>
        /// Клиент
        /// </summary>
        private Client client;

        /// <summary>
        /// Настройки боевой сцены
        /// </summary>
        private BattleSceneSettings settings;

        /// <summary>
        /// Сцена, которая будет отрисовываться на экране.
        /// </summary>
        private Scene scene;

        /// <summary>
        /// Отрисовщик сцены.
        /// </summary>
        private Renderer renderer;

        /// <summary>
        /// Создание окна приложения.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();

            InitStartScreen();
            scene = new BattleScene(this, settings);

            MainMenu.Visibility = Visibility.Visible;
            FirstPanzerInfo.Visibility = Visibility.Hidden;
            SecondPanzerInfo.Visibility = Visibility.Hidden;
            RocketShop.Visibility = Visibility.Hidden;
            WinMenu.Visibility = Visibility.Hidden;

            var WpfControlSettings = new GLWpfControlSettings();
            WpfControlSettings.MajorVersion = 3;
            WpfControlSettings.MinorVersion = 6;
            OpenTKControl.Start(WpfControlSettings);
        }

        private void OpenTKControl_Render(TimeSpan delta)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            GL.Ortho(-Width, Width, Height, -Height, 0d, 1d);

            scene.Update(delta);
            renderer.Render();
        }

        private void OpenTKControl_Ready()
        {
            GL.Enable(EnableCap.Texture2D);
            GL.Enable(EnableCap.Blend);

            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);

            scene.Init();
            renderer = new Renderer(scene);
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (client != null)
            {
                client.DisconnectFromServer();
            }

            client?.Close();
            server?.Close();
            scene.Dispose();
            GL.Disable(EnableCap.Blend);
            GL.Disable(EnableCap.Texture2D);
        }

        private void PlayGameBtn_Click(object sender, RoutedEventArgs e)
        {
            MainMenu.Visibility = Visibility.Hidden;
            RocketShop.Visibility = Visibility.Visible;
        }

        private void QuitGameBtn_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void StartGameBtn_Click(object sender, RoutedEventArgs e)
        {
            RocketShop.Visibility = Visibility.Hidden;
            FirstPanzerInfo.Visibility = Visibility.Visible;
            SecondPanzerInfo.Visibility = Visibility.Visible;

            scene.Dispose();

            settings = new BattleSceneSettings();
            int firstPanzerPowerfulRockets = int.Parse(FirstPanzerPowerfulRockets.Content.ToString());
            int firstPanzerFastRockets = int.Parse(FirstPanzerFastRockets.Content.ToString());
            int firstPanzerRockets = int.Parse(FirstPanzerRockets.Content.ToString());
            int secondPanzerPowerfulRockets = int.Parse(SecondPanzerPowerfulRockets.Content.ToString());
            int secondPanzerFastRockets = int.Parse(SecondPanzerFastRockets.Content.ToString());
            int secondPanzerRockets = int.Parse(SecondPanzerRockets.Content.ToString());
            settings.SetFirstPanzerAmounts(firstPanzerPowerfulRockets, firstPanzerFastRockets, firstPanzerRockets);
            settings.SetSecondPanzerAmounts(secondPanzerPowerfulRockets, secondPanzerFastRockets, secondPanzerRockets);
            settings.FirstPanzerHealth = 100;
            settings.SecondPanzerHealth = 100;
            settings.FirstPanzerControlType = (bool)IsFirstAI.IsChecked ?
                BattleSceneSettings.PanzerControlType.AI :
                BattleSceneSettings.PanzerControlType.Keyboard;
            settings.SecondPanzerControlType = (bool)IsSecondAI.IsChecked ?
                BattleSceneSettings.PanzerControlType.AI :
                BattleSceneSettings.PanzerControlType.Keyboard;
            scene = new BattleScene(this, settings);
            scene.Init();
            renderer = new Renderer(scene);
        }

        private void BuyBtn_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            Grid grid = button.Parent as Grid;
            Label outputLabel = grid.Children[0] as Label;
            Label moneyLable = (grid.Parent as Grid).Children[0] as Label;

            int amount = int.Parse(button.Content.ToString());
            int value = int.Parse(outputLabel.Content.ToString());
            int money = int.Parse(moneyLable.Content.ToString());
            int cost = int.Parse((grid.Children[1] as Label).Content.ToString()) * amount;

            if (value + amount >= 0 && money - cost >= 0)
            {
                value += amount;
                money -= cost;

                outputLabel.Content = value.ToString();
                moneyLable.Content = money.ToString();
            }
        }

        private void RestartGameBtn_Click(object sender, RoutedEventArgs e)
        {
            WinMenu.Visibility = Visibility.Hidden;
            scene.Dispose();
            scene = new BattleScene(this, settings);
            scene.Init();
            renderer = new Renderer(scene);
        }

        private void MainMenuBtn_Click(object sender, RoutedEventArgs e)
        {
            WinMenu.Visibility = Visibility.Hidden;
            MainMenu.Visibility = Visibility.Visible;
            FirstPanzerInfo.Visibility = Visibility.Hidden;
            SecondPanzerInfo.Visibility = Visibility.Hidden;

            scene.Dispose();
            InitStartScreen();
            scene = new BattleScene(this, settings);
            scene.Init();
            renderer = new Renderer(scene);
        }

        private void InitStartScreen()
        {
            settings = new BattleSceneSettings();
            settings.SetFirstPanzerAmounts(int.MaxValue, int.MaxValue, int.MaxValue);
            settings.SetSecondPanzerAmounts(int.MaxValue, int.MaxValue, int.MaxValue);
            settings.FirstPanzerHealth = int.MaxValue;
            settings.SecondPanzerHealth = int.MaxValue;
            settings.FirstPanzerControlType = BattleSceneSettings.PanzerControlType.AI;
            settings.SecondPanzerControlType = BattleSceneSettings.PanzerControlType.AI;
        }

        private void MultiplayerBtn_Click(object sender, RoutedEventArgs e)
        {
            MainMenu.Visibility = Visibility.Hidden;
            ChooseMultiplayerTypeMenu.Visibility = Visibility.Visible;
        }

        private void HostGameBtn_Click(object sender, RoutedEventArgs e)
        {
            ChooseMultiplayerTypeMenu.Visibility = Visibility.Hidden;

            StartServer();
            client = ConnectTo("localhost:4748");

            if (client == null)
            {
                MessageBox.Show("Не получилось создать сервер");
                server.Close();
                ChooseMultiplayerTypeMenu.Visibility = Visibility.Visible;
                return;
            }

            var secondPlayerListenerThread = new Thread(new ThreadStart(ListenSecondPlayer));
            secondPlayerListenerThread.Start();

            StartMultiplayerGameBtn.IsEnabled = false;
            MultiplayerRocketShop.Visibility = Visibility.Visible;
        }

        private void ConnectBtn_Click(object sender, RoutedEventArgs e)
        {
            ChooseMultiplayerTypeMenu.Visibility = Visibility.Hidden;
            ConnectMenu.Visibility = Visibility.Visible;
        }

        private void ConnectToServerBtn_Click(object sender, RoutedEventArgs e)
        {
            ConnectMenu.Visibility = Visibility.Hidden;

            client = ConnectTo(ServerAddressInput.Text);

            if (client == null)
            {
                MessageBox.Show("Не получилось подключиться к серверу");
                ConnectMenu.Visibility = Visibility.Visible;
                return;
            }

            StartMultiplayerGameBtn.Content = "Ready";
            IsSecondPlayerReadyLabel.Content = "Second Player (CONNECTED)";
            MultiplayerRocketShop.Visibility = Visibility.Visible;
        }

        private void StartServer()
        {
            server = new Server();
        }

        private void ListenSecondPlayer()
        {
            var connectedUsers = client.GetConnectedUsersCount();

            while (connectedUsers < 2 && !server.IsClosed())
            {
                Thread.Sleep(100);
                connectedUsers = client.GetConnectedUsersCount();
            }
            if (server.IsClosed()) return;

            Dispatcher.Invoke(() => IsSecondPlayerReadyLabel.Content = "Second Player (CONNECTED)");

            var isSecondPlayerReady = false;

            while (!isSecondPlayerReady && !server.IsClosed())
            {
                Thread.Sleep(100);
                isSecondPlayerReady = client.IsSecondReady();
            }
            if (server.IsClosed()) return;

            Dispatcher.Invoke(() => StartMultiplayerGameBtn.IsEnabled = true);
        }

        private Client ConnectTo(string address)
        {
            var client = new Client(address);
            if (client.ConnectToServer())
            {
                return client;
            }
            else
            {
                client.Close();
                return null;
            }
        }

        private void StartMultiplayerGameBtn_Click(object sender, RoutedEventArgs e)
        {
            StartMultiplayerGameBtn.IsEnabled = false;

            int powerfulRockets = int.Parse(MultiplayerPanzerPowerfulRockets.Content.ToString());
            int fastRockets = int.Parse(MultiplayerPanzerFastRockets.Content.ToString());
            int rockets = int.Parse(MultiplayerPanzerRockets.Content.ToString());

            if (server == null)
            {
                client.SetSecondPanzerAmounts(powerfulRockets, fastRockets, rockets);
            }
            else
            {
                client.SetFirstPanzerAmounts(powerfulRockets, fastRockets, rockets);
            }

            MessageBox.Show("Игра началась");
        }
    }
}
